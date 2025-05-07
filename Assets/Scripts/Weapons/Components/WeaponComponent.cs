using System;
using Avocado.CoreSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este archivo define una jerarquía base para componentes de armas en el sistema:
WeaponComponent (base simple):
-Se encarga de suscribirse a los eventos del arma (OnEnter, OnExit).
-Administra el estado de si el ataque está activo.
-Da acceso a sistemas centrales como Core, el tiempo de inicio de ataque, y el controlador de animaciones.
WeaponComponent<T1, T2> (base genérica):
-Se utiliza cuando el componente necesita datos específicos de ataque (AttackData) y de configuración (ComponentData).
-Al iniciar el ataque, selecciona los datos adecuados según el contador de ataque actual (CurrentAttackCounter).
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        // Referencia al arma que contiene este componente
        protected Weapon weapon;

        // Accesos rápidos a propiedades del arma
        protected AnimationEventHandler AnimationEventHandler => weapon.EventHandler;
        protected Core Core => weapon.Core;
        protected float attackStartTime => weapon.AttackStartTime;

        // Indica si el ataque actual está activo
        protected bool isAttackActive;

        // Inicialización personalizada que pueden sobreescribir las clases hijas.
        public virtual void Init()
        {
        }

        // En Awake se obtiene la referencia al arma asociada.
        protected virtual void Awake()
        {
            weapon = GetComponent<Weapon>();
        }

        // En Start se suscriben los eventos de inicio y fin de ataque.
        protected virtual void Start()
        {
            weapon.OnEnter += HandleEnter;
            weapon.OnExit += HandleExit;
        }

        // Se llama cuando comienza un ataque.
        protected virtual void HandleEnter()
        {
            isAttackActive = true;
        }

        // Se llama cuando termina un ataque.
        protected virtual void HandleExit()
        {
            isAttackActive = false;
        }

        // Se desuscriben los eventos al destruir el componente.
        protected virtual void OnDestroy()
        {
            weapon.OnEnter -= HandleEnter;
            weapon.OnExit -= HandleExit;
        }
    }

    /*---------------------------------------------------------------------------------------------
    Variante genérica del WeaponComponent para componentes que usan datos de ataque específicos.
    T1: Tipo de datos de componente (por ejemplo, config. general del arma)
    T2: Tipo de datos de ataque individual (por ejemplo, una animación o combo específico)
    ---------------------------------------------------------------------------------------------*/
    public abstract class WeaponComponent<T1, T2> : WeaponComponent where T1 : ComponentData<T2> where T2 : AttackData
    {
        // Configuración general del componente
        protected T1 data;

        // Datos del ataque actual
        protected T2 currentAttackData;

        // Al iniciar un ataque, se seleccionan los datos correspondientes al ataque actual.
        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentAttackData = data.GetAttackData(weapon.CurrentAttackCounter);
        }

        // Se inicializa el componente recuperando su configuración de datos.
        public override void Init()
        {
            base.Init();

            data = weapon.Data.GetData<T1>();
        }
    }
}
