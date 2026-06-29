using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        [SerializeField] MinimapUIManager minimapManager;

        public void test()
        {
            Debug.Log("test");
        }
        public void SetMinimapTeleports(TeleportManager _teleportManager)
        {
            minimapManager.teleportControllers = _teleportManager.allSavePoints;
        }

        [SerializeField] TMP_Text fpsText;
        public void SetFpsText(string _text)
        {
            fpsText.text = _text;
        }




        [SerializeField] TMP_Text coinsText;
        public void SetCoinsText(string _text)
        {
            coinsText.text = _text;
        }




        [SerializeField] GameObject pauseHolder;
        public void TogglePauseHolder(bool _active)
        {
            pauseHolder.SetActive(_active);
        }

        [SerializeField] GameObject[] holderToHideWithEsc;
        public void HideHolders()
        {
            foreach (GameObject holder in holderToHideWithEsc)
            {
                holder.SetActive(false);
            }
        }



        [Header("Abilities Unlock Holders")]
        [SerializeField] GameObject hookHolder;
        [SerializeField] GameObject dashHolder;
        [SerializeField] GameObject climbHolder;
        public void ActiveAbilityHolder(string _holdername)
        {
            switch (_holdername)
            {
                case "hook":
                    hookHolder.SetActive(true);
                    break;
                case "climb":
                    climbHolder.SetActive(true);
                    break;
                case "dash":
                    dashHolder.SetActive(true);
                    break;
            }
        }


        [SerializeField] Image healthBar, staminaBar;
        [SerializeField] GameObject[] healtBarRecoveryImages;

        public void UpdateHealthBar(float _value)
        {
            healthBar.fillAmount = 1 - _value;
        }
        public void UpdateStaminaBar(float _value)
        {
            staminaBar.fillAmount = 1 - _value;
        }
        public void UpdateHealthRecoveryImages(float _Value)
        {
            for (int i = 0; i < healtBarRecoveryImages.Length; i++)
            {
                if (i < (int)_Value)
                {
                    healtBarRecoveryImages[i].SetActive(true);
                    continue;
                }
                healtBarRecoveryImages[i].SetActive(false);
            }
        }



        [SerializeField] Animator healthAnimator, staminaAnimator, statsBackgroundAnimator;
        public void SetHealthAnimationLevel(int _level)
        {
            healthAnimator.SetInteger("Level", _level);
            if (statsBackgroundAnimator.GetInteger("Level") < _level)
                statsBackgroundAnimator.SetInteger("Level", _level);
        }
        public void SetStaminaAnimationLevel(int _level)
        {
            staminaAnimator.SetInteger("Level", _level);
            if (statsBackgroundAnimator.GetInteger("Level") < _level)
                statsBackgroundAnimator.SetInteger("Level", _level);
        }



        [field: SerializeField] public GameObject shopHolder { get; private set; }
        [SerializeField] GameObject windowSelector;

        public void ActiveShop()
        { 
            windowSelector.SetActive(true);
            shopHolder.SetActive(true);

        }


        [Header("Title Colors")]
        [SerializeField] Image titleBackgroundImage;
        [SerializeField] Image titleBorderImage01, titleBorderImage02;
        [SerializeField] TMPro.TMP_Text titleText;

        [SerializeField] Color shopBackgroundColor, inventoryBackgroundColor, statsBackgroundColor;
        [SerializeField] Color shopBorderColor, inventoryBorderColor, statsBorderColor;
        public void SetInfoMenuComputer(string _menuTitle)
        {
            titleText.SetText(_menuTitle);
            switch (_menuTitle)
            {
                case "Shop":
                    titleBorderImage01.color = shopBorderColor;
                    titleBorderImage02.color = shopBorderColor;
                    titleBackgroundImage.color = shopBackgroundColor;

                    return;
                case "Stats":
                    titleBorderImage01.color = statsBorderColor;
                    titleBorderImage02.color = statsBorderColor;
                    titleBackgroundImage.color = statsBackgroundColor;

                    return;
                case "Inventory":
                    titleBorderImage01.color = inventoryBorderColor;
                    titleBorderImage02.color = inventoryBorderColor;
                    titleBackgroundImage.color = inventoryBackgroundColor;

                    return;
                default:
                    return;
            }
        }



        OnEnableFirstButton[] onEnableFirstButtons;
        public void OnSelectedButtonDisabled()
        {
            onEnableFirstButtons = FindObjectsOfType<OnEnableFirstButton>();
            foreach (OnEnableFirstButton button in onEnableFirstButtons)
            {
                if (button.gameObject.activeSelf)
                    button.SelectButton();
            }
        }


        #region Sounds
        public void PlayButtonSelectSound() => AudioManager.instance.PlaySFXSound("ButtonSelect");
        public void PlayButtonSubmitSound() => AudioManager.instance.PlaySFXSound("ButtonSubmit");
        #endregion




        #region Minimap
        [Header("Minimap")]
        [SerializeField] GameObject minimapHolder;
        public void ToggleMinimap(bool _active)
        {
            GameManager.instance.ChangeState(GameManager.GameState.UI);
            minimapHolder.SetActive(_active);
        }
        #endregion

        #region Dialogue
        [Header("Dialogue UI")]
        [SerializeField] GameObject dialogueHolder;
        public void ActiveDialogueWindow()
        {
            dialogueHolder.SetActive(true);
        }


        #endregion
        #region Boss Health Bar
        [Header("Boss Health Bar")]
        [SerializeField] GameObject bossHealthBarHolder;
        [SerializeField] Animator bossHealthBarAnimator;
        [SerializeField] Image bossHealthBar;
        void DisabelHealthBar()
        {
            gameObject.SetActive(false);
        }

        public void UpdateBossHealthBar(float _value)
        {
            if (!bossHealthBarHolder.activeInHierarchy) bossHealthBarHolder.SetActive(true);
            bossHealthBar.fillAmount = _value;
        }

        public void BossDeath()
        {
            DisabelHealthBar();
            bossHealthBarAnimator.SetTrigger("BossDeath");
        }
        #endregion
    }
}
