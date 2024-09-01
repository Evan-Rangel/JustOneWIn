using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    //---PlayerCrouchIdleState Vars---//
    #region PlayerCrouchIdleState Vars
    #endregion

    //---PlayerCrouchIdleState Construct---//
    #region Construct
    public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }


    #endregion

    //---Override Functions---//
    #region Override Functions
    public override void Enter()
    {
        base.Enter();

        //Set velocity
        player.SetVelocityZero();

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
            //Condition that know when player move on axe "x" and previusly we are in crocuhstate, if is true then change the state to "CrouchMoveState"
            if (xInput != 0)//---> CrouchMoveState
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
            else if (yInput != -1 && !isTouchingCeiling)//---> IdleState
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
    #endregion

    //---Other Functions---//
    #region Other Functions

    #endregion

}
