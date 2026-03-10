using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.GlobalIllumination;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] GameObject sfxSourcePrefab;
    [SerializeField] AudioSource masterSource;

    [SerializeField] AudioMixer mixer;
    public static AudioManager instance;
    Queue<AudioSource> pool = new Queue<AudioSource>();
    Dictionary<string, AudioData> sounds = new Dictionary<string, AudioData>();

    public bool masterMute { get; private set; } = false;

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
        LoadSounds();
        InitPool();
    }

    

    void InitPool()
    {
        for (int i = 0; i < 20; i++)
        {
            pool.Enqueue(CreateSource());
        }
    }
    void LoadSounds()
    {
        AudioData[] allAudios = Resources.LoadAll<AudioData>("");
        foreach (var item in allAudios)
        {
            sounds[item.name] = item;
        }
    }
    public void PlaySFXSound(string soundName, Transform position=null)
    {
        if (!sounds.TryGetValue(soundName, out AudioData sound))
            return ;
        AudioSource source = pool.Count > 0 ? pool.Dequeue() : CreateSource();
        source.gameObject.SetActive(true);
        SoundCustomVolume clipToPlay = sound.clips[Random.Range(0, sound.clips.Length)];
        source.clip = clipToPlay.clip;
        source.volume = clipToPlay.volume;
        source.pitch = Random.Range(sound.pitchMin, sound.pitchMax);
        source.outputAudioMixerGroup = sound.mixerGroup;
        source.loop = sound.loop;
        source.Play();
        if (!sound.loop)
            StartCoroutine(StopSfxSound(clipToPlay.clip.length / source.pitch, source));
        if (position == null)
        {
            source.transform.parent = transform;
            source.transform.position = transform.position;
            source.spatialBlend = 0;
            return ;
        }
        source.spatialBlend = 1;
        source.transform.position = position.position;
        source.transform.parent = position;
    }
   
    public void StopSfxSoundLooping(AudioSource source)
    {
        source.transform.parent = transform;
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
    }
    IEnumerator StopSfxSound(float time, AudioSource source)
    { 
        yield return Helpers.GetWait(time);
        pool.Enqueue(source);
        source.transform.parent = transform;
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
    }
    AudioSource CreateSource()
    {
        GameObject sfxSourceObject = Instantiate(sfxSourcePrefab, transform);
        AudioSource src = sfxSourceObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.maxDistance = 20;
        src.rolloffMode = AudioRolloffMode.Linear;
        sfxSourceObject.SetActive(false);
        return src;
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
