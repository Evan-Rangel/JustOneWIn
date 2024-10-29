using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class DodgeState : State
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;

        protected D_DodgeState stateData;
        #endregion

        #region Flags
        protected bool performCloseRangeAction;
        protected bool isPlayerInMaxAgroRange;
        protected bool isGrounded;
        protected bool isDodgeOver;
        #endregion

        #region Construct
        public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData) : base(entity, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }
        #endregion

        #region Override Functions
        public override void Enter()
        {
            base.Enter();

            //Initialize off
            isDodgeOver = false;
            //Set Velocity
            Movement?.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -Movement.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //Condition that check the time used and left of the dodge
            if (Time.time >= startTime + stateData.dodgeTime && isGrounded)
            {
                isDodgeOver = true;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public override void DoChecks()
        {
            base.DoChecks();

            //Equal checks
            performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
            isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
            isGrounded = CollisionSenses.Ground;
        }
        #endregion
    }
}