using Avocado.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class ShopItemHolder : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TMP_Text txtName;
        [SerializeField] TMP_Text txtDescription;
        [SerializeField] TMP_Text txtPrice;

        public void SetItemData(WeaponDataSO weaponData, int _price)
        { 
            icon.sprite= weaponData.Icon;
            txtName.SetText(weaponData.Name);
            txtDescription.SetText(weaponData.Description);
            txtPrice.SetText(_price.ToString());
        }
        public void SetItemData(BuffItemDataSO buffData)
        { 
            
            icon.sprite = buffData.buffIcon;
            txtName.SetText(buffData.buffName);
            txtDescription.SetText(buffData.buffDescription);
            txtPrice.SetText(buffData.buffPrice.ToString());


        }


    }
}
