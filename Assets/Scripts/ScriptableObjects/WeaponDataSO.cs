using System;
using System.Collections.Generic;
using System.Linq; // Para usar LINQ como FirstOrDefault o OfType
using Avocado.Weapons.Components;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
WeaponDataSO es un ScriptableObject que almacena información de un arma: su nombre, ícono, 
descripción, animaciones, número de ataques y una lista de componentes personalizados 
(ComponentData) que definen comportamientos únicos como daño, efectos, retroceso, etc.

Además, provee utilidades para recuperar un componente específico (GetData<T>()), 
consultar dependencias (GetAllDependencies()), y agregar componentes sin duplicados (AddData()).
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Basic Weapon Data", order = 0)]
    public class WeaponDataSO : ScriptableObject
    {
        // Sprite del ícono del arma
        [field: SerializeField] public Sprite Icon { get; set; }

        // Nombre del arma
        [field: SerializeField] public string Name { get; private set; }

        // Descripción del arma
        [field: SerializeField] public string Description { get; private set; }

        // Controlador de animaciones que usará esta arma
        [field: SerializeField] public RuntimeAnimatorController AnimatorController { get; private set; }

        // Número de ataques que tiene esta arma (combos)
        [field: SerializeField] public int NumberOfAttacks { get; private set; }

        // Lista de datos de componentes asociados al arma (daño, knockback, etc.)
        [field: SerializeReference] public List<ComponentData> ComponentData { get; private set; }

        // Devuelve el primer componente del tipo solicitado
        public T GetData<T>()
        {
            return ComponentData.OfType<T>().FirstOrDefault();
        }

        // Devuelve una lista de todos los tipos de dependencias de los componentes del arma
        public List<Type> GetAllDependencies()
        {
            return ComponentData.Select(component => component.ComponentDependency).ToList();
        }

        // Agrega un nuevo dato de componente si no existe uno del mismo tipo
        public void AddData(ComponentData data)
        {
            if (ComponentData.FirstOrDefault(t => t.GetType() == data.GetType()) != null)
                return;

            ComponentData.Add(data);
        }
    }
}
