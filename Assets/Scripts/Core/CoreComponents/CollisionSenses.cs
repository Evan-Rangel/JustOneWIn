using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class CollisionSenses : CoreComponent
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        #endregion

        #region Integers
        #endregion

        #region Floats
        [Header("Values of Detectors")]
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private float wallCheckDistance;
        #endregion

        #region ReDeclare Floats
        public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
        public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }

        #endregion

        #region Flags
        #endregion

        #region Components
        #endregion

        #region Check Transforms
        [Header("Detectors")]

        [SerializeField][Tooltip("For Player And Enemy")] private Transform groundCheck;
        [SerializeField][Tooltip("For Player And Enemy")] private Transform wallCheck;
        [SerializeField][Tooltip("For Player")] private Transform ledgeCheckHorizontal;
        [SerializeField][Tooltip("For Enemy")] private Transform ledgeCheckVertical;
        [SerializeField][Tooltip("For Player")] private Transform ceilingCheck;
        #endregion

        #region ReDeclare Check Transforms
        public Transform GroundCheck
        {
            get => GenericNotImplementedError<Transform>.TryGet(groundCheck, core.transform.parent.name);
            private set => groundCheck = value;
        }
        public Transform WallCheck
        {
            get => GenericNotImplementedError<Transform>.TryGet(wallCheck, core.transform.parent.name);
            private set => wallCheck = value;
        }
        public Transform LedgeCheckHorizontal
        {
            get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckHorizontal, core.transform.parent.name);
            private set => ledgeCheckHorizontal = value;
        }
        public Transform LedgeCheckVertical
        {
            get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckVertical, core.transform.parent.name);
            private set => ledgeCheckVertical = value;
        }
        public Transform CeilingCheck
        {
            get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, core.transform.parent.name);
            private set => ceilingCheck = value;
        }
        #endregion

        #region Vectors
        #endregion

        #region LayerMasks
        [SerializeField] private LayerMask whatIsGround;
        #endregion

        #region ReDeclare LayerMasks
        public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }
        #endregion

        #region Unity CallBack Functions Override

        #endregion

        #region Own Functions

        #endregion

        #region Set Funtions

        #endregion

        #region Check Functions
        public bool Ground
        {
            get => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);
        }

        public bool Ceiling
        {
            get => Physics2D.OverlapCircle(CeilingCheck.position, groundCheckRadius, whatIsGround);
        }

        public bool WallFront
        {
            get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
        }

        public bool LedgeHorizontal
        {
            get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
        }

        public bool LedgeVertical
        {
            get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround);
        }

        public bool WallBack
        {
            get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround);
        }
        #endregion

        #region Other Functions

        #endregion
    }
}

