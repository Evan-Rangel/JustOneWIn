using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    //---PlayerWallJumpState Vars---//
    #region PlayerWallJumpState Vars
    private int wallJumpDirection;
    #endregion

    //---PlayerWallJumpState Construct---//
    #region Construct
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    #endregion

    //---Override Functions---//
    #region Override Functions
    public override void Enter()
    {
        base.Enter();

        //Use the Jump Inputs
        player.InputHandler.UseJumpInput();
        //Set wall Jump Velocity
        player.JumpState.ResetAmountOfJumps();
        player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        player.CheckIfShouldFlip(wallJumpDirection);
        player.JumpState.DeecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Pass "X" and "Y" velocity to the animator
        player.Animator.SetFloat("yVelocity", player.CurrentVelocity.y);
        player.Animator.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));

        //Condition that check if our walljump time is over
        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }
    #endregion

    //---Other Functions---//
    #region Other Functions
    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -player.FacingDirection;
        }
        else
        {
            wallJumpDirection = player.FacingDirection;
        }
    }
    #endregion

}
