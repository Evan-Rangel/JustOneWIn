using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        skinIdx = (skinIdx >= data.skins.Length) ? 0 : skinIdx;
        charImage.sprite = data.skins[skinIdx];
        skinIdx++;
    }
    public void PrevCharacterSkin()
    {
        skinIdx = (skinIdx < 0) ? data.skins.Length - 1 : skinIdx;
        charImage.sprite = data.skins[skinIdx];
        skinIdx--;
    }
}
