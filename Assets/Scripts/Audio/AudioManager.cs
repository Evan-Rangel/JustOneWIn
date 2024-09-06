using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource masterSource;
    [SerializeField] AudioMixer mixer;
    public static AudioManager instance;
    public bool masterMute { get; private set; }=false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        CheckAudioSources("Master");
        CheckAudioSources("SFX");
        CheckAudioSources("Music");
    }
    //Al iniciar, setear el volumen o mutear el canal
    void CheckAudioSources(string _channelName)
    {
        if (PlayerPrefs.HasKey(_channelName+"Volume"))
        {
            float volume = PlayerPrefs.GetFloat(_channelName + "Volume");
            if (volume > 0) mixer.SetFloat(_channelName, Mathf.Log10(volume / 10) * 20);
            else MuteAudioSource(_channelName, true);
        }
    }
    public void PlayOneShotSFX(AudioClip _clip)
    {
        sfxSource.PlayOneShot(_clip);
    }
    public void PlayClipAtPointSFX(AudioClip _clip, Transform _position)
    {
        AudioSource.PlayClipAtPoint(_clip, transform.position);
    }
    public void PlayMusic(AudioClip _clip)
    {
        musicSource.loop = true;
        musicSource.clip = _clip;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }
    public void MuteAudioSource(string _channelName, bool _mute)
    {
        switch (_channelName)
        {
            case "Music":
                musicSource.mute = _mute;
                break;
            case "SFX":
                sfxSource.mute = _mute;
                break;
            case "Master":
                masterSource.mute = _mute;
                masterMute = _mute;
                if (_mute)
                {
                    sfxSource.mute = _mute;
                    musicSource.mute = _mute;
                }
                break;
        }
    }
}
