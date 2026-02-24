using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    public void PlaySFXSound(string soundName, Vector3 position=default)
    {
        if (!sounds.TryGetValue(soundName, out AudioData sound))
            return;
        AudioSource source = pool.Count > 0 ? pool.Dequeue() : CreateSource();
        SoundCustomVolume clipToPlay = sound.clips[Random.Range(0, sound.clips.Length)];
        source.clip = clipToPlay.clip;
        source.volume = clipToPlay.volume;
        source.pitch = Random.Range(sound.pitchMin, sound.pitchMax);
        source.outputAudioMixerGroup = sound.mixerGroup;
        source.loop = sound.loop;
        source.Play();
        if (position == Vector3.zero)
        { 
            source.transform.position = transform.position;
            source.spatialBlend = 0;
        }
        else
        { 
            source.spatialBlend = 1;
            source.transform.position = position;
        }
        StartCoroutine(StopSfxSound(clipToPlay.clip.length/ source.pitch, source));
    }
    IEnumerator StopSfxSound(float time, AudioSource source)
    { 
        yield return Helpers.GetWait(time);
        pool.Enqueue(source);
        source.Stop();
        source.clip = null;
    }
    AudioSource CreateSource()
    {
        GameObject sfxSourceObject = Instantiate(sfxSourcePrefab, transform);
        AudioSource src = sfxSourceObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
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
    public void PlayClipAtPointSFX(AudioClip _clip, Vector2 _position)
    {
        AudioSource.PlayClipAtPoint(_clip, _position);
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
