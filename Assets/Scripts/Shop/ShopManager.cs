using Avocado.Weapons;
using Steamworks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    [Serializable]
  
    public class WeaponItemShop 
    {
        public int price;
        public WeaponDataSO weaponData;
    }
   
    public class ShopManager : InteractEventManager
    {
        [SerializeField] WeaponItemShop[] weaponsToSell;
        [SerializeField] BuffItemDataSO[] buffsToSell;

        [SerializeField] ShopItemHolder[] shopItemHolders;
        

        [SerializeField] GameObject weaponPickup;
        [SerializeField] Button button;
        GameManager gameManager;
        public override void Start()
        {
            base.Start();
            gameManager = GameManager.instance;
            shopItemHolders = gameManager.shopHolder.GetComponentsInChildren<ShopItemHolder>();
        }

        public override void OnInteractEvent()
        {
            base.OnInteractEvent();
            gameManager.shopHolder.SetActive(true);
            int shopIndex = 0;
            for (int i = 0;i < weaponsToSell.Length; i++)
            {
                shopItemHolders[shopIndex].SetItemData(weaponsToSell[i].weaponData, weaponsToSell[i].price);
                shopIndex++;
            }
            for (int i = 0; i < buffsToSell.Length; i++)
            {
                shopItemHolders[shopIndex].SetItemData(buffsToSell[i]);
                shopIndex++;
            }
            gameManager.ChangeState(GameManager.GameState.UI);
        }


        public void SpawnWeaponPickup(Vector3 position)
        {
            Instantiate(weaponPickup, position, Quaternion.identity);
        }
    }
}
