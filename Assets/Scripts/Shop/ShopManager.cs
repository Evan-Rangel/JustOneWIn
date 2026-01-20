using Avocado.Weapons;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    [Serializable]
    public class ItemShop
    {
        public WeaponDataSO weaponData;
        public int price;
    }
    public class ShopManager : InteractEventManager
    {
        [SerializeField] ItemShop[] weapons;
        [SerializeField] GameObject weaponPickup;
        [SerializeField] Button button;
        GameManager gameManager;
        public override void Start()
        {
            base.Start();
            gameManager = GameManager.instance;
        }

        public override void OnInteractEvent()
        {
            base.OnInteractEvent();
            gameManager.shopHolder.SetActive(true);
            gameManager.ChangeState(GameManager.GameState.UI);
        }


        public void SpawnWeaponPickup(Vector3 position)
        {
            Instantiate(weaponPickup, position, Quaternion.identity);
        }
    }
}
