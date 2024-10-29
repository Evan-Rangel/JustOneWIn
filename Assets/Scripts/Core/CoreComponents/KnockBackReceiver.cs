using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class KnockBackReceiver : CoreComponent, IKnockBackable
    {
        #region References
        private CoreComp<Movement> movement;
        private CoreComp<CollisionSenses> collisionSenses;
        #endregion

        #region Integers
        #endregion

        #region Floats
        [SerializeField] private float maxKnockBackTime = 0.2f;
        private float knockBackStartTime;
        #endregion

        #region Flags
        private bool isKnockBackActive;
        #endregion

        #region Components
        #endregion

        #region Transforms
        #endregion

        #region Vectors
        #endregion

        #region Unity CallBack Functions Override
        protected override void Awake()
        {
            base.Awake();

            movement = new CoreComp<Movement>(core);
            collisionSenses = new CoreComp<CollisionSenses>(core);
        }
        #endregion

        #region Own Functions
        public override void LogicUpdate()
        {
            CheckKnoackBack();
        }
        #endregion

        #region Set Funtions
        #endregion

        #region Interfaces Function
        public void KnockBack(Vector2 angle, float strength, int direction)
        {
            movement.Comp?.SetVelocity(strength, angle, direction);
            movement.Comp.CanSetVelocity = false;
            isKnockBackActive = true;
            knockBackStartTime = Time.time;
        }
        #endregion

        #region Check Functions
        private void CheckKnoackBack()
        {
            //Condition that check the position og the Entity to know that is in ground and stop moving
            if (isKnockBackActive && (movement.Comp?.CurrentVelocity.y <= 0.01f && collisionSenses.Comp.Ground) || Time.time >= knockBackStartTime + maxKnockBackTime)
            {
                isKnockBackActive = false;
                movement.Comp.CanSetVelocity = true;
            }
        }
        #endregion

        #region Other Functions
        #endregion
    }
}

