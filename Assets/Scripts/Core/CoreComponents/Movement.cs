using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class Movement : CoreComponent
    {
        #region References

        #endregion

        #region Integers
        public int FacingDirection { get; private set; }
        #endregion

        #region Floats

        #endregion

        #region Flags
        public bool CanSetVelocity { get; set; }
        #endregion

        #region Components
        public Rigidbody2D RB { get; private set; }
        #endregion

        #region Transforms

        #endregion

        #region Vectors
        public Vector2 CurrentVelocity { get; private set; }

        private Vector2 workSpace;
        #endregion

        #region Unity CallBack Functions Override
        protected override void Awake()
        {
            base.Awake();

            //Get Components
            RB = GetComponentInParent<Rigidbody2D>();

            //Initialize
            FacingDirection = 1;
            CanSetVelocity = true;
        }
        #endregion

        #region Own Functions
        public override void LogicUpdate()
        {
            CurrentVelocity = RB.velocity;
        }
        #endregion

        #region Set Funtions
        public void SetVelocityZero()
        {
            workSpace = Vector2.zero;
            SetFinalVelocity();
        }
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workSpace.Set(angle.x * velocity * direction, angle.y * velocity);
            SetFinalVelocity();
        }

        public void SetVelocity(float velocity, Vector2 direction)
        {
            workSpace = direction * velocity;
            SetFinalVelocity();
        }

        public void SetVelocityX(float velocity)
        {
            workSpace.Set(velocity, CurrentVelocity.y);
            SetFinalVelocity();
        }

        public void SetVelocityY(float velocity)
        {
            workSpace.Set(CurrentVelocity.x, velocity);
            SetFinalVelocity();
        }

        private void SetFinalVelocity()
        {
            if (CanSetVelocity)
            {
                RB.velocity = workSpace;
                CurrentVelocity = workSpace;
            }
        }
        #endregion

        #region Check Functions
        public void CheckIfShouldFlip(int xInput)
        {
            //Condition that check th valu of th axe "X" to determinate if should Flip the facedirection
            if (xInput != 0 && xInput != FacingDirection)
            {
                Flip();
            }
        }
        #endregion

        #region Other Functions
        public void Flip()
        {
            FacingDirection *= -1;
            RB.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        #endregion
    }
}

