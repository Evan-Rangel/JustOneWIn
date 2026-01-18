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
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] ItemShop[] weapons;
        [SerializeField] GameObject weaponPickup;
        [SerializeField] Button button;
        public void SpawnWeaponPickup(Vector3 position)
        {
            Instantiate(weaponPickup, position, Quaternion.identity);
        }
    }
}
