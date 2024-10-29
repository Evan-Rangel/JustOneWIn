using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerAbilityState : PlayerState
    {
        //---PlayerAbilityState Vars---//
        #region References
        protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;
        #endregion

        #region Flags
        protected bool isAbilityDone;
        private bool isGrounded;
        #endregion

        //---PlayerGroundedState Construct---//
        #region Construct
        public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {

        }
        #endregion

        //---Override Functions---//
        #region Override Functions
        public override void Enter()
        {
            base.Enter();

            //Set Flags
            isAbilityDone = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //Condition that check if the ability ends
            if (isAbilityDone)
            {
                if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
                else
                {
                    stateMachine.ChangeState(player.InAirState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public override void DoChecks()
        {
            base.DoChecks();

            if (CollisionSenses)
            {
                //Check ground
                isGrounded = CollisionSenses.Ground;
            }
        }
        #endregion
    }
}