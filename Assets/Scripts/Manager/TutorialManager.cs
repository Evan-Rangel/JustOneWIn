using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] Image inputOnImage;
        [SerializeField] Image inputOffImage;
        [SerializeField] Image iconImage;
        [SerializeField] TMP_Text descriptionText;
        [SerializeField] GameObject descriptionHolder;
        [SerializeField] GameObject tutorialUIHolder;
        bool activeTutorial = false;
        float maxTime=3;
        float timer;
        int currentIconSprite;
        int maxIconSprite;
        [SerializeField] TutorialSO tutorialItem;
        public static TutorialManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
            tutorialUIHolder.SetActive(false);
            Invoke("TempTest", 2);
        }
        void TempTest()
        {
            ShowTutorialItem(tutorialItem);

        }
        private void Update()
        {
            if (!activeTutorial) return;
            timer += Time.deltaTime;

            
            if (timer < maxTime)
                return;
                
            activeTutorial=false;
            tutorialUIHolder.SetActive(false);

        }
        public void ShowTutorialItem(TutorialSO tutorialItem)
        {
            activeTutorial = true;
            timer = 0;
            tutorialUIHolder.SetActive(true);
            inputOnImage.sprite = tutorialItem.keyboardKey.OnKeySprite;
            inputOffImage.sprite = tutorialItem.keyboardKey.OffKeySprite;
            inputOffImage.gameObject.SetActive(false);
            inputOnImage.gameObject.SetActive(true);
            currentIconSprite = 0;
            iconImage.sprite = tutorialItem.iconSprite[currentIconSprite];
            maxIconSprite = tutorialItem.iconSprite.Length;
            descriptionText.text = tutorialItem.description;
            descriptionHolder.SetActive(true);
            GameManager.instance.ChangeState(GameManager.GameState.UI);
            StartCoroutine(InputAnimation());
            StartCoroutine(ActionAnimation());
            Invoke("PlayerStaticDelay", 0.5f);
        }
        IEnumerator InputAnimation()
        {
            while (activeTutorial)
            {
                
                yield return Helpers.GetWait(.3f);
                inputOffImage.gameObject.SetActive(!inputOffImage.gameObject.activeSelf);
                inputOnImage.gameObject.SetActive(!inputOnImage.gameObject.activeSelf);
            }
        }
        
        IEnumerator ActionAnimation()
        {
            while (activeTutorial)
            {

                yield return Helpers.GetWait(0.1f);
                currentIconSprite++;
                if (currentIconSprite >= maxIconSprite)
                    currentIconSprite = 0;
                iconImage.sprite = tutorialItem.iconSprite[currentIconSprite];

            }

        }
        void PlayerStaticDelay()
        { 
            GameManager.instance.ChangeState(GameManager.GameState.Gameplay);
        }
    }
}
