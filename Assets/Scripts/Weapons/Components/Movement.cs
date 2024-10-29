using Avocado.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    public class Movement : WeaponComponent<MovementData, AttackMovement>
    {
        #region References
        private CoreSystem.Movement coreMovement;
        private CoreSystem.Movement CoreMovement => coreMovement ? coreMovement : Core.GetCoreComponent(ref coreMovement);
        #endregion

        #region Functions
        private void HandlerStartMovement()
        {
            CoreMovement.SetVelocity(currentAttackData.Velocity, currentAttackData.Direction, CoreMovement.FacingDirection);
        }

        private void HandlerStopMovement()
        {
            CoreMovement.SetVelocityZero();
        }
        #endregion

        #region Override Functions
        protected override void Start()
        {
            base.Start();

            eventHandler.OnStartMovement += HandlerStartMovement;
            eventHandler.OnStopMovement += HandlerStopMovement;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            eventHandler.OnStartMovement -= HandlerStartMovement;
            eventHandler.OnStopMovement -= HandlerStopMovement;
        }
        #endregion
    }
}
