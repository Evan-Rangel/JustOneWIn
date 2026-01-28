using Avocado.Interaction.Interactables;
using Avocado.Weapons;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class InventoryUIController : MonoBehaviour
    {
        public event Action<WeaponDataSO> showWeapon;
        public event Action<WeaponDataSO> selectWeapon;
        [SerializeField] Image uiImage;
        [SerializeField] TMPro.TMP_Text uiName;
        [SerializeField] TMPro.TMP_Text uiDescription;
        int currentIndex;
        [SerializeField] GameObject buttons;
        WeaponDataSO selectedWeaponData;
        [SerializeField] GameObject weaponPickupPrefab;
        [SerializeField] Button selectButton;
        public void SelectButton()
        { 
            selectWeapon?.Invoke(selectedWeaponData);
        }
        private void Update()
        {
            if (selectWeapon == null) return; 
            selectButton.interactable=!GameManager.instance.IsWeaponInGame(selectedWeaponData);
        }
        void SelectWeapon(WeaponDataSO _data)
        {
            WeaponPickup weapon = Instantiate(weaponPickupPrefab, GameManager.instance.currentSpawnPosition.pos.position, Quaternion.identity).GetComponent<WeaponPickup>();
            weapon.weaponIcon.enabled = false;
            weapon.SetContext(_data);
            GameSaveCapsule capsule = GameManager.instance.currentSpawnPosition.pos.root.GetComponentInChildren<GameSaveCapsule>();

            capsule.spawnWeaponAnimation = true;
            capsule.OnSpawnWeaponAnimationEnd += () =>
            {
                weapon.weaponIcon.enabled = true;
            };
        }

        public void OnShowWeapon(WeaponDataSO weaponData)
        {
            uiImage.sprite = weaponData.Icon;
            uiName.text = weaponData.Name;
            uiDescription.text = weaponData.Description;
            selectedWeaponData = weaponData;
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
            selectWeapon += SelectWeapon;
            currentIndex = 0;
            if (GameManager.instance.GetWeaponDataAtIndex(currentIndex))
            {
                showWeapon?.Invoke(GameManager.instance.GetWeaponDataAtIndex(currentIndex));
                buttons.SetActive(GameManager.instance.GetTotalWeapons() > 1);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        private void OnDisable()
        {
           // GameManager.instance.ChangeState(GameManager.GameState.Gameplay);

            showWeapon -= OnShowWeapon;
            selectWeapon -= SelectWeapon;

        }

    }
}
