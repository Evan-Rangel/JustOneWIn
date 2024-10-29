using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerCrouchMoveState : PlayerGroundedState
    {
        //---PlayerCrouchMoveState Vars---//
        #region PlayerCrouchMoveState Vars

        #endregion

        //---PlayerCrouchMoveState Construct---//
        #region Construct
        public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {

        }
        #endregion

        //---Override Functions---//
        #region Override Functions
        public override void Enter()
        {
            base.Enter();

            //Set Collider when crouch
            player.SetColliderHeight(playerData.crouchColliderHeight);
        }

        public override void Exit()
        {
            base.Exit();

            //Set Collider when stand
            player.SetColliderHeight(playerData.standColliderHeight);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //Condition that check that we are not exiting a state
            if (!isExitingState)
            {
                Movement?.SetVelocityX(playerData.crouchMovementVelocity * Movement.FacingDirection);
                Movement?.CheckIfShouldFlip(xInput);

                //Condition that know when player not move on axe "x" and previusly we are in crocuhstate, if is true then change the state to "CrouchIdleState"
                if (xInput == 0)//---> CrouchIdleState
                {
                    stateMachine.ChangeState(player.CrouchIdleState);
                }
                else if (yInput != -1 && !isTouchingCeiling)//---> IdleState
                {
                    stateMachine.ChangeState(player.MoveState);
                }
            }
        }
        #endregion

        //---Other Functions---//
        #region Other Functions

        #endregion

    }
}