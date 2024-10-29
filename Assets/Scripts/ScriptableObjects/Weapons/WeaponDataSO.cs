using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avocado.Weapons.Components;
using UnityEngine;

namespace Avocado.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Basic Weapon Data", order = 0)]
    public class WeaponDataSO : ScriptableObject
    {
        [field: SerializeField] public RuntimeAnimatorController AnimatorController { get; private set; }

        #region Integers
        [field: SerializeField] public int NumberOfAttacks { get; private set; }
        #endregion

        #region Lists
        [field: SerializeReference] public List<ComponentData> ComponentData { get; private set; }
        #endregion

        #region Functions
        public T GetData<T>()
        {
            return ComponentData.OfType<T>().FirstOrDefault();
        }

        public List<Type> GetAllDependencies()
        {
            return ComponentData.Select(component => component.ComponentDependency).ToList();
        }

        public void AddData(ComponentData data)
        {
            if (ComponentData.FirstOrDefault(t => t.GetType() == data.GetType()) != null)
            {
                return;
            }

            ComponentData.Add(data);
        }
        /*
        [ContextMenu("Add Sprite Data")]
        private void AddSpriteData() => ComponentData.Add(new WeaponSpriteData());

        [ContextMenu("Add Movement Data")]
        private void AddMovementData() => ComponentData.Add(new MovementData());
        */
        #endregion
    }
}
