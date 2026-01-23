using Avocado.Weapons;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class InventoryUIController : MonoBehaviour
    {
        public event Action<WeaponDataSO> showWeapon;
        [SerializeField] Image uiImage;
        [SerializeField] TMPro.TMP_Text uiName;
        [SerializeField] TMPro.TMP_Text uiDescription;
        int currentIndex;
        [SerializeField] GameObject buttons;
        private void Awake()
        {
            //gameObject.SetActive(false);
        }

        public void OnShowWeapon(WeaponDataSO weaponData)
        {
            uiImage.sprite = weaponData.Icon;
            uiName.text = weaponData.Name;
            uiDescription.text = weaponData.Description;
        }
        public void NextWeapon()
        { 
            currentIndex++;
            if (!GameManager.instance.GetWeaponDataAtIndex(currentIndex))
            {
                currentIndex = 0;
            }
            showWeapon?.Invoke(GameManager.instance.GetWeaponDataAtIndex(currentIndex));
        }
        public void PreviousWeapon()
        {
            currentIndex--;
            if (!GameManager.instance.GetWeaponDataAtIndex(currentIndex))
            {
                currentIndex = GameManager.instance.GetTotalWeapons() - 1;
            }
            showWeapon?.Invoke(GameManager.instance.GetWeaponDataAtIndex(currentIndex));
        }
        private void OnEnable()
        {
            showWeapon += OnShowWeapon;
            currentIndex = 0;
            if (GameManager.instance.GetWeaponDataAtIndex(currentIndex))
            {
                showWeapon?.Invoke(GameManager.instance.GetWeaponDataAtIndex(currentIndex));
                buttons.SetActive(GameManager.instance.GetTotalWeapons() > 1);
                GameManager.instance.ChangeState(GameManager.GameState.UI);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        private void OnDisable()
        {
            GameManager.instance.ChangeState(GameManager.GameState.Gameplay);

            showWeapon -= OnShowWeapon;
        }

    }
}
