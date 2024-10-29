using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class StunState : State
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;

        protected D_StunState stateData;
        #endregion

        #region Flags
        protected bool isStunTimeOver;
        protected bool isGrounded;
        protected bool isMovementStopped;
        protected bool performCloseRangeAction;
        protected bool isPlayerInMinAgroRange;
        #endregion

        #region Constructor
        public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(entity, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }
        #endregion

        #region Override Functions
        public override void Enter()
        {
            base.Enter();

            //Stun start off
            isStunTimeOver = false;
            isMovementStopped = false;
            //Set stun
            Movement?.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);
        }

        public override void Exit()
        {
            base.Exit();

            //Stun reset
            entity.ResetStunResistance();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //Condition that keep track of stuntime
            if (Time.time >= startTime + stateData.stunTime)
            {
                isStunTimeOver = true;
            }
            //Condition that know when the enemy is grounded and the time of knockback ends to allow another knockback
            if (isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
            {
                isMovementStopped = true;
                Movement?.SetVelocityX(0f);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void DoChecks()
        {
            base.DoChecks();

            //Ground Check
            isGrounded = CollisionSenses.Ground;
            //Player Check
            performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
            isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        }
        #endregion
    }
}