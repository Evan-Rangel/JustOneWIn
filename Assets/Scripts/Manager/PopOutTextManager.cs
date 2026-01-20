using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class PopOutTextManager : MonoBehaviour
    {
        [SerializeField] Image[] backgrounImage;
        [SerializeField] TMP_Text text;
        public void ChangeText(string newText)
        {
            text.SetText(newText);
        }
        private void Start()
        {
            gameObject.SetActive(false);
        }
        private void Update()
        {
            
            float alphaValue = Mathf.PingPong(Time.time, 1);
            foreach (Image image in backgrounImage)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alphaValue);
            }
            text.color = new Color(text.color.r, text.color.g, text.color.b, alphaValue);
    }
    }
}
