using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterButton : MonoBehaviour
{
    [SerializeField] Image charImage;
    [SerializeField] TMP_Text charName;
    [SerializeField]Button selectButton;
    [SerializeField] AudioClip clickSound;
    CharacterData data;
    int skinIdx=0;
    
    public void SetCharacterData(CharacterData _data) 
    { 
        data = _data; 
        charName.text = data.cName;
        selectButton.onClick.AddListener(delegate { MenuManager.instance.SelectCharacter(_data); });
        selectButton.onClick.AddListener(delegate { AudioManager.instance.PlayOneShotSFX(clickSound); });
        NextCharacterSkin(); 
    }

    public void NextCharacterSkin()
    {
        if (skinIdx >= data.skins.Length) skinIdx = 0;
        charImage.sprite = data.skins[skinIdx];
        skinIdx++;
    }
    public void PrevCharacterSkin()
    {
        if (skinIdx < 0) skinIdx = data.skins.Length-1;
        charImage.sprite = data.skins[skinIdx];
        skinIdx--;
    }
}
