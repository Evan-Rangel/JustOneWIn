using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class IdleState : State
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;

        protected D_IdleState stateData;
        #endregion

        #region Flags
        protected bool flipAfterIdle;
        protected bool isIdleTimeOver;
        protected bool isPlayerInMinAgroRange;
        #endregion

        #region Floats
        protected float idleTime;
        #endregion

        #region Construct
        public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }
        #endregion

        #region Override Functions
        public override void Enter()
        {
            base.Enter();

            //Stop Moving
            Movement?.SetVelocityX(0f);
            isIdleTimeOver = false;
            SetRandomIdleTime();
        }

        public override void Exit()
        {
            base.Exit();

            //Condition taht wheb ends, then can flip the character
            if (flipAfterIdle)
            {
                Movement?.Flip();
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Movement?.SetVelocityX(0f);

            //Conditions thats if the time pass the idle time then the idle is over
            if (Time.time >= startTime + idleTime)
            {
                isIdleTimeOver = true;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void DoChecks()
        {
            base.DoChecks();

            //Check Range
            isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        }
        //-------END OVERRIDES-------//

        //-------OTHER FUNCTIONS-------//
        //Function SetFlipAfterIdle
        public void SetFlipAfterIdle(bool flip)
        {
            flipAfterIdle = flip;
        }

        //Funtion SetRandomIdleTime
        private void SetRandomIdleTime()
        {
            idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
        }
        #endregion
    }
}