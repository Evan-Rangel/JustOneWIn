using Avocado.Weapons.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Avocado
{
    [CreateAssetMenu(fileName ="newItemData", menuName ="Data/Item Data/Basic Item Data", order =0)]
    public class ItemDataSO : ScriptableObject
    {

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
        #endregion
    }
}
