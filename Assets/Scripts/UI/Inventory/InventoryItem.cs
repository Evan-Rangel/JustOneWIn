using UnityEngine;
using UnityEngine.EventSystems;

namespace Avocado
{
    public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]GameObject description;
        [SerializeField] GameObject icon;
        [SerializeField] string playerPrefbs;
        [SerializeField] int level;
        private void OnEnable()
        {
            icon.SetActive(false);
            if (PlayerPrefs.HasKey(playerPrefbs)&& PlayerPrefs.GetInt(playerPrefbs)==level)
            {
                icon.SetActive(true);
            }
            description.SetActive(false);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (icon.activeSelf)
                description.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (icon.activeSelf)
                description.SetActive(false);
        }
    }
}
