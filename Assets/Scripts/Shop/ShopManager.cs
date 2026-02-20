using Avocado.Interaction.Interactables;
using Avocado.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class ShopManager : InteractEventManager
    {
        [field: SerializeField] public TpEntity shopID {get; private set;} 

        [SerializeField] WeaponDataSO[] weaponsToSell;
        [SerializeField] BuffItemDataSO[] buffsToSell;
        List<ScriptableObject> itemsToSell;
        ShopItemHolder[] shopItemHolders;
       // [SerializeField] Transform spawnPosition;

        [SerializeField] GameObject weaponPickupPrefab;
        GameManager gameManager;
        public override void Start()
        {
            base.Start();
            itemsToSell = new List<ScriptableObject>();
            foreach (var weapon in weaponsToSell)
            {
                if (!SaveManager.IsWeaponSaved(weapon))
                    itemsToSell.Add(weapon);
            }
            foreach (var buff in buffsToSell)
            {
                itemsToSell.Add(buff);
            }
            gameManager = GameManager.instance;
            shopItemHolders = gameManager.shopHolder.GetComponentsInChildren<ShopItemHolder>();
        }

        public override void OnInteractEvent()
        {
            base.OnInteractEvent();
            gameManager.ActiveShop(shopID);
 
            int shopIndex = 0;
            foreach (var holder in shopItemHolders)
            {
                holder.gameObject.SetActive(true);
            }
            foreach (var item in itemsToSell)
            {
                if (item is WeaponDataSO && !SaveManager.IsWeaponSaved((WeaponDataSO)item))
                {
                    shopItemHolders[shopIndex].SetItemData((WeaponDataSO)item);
                    shopItemHolders[shopIndex].button.onClick.AddListener(() => SpawnWeaponPickup((WeaponDataSO)item));
                    shopIndex++;
                }
                else if (item is BuffItemDataSO && !SaveManager.IsBuffPurchased(shopID.zoneName))
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
        }
        public void ApplyBuffEffect(BuffItemDataSO _data)
        {
            _data.ApllyBuff(shopID.zoneName);
            gameManager.OnSelectedButtonDisabled();    
            OnInteractEvent();
        }
        public void SpawnWeaponPickup(WeaponDataSO _data)
        {
            if (!gameManager.SubstractCoin(_data.PriceOnShop))
                return;
            gameManager.AddWeaponDataToInventory(_data);
            gameManager.OnSelectedButtonDisabled();

            WeaponPickup weapon = Instantiate(weaponPickupPrefab, gameManager.currentSpawnPosition.pos.position, Quaternion.identity).GetComponent<WeaponPickup>();
            weapon.weaponIcon.enabled = false;
            weapon.SetContext(_data);
            GameSaveCapsule capsule = gameManager.currentSpawnPosition.pos.root.GetComponentInChildren<GameSaveCapsule>();

            capsule.spawnWeaponAnimation = true;
            capsule.OnSpawnWeaponAnimationEnd += () =>
            {
                weapon.weaponIcon.enabled = true;
            };
            OnInteractEvent();
        }

        
    }
}
