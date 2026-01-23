using Avocado.Interaction.Interactables;
using Avocado.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
   
    public class ShopManager : InteractEventManager
    {
        [SerializeField] string shopID="01";

        [SerializeField] WeaponDataSO[] weaponsToSell;
        [SerializeField] BuffItemDataSO[] buffsToSell;
        List<ScriptableObject> itemsToSell;
        ShopItemHolder[] shopItemHolders;
        

        [SerializeField] GameObject weaponPickupPrefab;
        [SerializeField] Transform weaponPickupSpawnPoint;
        GameSaveCapsule capsule;
        GameManager gameManager;
        public override void Start()
        {
            base.Start();
            itemsToSell = new List<ScriptableObject>();
            foreach (var weapon in weaponsToSell)
            {
                if (!SaveManager.IsWeaponSavedInPlayerPrefs(weapon))
                    itemsToSell.Add(weapon);
            }
            foreach (var buff in buffsToSell)
            {
                itemsToSell.Add(buff);
            }
            gameManager = GameManager.instance;
            shopItemHolders = gameManager.shopHolder.GetComponentsInChildren<ShopItemHolder>();
            capsule= weaponPickupSpawnPoint.root.GetComponentInChildren<GameSaveCapsule>();
        }

        public override void OnInteractEvent()
        {
            base.OnInteractEvent();
            gameManager.shopHolder.SetActive(true);
            gameManager.windowSelector.SetActive(true);
            int shopIndex = 0;
            foreach (var holder in shopItemHolders)
            {
                holder.gameObject.SetActive(true);
            }
            foreach (var item in itemsToSell)
            {
                if (item is WeaponDataSO && !SaveManager.IsWeaponSavedInPlayerPrefs((WeaponDataSO)item))
                {
                    shopItemHolders[shopIndex].SetItemData((WeaponDataSO)item);
                    shopItemHolders[shopIndex].button.onClick.AddListener(() => SpawnWeaponPickup((WeaponDataSO)item));
                    shopIndex++;
                }
                else if (item is BuffItemDataSO && !SaveManager.IsBuffPurchasedInPlayerPrefs(shopID))
                { 
                    shopItemHolders[shopIndex].SetItemData((BuffItemDataSO)item);
                    shopItemHolders[shopIndex].button.onClick.AddListener(() => ApplyBuffEffect((BuffItemDataSO)item));
                    shopIndex++;
                }
                else continue;
            }

            for (int i = 0; i < shopItemHolders.Length; i++)
            {
                if (i>=shopIndex)
                   shopItemHolders[i].gameObject.SetActive(false);
            }
            gameManager.ChangeState(GameManager.GameState.UI);
        }
        public void ApplyBuffEffect(BuffItemDataSO _data)
        {
            _data.ApllyBuff(shopID);
            
            OnInteractEvent();
        }
        public void SpawnWeaponPickup(WeaponDataSO _data)
        {
            if (gameManager.coins < _data.PriceOnShop)
                return;
            gameManager.SubstractCoin(_data.PriceOnShop);
            gameManager.AddWeaponDataToInventory(_data);
            WeaponPickup weapon = Instantiate(weaponPickupPrefab, weaponPickupSpawnPoint.position, Quaternion.identity).GetComponent<WeaponPickup>();
            weapon.GetComponentInChildren<SpriteRenderer>().enabled = false;
            weapon.SetContext(_data);
            capsule.spawnWeaponAnimation = true;
            capsule.OnSpawnWeaponAnimationEnd += () =>
            {
                Debug.Log("Disable sprite");
                weapon.GetComponentInChildren<SpriteRenderer>().enabled = true;
            };
            OnInteractEvent();
        }

        
    }
}
