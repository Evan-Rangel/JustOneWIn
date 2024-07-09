using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource masterSource;

    public static AudioManager instance;
    bool masterMute;
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
    public void PlaySFX(AudioClip _clip)
    {
        sfxSource.PlayOneShot(_clip);
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
                break;
        }
    }
}
