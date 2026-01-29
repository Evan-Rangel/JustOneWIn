using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] Image keyboardKeyOnImage;
        [SerializeField] Image keyboardKeyOffImage;
        [SerializeField] Image iconImage;
        [SerializeField] TMP_Text descriptionText;
        [SerializeField] GameObject descriptionHolder;
        [SerializeField] GameObject tutorialUIHolder;
        bool activeTutorial = false;
        float maxTime=3;
        float timer;
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
        }
        private void Update()
        {
            if (!activeTutorial) return;
            timer += Time.deltaTime;

            if (1%timer != 0)
                return;
            keyboardKeyOffImage.gameObject.SetActive(!keyboardKeyOffImage.gameObject.activeSelf);
            keyboardKeyOnImage.gameObject.SetActive(!keyboardKeyOnImage.gameObject.activeSelf);
            
            
            
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
            keyboardKeyOnImage.sprite = tutorialItem.keyboardKey.OnKeySprite;
            keyboardKeyOffImage.sprite = tutorialItem.keyboardKey.OffKeySprite;
            keyboardKeyOffImage.gameObject.SetActive(false);
            keyboardKeyOnImage.gameObject.SetActive(true);
            iconImage.sprite = tutorialItem.iconSprite;
            descriptionText.text = tutorialItem.description;
            descriptionHolder.SetActive(true);
            GameManager.instance.ChangeState(GameManager.GameState.UI);
            Invoke("PlayerStaticDelay", 0.5f);
        }
        void PlayerStaticDelay()
        { 
            GameManager.instance.ChangeState(GameManager.GameState.Gameplay);
        }
    }
}
