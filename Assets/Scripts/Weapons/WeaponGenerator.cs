using System;
using System.Collections.Generic;
using System.Linq;
using Avocado.CoreSystem;
using Avocado.Weapons.Components;
using UnityEngine;
using Mirror;
/*---------------------------------------------------------------------------------------------
El script WeaponGenerator es el responsable de generar dinámicamente un arma en un GameObject 
según los datos de un WeaponDataSO (un ScriptableObject que define el comportamiento del arma). 
Este sistema permite:
-Agregar solo los componentes necesarios para cada arma (gracias a una lista de dependencias en WeaponDataSO).
-Eliminar los componentes que ya no son necesarios, evitando residuos entre armas previas.
-Configurar automáticamente el AnimatorController del arma para reflejar su animación específica.
-Sincronizar con un sistema de inventario, respondiendo a los cambios cuando el jugador cambia de arma.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    public class WeaponGenerator : NetworkBehaviour
    {
        // Evento que se dispara cuando se comienza a generar un arma
        public event Action OnWeaponGenerating;

        // Referencias al arma y al input de combate (por ejemplo: botón de ataque 1 o 2)
        [SerializeField] private Weapon weapon;
        [SerializeField] private CombatInputs combatInput;

        // Lista de componentes ya presentes en el GameObject del arma
        private List<WeaponComponent> componentAlreadyOnWeapon = new List<WeaponComponent>();

        // Lista de componentes que se agregaron al generar esta arma
        private List<WeaponComponent> componentsAddedToWeapon = new List<WeaponComponent>();

        // Tipos de componentes que esta arma necesita según su definición (ScriptableObject)
        private List<Type> componentDependencies = new List<Type>();

        // Referencia al Animator del arma
        private Animator anim;

        // Referencia al inventario de armas del jugador
        private WeaponInventory weaponInventory;
        
        public List<RuntimeAnimatorController> animatorControllers;


        public void GenerateWeapon(WeaponDataSO data)
        {
            OnWeaponGenerating?.Invoke();

            weapon.SetData(data);

            if (data is null)
            {
                weapon.SetCanEnterAttack(false); // Si no hay datos, el arma no puede atacar
                return;
            }

            // Limpiar listas para evitar residuos de configuraciones anteriores
            componentAlreadyOnWeapon.Clear();
            componentsAddedToWeapon.Clear();
            componentDependencies.Clear();

            // Obtener los componentes ya existentes
            componentAlreadyOnWeapon = GetComponents<WeaponComponent>().ToList();

            // Obtener las dependencias necesarias desde el ScriptableObject
            componentDependencies = data.GetAllDependencies();

            // Asegurarse de que todos los componentes necesarios estén presentes
            foreach (var dependency in componentDependencies)
            {
                // Evitar agregar dos veces el mismo tipo
                if (componentsAddedToWeapon.FirstOrDefault(component => component.GetType() == dependency))
                    continue;

                // Buscar si ya está presente el componente
                var weaponComponent =
                    componentAlreadyOnWeapon.FirstOrDefault(component => component.GetType() == dependency);

                // Si no está, lo agregamos
                if (weaponComponent == null)
                {
                    weaponComponent = gameObject.AddComponent(dependency) as WeaponComponent;
                }

                // Inicializamos el componente
                weaponComponent.Init();

                // Lo añadimos a la lista de componentes activos
                componentsAddedToWeapon.Add(weaponComponent);
            }

            // Eliminar cualquier componente que no sea necesario para esta arma
            var componentsToRemove = componentAlreadyOnWeapon.Except(componentsAddedToWeapon);
            foreach (var weaponComponent in componentsToRemove)
            {
                Destroy(weaponComponent);
            }

            // Asignar el AnimatorController correcto para esta arma
            int animatorIndex = animatorControllers.IndexOf(data.AnimatorController);
            CmdChangeWeaponAnimator(animatorIndex);
            //anim.runtimeAnimatorController = data.AnimatorController;

            // Ahora el arma puede atacar
            weapon.SetCanEnterAttack(true);
        }
        [Command]
        public void CmdChangeWeaponAnimator(int controllerIdx)
        {
            RcpChangeWeaponAnimator(controllerIdx);
        }
        [ClientRpc]
        public void RcpChangeWeaponAnimator(int controllerIdx)
        {
            anim.runtimeAnimatorController = animatorControllers[controllerIdx];
        }

        // Manejador del evento que indica que los datos del arma han cambiado.
        private void HandleWeaponDataChanged(int inputIndex, WeaponDataSO data)
        {
            // Solo responder si el índice corresponde al input de combate de este generador
            if (inputIndex != (int)combatInput)
                return;

            GenerateWeapon(data); // Genera el arma con los nuevos datos
        }

        private void Start()
        {
            // Obtener el sistema de inventario desde el núcleo
            weaponInventory = weapon.Core.GetCoreComponent<WeaponInventory>();

            // Suscribirse al evento de cambio de arma
            weaponInventory.OnWeaponDataChanged += HandleWeaponDataChanged;

            // Obtener el Animator del hijo
            anim = GetComponentInChildren<Animator>();

            // Generar el arma actual si ya existe una asignada en el inventario
            if (weaponInventory.TryGetWeapon((int)combatInput, out var data))
            {
                GenerateWeapon(data);
            }
        }

        private void OnDestroy()
        {
            // Evitar memory leaks: desuscribirse del evento al destruirse
            weaponInventory.OnWeaponDataChanged -= HandleWeaponDataChanged;
        }
    }
}
