using Avocado.Weapons;
using UnityEngine;

namespace Avocado
{
    public static class SaveManager 
    {
        /* static bool dashUnlocked, grabUnlocked, grappleUnlocked;
         //WeaponDataSO[] weaponsList;
         static int firstWeapon, secondWeapon;
         static int staminaLevel, healthLevel;
         static int xPos, yPos, zPos;
         static int coins;


         */

        #region weaponPrefs
        public static void SaveWeaponInPlayerPrefs(WeaponDataSO weaponData)
        {
            PlayerPrefs.SetInt(weaponData.Name, 1);
            PlayerPrefs.Save();
        }public static bool IsWeaponSavedInPlayerPrefs(WeaponDataSO weaponData)
        {
            return PlayerPrefs.HasKey(weaponData.Name) && PlayerPrefs.GetInt(weaponData.Name) == 1;
        }
        #endregion

        #region PlayerPositionPrefs
        public static void SavePlayerPositionInPlayerPrefs(Vector3 _position)
        {
            PlayerPrefs.SetFloat("XPos", _position.x);
            PlayerPrefs.SetFloat("YPos", _position.y);
            PlayerPrefs.SetFloat("ZPos", _position.z);
            PlayerPrefs.Save();
        }
        public static Vector3 GetPlayerPosition()
        {
            if (PlayerPrefs.HasKey("XPos")&& PlayerPrefs.HasKey("YPos")&& PlayerPrefs.HasKey("ZPos"))
            {
                Vector3 pos = new Vector3(PlayerPrefs.GetFloat("XPos"), PlayerPrefs.GetFloat("YPos"), PlayerPrefs.GetFloat("ZPos"));
                return pos;
            }
            return GameManager.instance.newGameStartPosition.position;
        }
        #endregion

        #region AbilitiesPrefs
        public static void SaveDashUnlocked(bool _unlocked)
        {
            PlayerPrefs.SetInt("DashUnlocked", _unlocked ? 1 : 0);
            PlayerPrefs.Save();

        }
        public static bool GetDashUnlocked()
        {
            return PlayerPrefs.HasKey("DashUnlocked")&& PlayerPrefs.GetInt("DashUnlocked", 0) == 1;
        }

        public static void SaveGrabUnlocked(bool _unlocked)
        {
            PlayerPrefs.SetInt("GrabUnlocked", _unlocked ? 1 : 0);
            PlayerPrefs.Save();
        }
        public static bool GetGrabUnlocked()
        {
            return PlayerPrefs.HasKey("GrabUnlocked") && PlayerPrefs.GetInt("GrabUnlocked", 0) == 1;
        }

        public static void SaveGrappleUnlocked(bool _unlocked)
        {
            PlayerPrefs.SetInt("GrappleUnlocked", _unlocked ? 1 : 0);
            PlayerPrefs.Save();
        }
        public static bool GetGrappleUnlocked()
        {
            return PlayerPrefs.HasKey("GrappleUnlocked") && PlayerPrefs.GetInt("GrappleUnlocked", 0) == 1;
        }
        #endregion

        #region StatsPrefs
        public static void SaveCoins(int _coins)
        {
            PlayerPrefs.SetInt("Coins", _coins);
            PlayerPrefs.Save();
        }
        public static int GetCoins()
        {
            return PlayerPrefs.HasKey("Coins") ? PlayerPrefs.GetInt("Coins", 0) : 0;
        }
        public static void SaveStaminaLevel(int _level)
        {
            PlayerPrefs.SetInt("StaminaLevel", _level);
            PlayerPrefs.Save();
        }
        public static int GetStaminaLevel()
        {
            return PlayerPrefs.HasKey("StaminaLevel") ? PlayerPrefs.GetInt("StaminaLevel", 1) : 1;
        }
        public static void SaveHealthLevel(int _level)
        {
            PlayerPrefs.SetInt("HealthLevel", _level);
            PlayerPrefs.Save();
        }
        public static int GetHealthLevel()
        {
            return PlayerPrefs.HasKey("HealthLevel") ? PlayerPrefs.GetInt("HealthLevel", 1) : 1;
        }
        #endregion
        #region Shop Buffs
        public static void SaveBuffPurchaseInPlayerPrefs(string _shopID, int value)
        {
            PlayerPrefs.SetInt("DamageBuffLevel", value);
            PlayerPrefs.Save();
        }
        public static bool IsBuffPurchasedInPlayerPrefs(string _shopID)
        {
            return PlayerPrefs.HasKey("DamageBuffLevel") && PlayerPrefs.GetInt("DamageBuffLevel")==1;
        }
        #endregion
        public static void DeleteSaved()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
