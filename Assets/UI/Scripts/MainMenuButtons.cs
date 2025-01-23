using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Avocado
{
    public class MainMenuButtons : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] Image buttonImage;
        [SerializeField] Sprite[] buttonSprites;

        private void Awake()
        {
            buttonImage = GetComponent<Image>();
        }
        private void Start()
        {
            buttonImage.sprite = buttonSprites[0];
        }
     
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (buttonImage.sprite == buttonSprites[1]) return;
            buttonImage.sprite=buttonSprites[1];
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buttonImage.sprite == buttonSprites[0]) return;
            buttonImage.sprite = buttonSprites[0];

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (buttonImage.sprite == buttonSprites[2]) return;
            buttonImage.sprite = buttonSprites[2];
        }
    }
}
