using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    //---PlayerLedgeClimbState Vars---//
    #region PlayerLedgeClimbState Vars
    //Vectors
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    //Flags
    private bool isHanging;
    private bool isClimbing;
    private bool isTouchingCeiling;

    private bool jumpInput;
    //Ints
    private int xInput;
    private int yInput;
    #endregion

    //---PlayerLedgeClimbState Construct---//
    #region Construct
    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    #endregion

    //---Override Functions---//
    #region Override Functions
    public override void Enter()
    {
        base.Enter();

        //Set velocity to zero, to avoid moving the player out of the climb
        player.SetVelocityZero();
        player.transform.position = detectedPos;
        //Determinate the corner position
        cornerPos = player.DetermineCornerPosition();

        //Set the pos we start the ledge climb and when should stop
        startPos.Set(cornerPos.x - (player.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (player.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        //Condition that check when stop hanging
        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Condition that check when the animation ends, to stop the action of climb
        if (isAnimationFinished)
        {
            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else 
        {
            //Equal to the inputs
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;

            jumpInput = player.InputHandler.JumpInput;

            //Set velocity to zero, to avoid moving the player out of the climb, this is doit again becasuse we change the player pos
            player.SetVelocityZero();
            player.transform.position = startPos;

            //Condition that check if we are hanging on a ledge and, if that true and press some input forward then climb, if teh input is down then stop hanging
            if (xInput == player.FacingDirection && isHanging && !isClimbing)//---> Input is: (Right), then Climb
            {
                CheckForSpace();//Before climb check if we are gonne climb a short space were is needed to be in crouchstate
                isClimbing = true;
                player.Animator.SetBool("climbLedge", true);
            }
            else if (yInput == -1 && isHanging && !isClimbing)//---> Input is: (Down), then Stop Hanging
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }    
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        //Flag that indicate that we are hanging (hold andimation)
        isHanging = true;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        //Stop the climbing
        player.Animator.SetBool("climbLedge", false);
    }
    #endregion

    //---Other Functions---//
    #region Other Functions
    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

    private void CheckForSpace()
    {
        isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * player.FacingDirection * 0.015f), Vector2.up, playerData.standColliderHeight, playerData.whatIsGround);

        player.Animator.SetBool("isTouchingCeiling", isTouchingCeiling);
    }
    #endregion
}
