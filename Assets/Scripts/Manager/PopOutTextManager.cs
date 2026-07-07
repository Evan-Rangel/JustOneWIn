using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{

    public class PopOutTextManager : MonoBehaviour
    {

        [SerializeField] GameObject dialogueHolder, iconHolder;
        [SerializeField] Image inputOnImage;
        [SerializeField] Image inputOffImage;
        [SerializeField] TMP_Text dialogueText;
        bool activeTutorial = false;
        float maxTime = 3;
        public static PopOutTextManager instance;

     
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this; 
            }
            dialogueHolder.SetActive(false);
            iconHolder.SetActive(false);
        }
        public void ShowText(string _text, Vector2 _position)
        { 
            dialogueText.text = "";
            transform.position = _position;
            StopAllCoroutines();
            gameObject.SetActive(true);
            iconHolder.SetActive(false);
            dialogueHolder.SetActive(true);
            StartCoroutine(TextAnimation(_text));
        }
        IEnumerator TextAnimation(string _text)
        {
            while (dialogueText.text!=_text)
            {
                yield return Helpers.GetWait(0.05f);
                dialogueText.text = _text.Substring(0, Mathf.Min(dialogueText.text.Length + 1, _text.Length));
            }
            StartCoroutine(TimerText());
        }
        IEnumerator TimerText()
        {
            yield return Helpers.GetWait(maxTime);
            dialogueHolder.SetActive(false);
            activeTutorial = false;
        }

        public void ShowIcon(KeyboardKeySO _keyToShow, Vector2 _position)
        {
            transform.position = _position;
            StopAllCoroutines();
            dialogueHolder.SetActive(false);
            gameObject.SetActive(true);
            iconHolder.SetActive(true);
            inputOffImage.sprite = _keyToShow.OffKeySprite;
            inputOnImage.sprite = _keyToShow.OnKeySprite;
            activeTutorial = true;
            StartCoroutine(IconAnimation());
            StartCoroutine(TimerIcon());
        }
        IEnumerator TimerIcon()
        {
            yield return Helpers.GetWait(maxTime);

            iconHolder.SetActive(false);
            activeTutorial = false;
        }
        IEnumerator IconAnimation()
        {
            while (activeTutorial)
            {
                yield return Helpers.GetWait(.3f);
                inputOffImage.gameObject.SetActive(!inputOffImage.gameObject.activeSelf);
                inputOnImage.gameObject.SetActive(!inputOnImage.gameObject.activeSelf);
            }
        }

    }
}
