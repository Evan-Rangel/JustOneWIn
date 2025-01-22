using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] string channelName;
    [SerializeField] AudioMixer mainAudio;
    [SerializeField] TMP_Text value;
    [SerializeField] AudioClip sliderClip;
    Slider slider;
    float volume = 0;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        if (PlayerPrefs.HasKey(channelName + "Volume"))
        {
            LoadVolume();
        }
    }
    public void SetVolume(Slider _masterSlider)
    {
        volume = slider.value;
        value.text = volume.ToString();
        PlayerPrefs.SetFloat(channelName + "Volume", volume);
        mainAudio.SetFloat(channelName, Mathf.Log10(volume / 10) * 20);
        if (slider.value==0 || _masterSlider.value==0)
        {
            AudioManager.instance.MuteAudioSource(channelName, true);
            return;
        }
        AudioManager.instance.MuteAudioSource(channelName, false);
    }
    public void ChekMasterMute(Slider _masterSlider)
    {
        if (volume > 0 && _masterSlider.value>0)
        {
            AudioManager.instance.MuteAudioSource(channelName, false);
        }
    }
    private void LoadVolume()
    {
        volume = PlayerPrefs.GetFloat(channelName + "Volume");
        slider.value = volume;
        slider.onValueChanged.AddListener(delegate {AudioManager.instance.PlayOneShotSFX(sliderClip); });
        value.text = volume.ToString();
    }
}
