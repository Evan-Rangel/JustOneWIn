using System;
using Avocado.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        #region References
        protected Weapon weapon;
        protected Core Core => weapon.Core;
        // TODO: Fix this when finished weapon data
        //protected AnimationEventHandler EventHandler => weapon.EventHandler;
        protected AnimationEventHandler eventHandler;
        #endregion

        #region Flags
        protected bool isAttackActive;
        public void SetIsAttackActive(bool value) { isAttackActive = value; }
        #endregion

        #region Functions

        public virtual void Init()
        {

        }

        protected virtual void Awake()
        {
            weapon = GetComponent<Weapon>();

            eventHandler = GetComponentInChildren<AnimationEventHandler>();
        }

        protected virtual void Start()
        {
            weapon.OnEnter += HandleEnter;
            weapon.OnExit += HandleExit;
        }

        protected virtual void HandleEnter()
        {
            isAttackActive = true;
            GameManager.instance.ChangeAttacActive(isAttackActive);
        }

        protected virtual void HandleExit()
        {
            isAttackActive = false;
            GameManager.instance.ChangeAttacActive(isAttackActive);

        }

        protected virtual void OnDestroy()
        {
            weapon.OnEnter -= HandleEnter;
            weapon.OnExit -= HandleExit;
        }
        #endregion
    }

    public abstract class WeaponComponent<T1, T2> : WeaponComponent where T1 : ComponentData<T2> where T2 : AttackData
    {
        protected T1 data;
        protected T2 currentAttackData;

        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentAttackData = data.AttackData[weapon.CurrentAttackCounter];
        }

        public override void Init()
        {
            base.Init();

            data = weapon.Data.GetData<T1>();
        }
    }
}
