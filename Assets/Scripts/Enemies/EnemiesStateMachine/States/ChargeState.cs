using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class ChargeState : State
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;

        protected D_ChargeState stateData;
        #endregion

        #region Flags
        protected bool isPlayerInMinAgroRange;
        protected bool isDetectingLedge;
        protected bool isDetectingWall;
        protected bool isChargeTimeOver;
        protected bool performCloseRangeAction;
        #endregion

        #region Construct
        public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData) : base(entity, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }
        #endregion

        #region Override Functions
        public override void Enter()
        {
            base.Enter();

            isChargeTimeOver = false;

            //Set velocity of the charge
            Movement?.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Movement?.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);

            //Condition thats if the time while the scarab is charging then stop that
            if (Time.time >= startTime + stateData.chargeTime)
            {
                isChargeTimeOver = true;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            //Check Range
            isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        }

        public override void DoChecks()
        {
            base.DoChecks();

            //Check Range
            isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();

            //Check for a wall or ledge to stop the charge when reach one of those
            isDetectingLedge = CollisionSenses.LedgeVertical;
            isDetectingWall = CollisionSenses.WallFront;

            //Check for Attack
            performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        }
        #endregion

        //-------OTHER FUNCTIONS-------//

        //-------END OTHERS FUNCTIONS-------//
    }
}