/*using Avocado.Weapons;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace Avocado
{

    public static class SaveManager 
    {
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
        public static void SaveZoneSpawnName(string _zoneName)
        { 
            PlayerPrefs.SetString("ZoneSpawn", _zoneName);
            PlayerPrefs.Save();
        }
        public static string GetZoneSpawnName()
        {
            if (PlayerPrefs.HasKey("ZoneSpawn"))
            {
                return PlayerPrefs.GetString("ZoneSpawn");
            }
            return "";
        }
      
        #endregion

        #region ZonesPrefs
        public static void SaveZoneUnlocked(string zoneName)
        {
            PlayerPrefs.SetInt(zoneName, 1);
            PlayerPrefs.Save();
        }
        public static bool IsZoneUnlocked(string zoneName)
        {
            return PlayerPrefs.HasKey(zoneName) && PlayerPrefs.GetInt(zoneName, 0) == 1;
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
            PlayerPrefs.SetInt(_shopID+"Buff", value);
            PlayerPrefs.Save();
        }
        public static bool IsBuffPurchasedInPlayerPrefs(string _shopID)
        {
            return PlayerPrefs.HasKey(_shopID + "Buff") && PlayerPrefs.GetInt(_shopID + "Buff") ==2;
        }
        #endregion
        public static void DeleteSaved()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
*/
using Avocado.Weapons;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections.Generic;

namespace Avocado
{
    [Serializable]
    public class SaveData
    {
        public List<string> eventsUnlocked = new List<string>();
        public List<string> unlockedTutorials = new List<string>();
        public List<string> dialoguesKey = new List<string>();
        public string firstWeaponName="";    
        public string secondWeaponName="";    
        public List<string> unlockedWeapons = new List<string>();
        public string sceneSpawn = "";
        public List<string> unlockedZones = new List<string>();
        public bool dashUnlocked = false;
        public bool grabUnlocked = false;
        public bool grappleUnlocked = false;
        public int coins = 0;
        public int staminaLevel = 1;
        public int healthLevel = 1;
        public Dictionary<string, int> shopBuffs = new Dictionary<string, int>();
    }

    public static class SaveManager
    {
        private static SaveData data;
        private static string savePath = Application.persistentDataPath + "/save.dat";

        private static void Load()
        {
            if (File.Exists(savePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(savePath, FileMode.Open);
                data = (SaveData)bf.Deserialize(file);
                file.Close();
            }
            else
            {
                data = new SaveData();
            }
        }

        private static void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(savePath);
            bf.Serialize(file, data);
            file.Close();
        }

        #region Events
        public static void SaveEventKey(string eventKey)
        {
            if (data == null) Load();
            if (!data.eventsUnlocked.Contains(eventKey))
            {
                data.eventsUnlocked.Add(eventKey);
                Save();
            }
        }
        public static bool IsEventKeySaved(string eventKey)
        {
            if (data == null) Load();
            return data.eventsUnlocked.Contains(eventKey);
        }


        #endregion

        #region Tutorials

        public static void SaveTutorialKey(string tutorialKey)
        {
            if (data == null) Load();
            if (!data.unlockedTutorials.Contains(tutorialKey))
            {
                data.unlockedTutorials.Add(tutorialKey);
                Save();
            }
        }
        public static bool IsTutorialKeySaved(string tutorialKey)
        {
            if (data == null) Load();
            return data.unlockedTutorials.Contains(tutorialKey);
        }

        #endregion

        #region Dialogues

        public static void SaveDialogueKey(string dialogueKey)
        {
            if (data == null) Load();
            if (!data.dialoguesKey.Contains(dialogueKey))
            {
                data.dialoguesKey.Add(dialogueKey);
                Save();
            }
        }

        public static bool IsDialogueKeySaved(string dialogueKey)
        {
            if (data == null) Load();
            return data.dialoguesKey.Contains(dialogueKey);
        }

        #endregion


        #region weapon
        public static string GetFirstWeaponName()
        {
            if (data == null) Load();

            return data.firstWeaponName;
        }
        public static void SaveInitWeaponName(WeaponDataSO weaponData, int index)
        {
            if (data == null) Load();

            if (index==0)
                data.firstWeaponName= weaponData.Name;
            if (index==1)
                data.secondWeaponName = weaponData.Name;

            Save();
        }    
        public static string GetSecondWeaponName()
        {
            if (data == null) Load();

            return data.secondWeaponName;
        }
   
        public static void SaveWeapon(WeaponDataSO weaponData)
        {
            if (data == null) Load();
            if (!data.unlockedWeapons.Contains(weaponData.Name))
            {
                data.unlockedWeapons.Add(weaponData.Name);
                Save();
            }
        }
      
        public static bool IsWeaponSaved(WeaponDataSO weaponData)
        {
            if (data == null) Load();
            return data.unlockedWeapons.Contains(weaponData.Name);
        }
        #endregion

        #region PlayerPosition
        public static void SaveZoneSpawnName(string _zoneName)
        {
            if (data == null) Load();
            //data.sceneSpawn = _zoneName;
            Save();
        }
        public static string GetZoneSpawnName()
        {
            if (data == null) Load();
            return data.sceneSpawn;
        }


        public static void SaveSceneSpawnName(string _sceneName)
        {
            if (data == null) Load();
            data.sceneSpawn = _sceneName;
            Save();
        }
        public static string GetSceneSpawnName()
        {
            if (data == null) Load();
            return data.sceneSpawn;
        }
        #endregion

        #region Zones
        public static void SaveZoneUnlocked(string zoneName)
        {
            if (data == null) Load();
            if (!data.unlockedZones.Contains(zoneName))
            {
                data.unlockedZones.Add(zoneName);
                Save();
            }
        }

        public static bool IsZoneUnlocked(string zoneName)
        {
            if (data == null) Load();
            return data.unlockedZones.Contains(zoneName);
        }
        #endregion

        #region Abilities
        public static void SaveDashUnlocked(bool _unlocked)
        {
            if (data == null) Load();
            data.dashUnlocked = _unlocked;
            Save();
        }

        public static bool GetDashUnlocked()
        {
            if (data == null) Load();
            return data.dashUnlocked;
        }

        public static void SaveGrabUnlocked(bool _unlocked)
        {
            if (data == null) Load();
            data.grabUnlocked = _unlocked;
            Save();
        }

        public static bool GetGrabUnlocked()
        {
            if (data == null) Load();
            return data.grabUnlocked;
        }

        public static void SaveGrappleUnlocked(bool _unlocked)
        {
            if (data == null) Load();
            data.grappleUnlocked = _unlocked;
            Save();
        }

        public static bool GetGrappleUnlocked()
        {
            if (data == null) Load();
            return data.grappleUnlocked;
        }
        #endregion

        #region Stats
        public static void SaveCoins(int _coins)
        {
            if (data == null) Load();
            data.coins = _coins;
            Save();
        }

        public static int GetCoins()
        {
            if (data == null) Load();
            return data.coins;
        }

        public static void SaveStaminaLevel(int _level)
        {
            if (data == null) Load();
            data.staminaLevel = _level;
            Save();
        }

        public static int GetStaminaLevel()
        {
            if (data == null) Load();
            return data.staminaLevel;
        }

        public static void SaveHealthLevel(int _level)
        {
            if (data == null) Load();
            data.healthLevel = _level;
            Save();
        }

        public static int GetHealthLevel()
        {
            if (data == null) Load();
            return data.healthLevel;
        }
        #endregion

        #region Shop Buffs
        public static void SaveBuffPurchase(string _shopID, int value)
        {
            if (data == null) Load();
            data.shopBuffs[_shopID] = value;
            Save();
        }

        public static bool IsBuffPurchased(string _shopID)
        {
            if (data == null) Load();
            if (data.shopBuffs.TryGetValue(_shopID, out int value))
            {
                return value == 2;
            }
            return false;
        }
        #endregion

        public static void DeleteSaved()
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            data = null; // Next access will load a new SaveData
        }
    }
}