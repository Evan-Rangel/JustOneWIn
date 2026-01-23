using Avocado.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Avocado
{
    public class ShopItemHolder : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TMP_Text txtName;
        [SerializeField] TMP_Text txtDescription;
        [SerializeField] TMP_Text txtPrice;
        [field: SerializeField] public Button button { get; private set; }


        public void SetItemData(WeaponDataSO weaponData)
        { 
            icon.sprite= weaponData.Icon;
            txtName.SetText(weaponData.Name);
            txtDescription.SetText(weaponData.Description);
            txtPrice.SetText(weaponData.PriceOnShop.ToString());
            button.onClick.RemoveAllListeners();
        }
        public void SetItemData(BuffItemDataSO buffData)
        { 
            icon.sprite = buffData.buffIcon;
            txtName.SetText(buffData.buffName);
            txtDescription.SetText(buffData.buffDescription);
            txtPrice.SetText(buffData.buffPrice.ToString());
            button.onClick.RemoveAllListeners();
          
        }



    }
}
