using UnityEngine;
using TMPro;

public class SettingsResolution : MonoBehaviour
{
    TMP_Dropdown dropdown;
    Resolution[] resolutions;
    [SerializeField] AudioClip dropdownClip;

    private void Start()
    {
        resolutions = Screen.resolutions;
        dropdown = GetComponent<TMP_Dropdown>();
        foreach (var res in resolutions)
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
            newOption.text = res.width.ToString() + "X" + res.height.ToString();
            dropdown.options.Add(newOption);
        }
        if (PlayerPrefs.HasKey("Resolution"))
        {
            LoadResolution();
        }
        else
        {
            SetResolution();
        }
    }
    void LoadResolution()
    {
        dropdown.onValueChanged.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(dropdownClip); });
        dropdown.value = PlayerPrefs.GetInt("Resolution");
    }
    public void SetResolution()
    {
        int resolutionIndex = dropdown.value;
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, ConvertToBool(PlayerPrefs.GetInt("Windowed")));
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }
    bool ConvertToBool(int _value)
    {
        return _value == 1 ? true : false;
    }
}
