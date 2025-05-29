using System.Collections.Generic;
using UnityEngine;
using Avocado.Weapons; // Asegúrate que coincida con tu namespace

public class WeaponDatabase : MonoBehaviour
{
    public static WeaponDatabase Instance { get; private set; }

    private Dictionary<string, WeaponDataSO> weaponsByName = new Dictionary<string, WeaponDataSO>();
    [SerializeField] private WeaponDataSO[] weaponAssets;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (weaponAssets == null || weaponAssets.Length == 0)
        {
            weaponAssets = Resources.LoadAll<WeaponDataSO>("");
        }

        foreach (var weapon in weaponAssets)
        {
            if (weapon == null) continue;
            if (!string.IsNullOrEmpty(weapon.Name) && !weaponsByName.ContainsKey(weapon.Name))
            {
                weaponsByName.Add(weapon.Name, weapon);
            }
        }
    }

    public WeaponDataSO GetWeaponData(string name)
    {
        weaponsByName.TryGetValue(name, out var data);
        return data;
    }

    public IEnumerable<WeaponDataSO> GetAllWeapons()
    {
        return weaponAssets;
    }
}