using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class MoveState : State
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;

        protected D_MoveState stateData;
        #endregion

        #region Flags
        protected bool isDetectingWall;
        protected bool isDetectingLedge;
        protected bool isPlayerInMinAgroRange;
        #endregion

        #region Constructor
        public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }
        #endregion

        #region Oberride Functions
        public override void Enter()
        {
            base.Enter();//"base" means that when this enter funtion gets called it's going to also call the enter funtion in our base class, which is "State", So if we want to change to completely the functionality of a function we can just remove this based on enter and the code inside of our "State" base won't get call.
                         //Entity Movement
            Movement?.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //This is here to to avoid the Entity to slice when recibe the Knockback
            Movement?.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public override void DoChecks()
        {
            base.DoChecks();

            //Equal to same function
            isDetectingWall = CollisionSenses.WallFront;
            isDetectingLedge = CollisionSenses.LedgeVertical;
            isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        }
        #endregion
    }
}