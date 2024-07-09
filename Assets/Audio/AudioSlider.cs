using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] string channelName;
    [SerializeField] AudioMixer mainAudio;
    [SerializeField] TMP_Text value; 
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

    public void SetVolume()
    {
        volume = slider.value;
        value.text = slider.value.ToString();
        PlayerPrefs.SetFloat(channelName + "Volume", volume);
        mainAudio.SetFloat(channelName, Mathf.Log10(volume / 10) * 20);
        if (slider.value==0)
        {
            AudioManager.instance.MuteAudioSource(channelName, true);
            return;
        }
        AudioManager.instance.MuteAudioSource(channelName, false);
    }
    private void LoadVolume()
    {
        slider.value = PlayerPrefs.GetFloat(channelName + "Volume");
        value.text = slider.value.ToString();
    }
}
