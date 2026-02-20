using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Avocado
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField]GameObject description;
        [SerializeField] GameObject icon;
        [SerializeField] string itemName;
        [SerializeField] int level;
       [SerializeField]GameObject buttonObject;
        private void Awake()
        {
            buttonObject = GetComponentInChildren<Button>().gameObject;
         
        }
      
        private void Update()
        {
            if (!IsUnlocked()) return;
            description.SetActive(EventSystem.current.currentSelectedGameObject == buttonObject);
        }
       
        private void OnEnable()
        {
            icon.SetActive(false);

            if (IsUnlocked())
            {
                icon.SetActive(true);
            }

            description.SetActive(false);
        }
        bool IsUnlocked()
        {
            switch (itemName)
            {
                case "Dash":
                    return SaveManager.GetDashUnlocked();
                case "Grapple":return SaveManager.GetGrappleUnlocked();
                case "Grab": return SaveManager.GetGrabUnlocked();
                case "Stamina": return SaveManager.GetStaminaLevel()>=level;
                case "Health": return SaveManager.GetHealthLevel() >= level;
                default: return false;
            }
        }
    }
}
