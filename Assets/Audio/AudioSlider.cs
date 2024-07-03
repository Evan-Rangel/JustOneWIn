using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioSlider : MonoBehaviour
{
    [SerializeField] string channelName;
    [SerializeField] AudioMixer mainAudio;
    Slider slider;
    float volume = 0;
    private void Start()
    {
        slider = GetComponent<Slider>();
        if (PlayerPrefs.HasKey(channelName + "Volume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
        }
    }
    public string GetChannelName()
    {
        return channelName;
    }
    public void SetVolume()
    {
        volume = slider.value;
        mainAudio.SetFloat(channelName, Mathf.Log10(volume / 100) * 20);
        PlayerPrefs.SetFloat(channelName + "Volume", volume);
    }
    private void LoadVolume()
    {
        slider.value = PlayerPrefs.GetFloat(channelName + "Volume");
    }
}
