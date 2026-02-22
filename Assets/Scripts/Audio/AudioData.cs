using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "newAudioData", menuName = "Data/AudioData", order = 0)]

public class AudioData : ScriptableObject
{
    public SoundCustomVolume[] clips;  
    [Range(0.8f, 1.5f)] public float pitchMin = 0.9f, pitchMax = 1.1f;
    public AudioMixerGroup mixerGroup;
    public bool loop = false; 
 

}
[Serializable]
public class SoundCustomVolume
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume=1;
}
