using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerIdleState : PlayerGroundedState
    {
        //---PlayerIdleState Vars---//
        #region PlayerIdleState Vars
        #endregion

        //---PlayerIdleState Construct---//
        #region Construct
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {

        }
        #endregion

        //---Override Functions---//
        #region Override Functions
        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //Set velocity
            Movement?.SetVelocityX(0f);

            //Condition that check that we are not exiting a state
            if (!isExitingState)
            {
                //Condition that know when player move on axe "x", if is true then change the state to "MoveState"
                if (xInput != 0)//---> MoveState
                {
                    stateMachine.ChangeState(player.MoveState);
                }
                else if (yInput == -1)//---> CrouchIdleState
                {
                    stateMachine.ChangeState(player.CrouchIdleState);
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
        }
        #endregion

        //---Other Functions---//
        #region Other Functions

        #endregion
    }
}