using Avocado.Weapons;
using UnityEngine;

namespace Avocado
{
    public class SaveManager : MonoBehaviour
    {
        bool dashUnlocked, grabUnlocked, grappleUnlocked;
        WeaponDataSO[] weaponsList;
        int firstWeapon, secondWeapon;
        int staminaLevel, healthLevel;
        int xPos, yPos, zPos;
        int coins;


        public void SavePlayerPrefs()
        { 
            PlayerPrefs.SetInt("DashUnlocked", dashUnlocked ? 1 : 0);
            PlayerPrefs.SetInt("GrabUnlocked", grabUnlocked ? 1 : 0);
            PlayerPrefs.SetInt("GrappleUnlocked", grappleUnlocked ? 1 : 0);
            PlayerPrefs.SetInt("FirstWeapon", firstWeapon);
            PlayerPrefs.SetInt("SecondWeapon", secondWeapon);
            PlayerPrefs.SetInt("StaminaLevel", staminaLevel);
            PlayerPrefs.SetInt("HealthLevel", healthLevel);
            PlayerPrefs.SetInt("XPos", xPos);
            PlayerPrefs.SetInt("YPos", yPos);
            PlayerPrefs.SetInt("ZPos", zPos);
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.Save();
        }
        public void LoadPlayerPrefs()
        {
            if (PlayerPrefs.HasKey("DashUnlocked"))
                dashUnlocked = PlayerPrefs.GetInt("DashUnlocked") == 1;
            if (PlayerPrefs.HasKey("GrabUnlocked"))
                grabUnlocked = PlayerPrefs.GetInt("GrabUnlocked") == 1;
            if (PlayerPrefs.HasKey("GrappleUnlocked"))
                grappleUnlocked = PlayerPrefs.GetInt("GrappleUnlocked") == 1;
            if (PlayerPrefs.HasKey("FirstWeapon"))
                firstWeapon = PlayerPrefs.GetInt("FirstWeapon");
            if (PlayerPrefs.HasKey("SecondWeapon"))
                secondWeapon = PlayerPrefs.GetInt("SecondWeapon");
            if (PlayerPrefs.HasKey("StaminaLevel"))
                staminaLevel = PlayerPrefs.GetInt("StaminaLevel");
            if (PlayerPrefs.HasKey("HealthLevel"))
                healthLevel = PlayerPrefs.GetInt("HealthLevel");
            if (PlayerPrefs.HasKey("XPos"))
                xPos = PlayerPrefs.GetInt("XPos");
            if (PlayerPrefs.HasKey("YPos"))
                yPos = PlayerPrefs.GetInt("YPos");
            if (PlayerPrefs.HasKey("ZPos"))
                zPos = PlayerPrefs.GetInt("ZPos");
            if (PlayerPrefs.HasKey("Coins"))
                coins = PlayerPrefs.GetInt("Coins");
        }


    }
}
