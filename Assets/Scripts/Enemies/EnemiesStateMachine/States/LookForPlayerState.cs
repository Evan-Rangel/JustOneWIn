using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class LookForPlayerState : State
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;

        protected D_LookForPlayerState stateData;
        #endregion

        #region Flags
        protected bool turnImmediately;
        protected bool isPlayerInMinAgroRange;
        protected bool isAllTurnsDone;
        protected bool isAllTurnsTimeDone;
        #endregion

        #region Floats
        protected float lastTurnTime;
        #endregion

        #region Integers
        protected int amountOfTurnsDone;
        #endregion

        #region Constructor
        public LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData) : base(entity, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }
        #endregion

        #region Override Functions
        public override void Enter()
        {
            base.Enter();

            //Initialize thinks
            isAllTurnsDone = false;
            isAllTurnsTimeDone = false;

            lastTurnTime = startTime;
            amountOfTurnsDone = 0;

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

            //Condition that indicate when the force flip occurs
            if (turnImmediately)
            {
                Movement?.Flip();
                lastTurnTime = Time.time;
                amountOfTurnsDone++;
                turnImmediately = false;
            }
            else if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)
            {
                Movement?.Flip();
                lastTurnTime = Time.time;
                amountOfTurnsDone++;
            }

            //Condition that check if we done doing all the flip searching for the player
            if (amountOfTurnsDone >= stateData.amountOfTurns)
            {
                isAllTurnsDone = true;
            }

            //This condition know the final turn to do a transition to other state
            if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
            {
                isAllTurnsTimeDone = true;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void DoChecks()
        {
            base.DoChecks();

            //Checks
            isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        }
        //-------END OVERRIDES-------//

        //-------OTHER FUNCTIONS-------//
        public void SetTurnImmediately(bool flip)
        {
            turnImmediately = flip;
        }
        #endregion
    }
}