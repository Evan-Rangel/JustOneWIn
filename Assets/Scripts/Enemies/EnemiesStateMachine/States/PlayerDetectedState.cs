using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerDetectedState : State
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;

        protected D_PlayerDetectedState stateData;
        #endregion

        #region Flags
        protected bool isPlayerInMinAgroRange;
        protected bool isPlayerInMaxAgroRange;
        protected bool performLongRangeAction;
        protected bool performCloseRangeAction;
        protected bool isDetectingLedge;
        #endregion

        #region Constructor
        public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData) : base(entity, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }
        #endregion

        #region Override Functions
        public override void Enter()
        {
            base.Enter();

            performLongRangeAction = false;

            //Detects Player then Stop
            Movement?.SetVelocityX(0f);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Movement?.SetVelocityX(0f);

            //Condition thats check the time to realize the action
            if (Time.time >= startTime + stateData.longRangeActionTime)
            {
                performLongRangeAction = true;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void DoChecks()
        {
            base.DoChecks();

            //Detect Player
            isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
            isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();

            //Perform Attacks
            performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();

            //Detectors
            isDetectingLedge = CollisionSenses.LedgeVertical;
        }
        #endregion
    }
}
