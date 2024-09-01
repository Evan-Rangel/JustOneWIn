using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    //---PlayerGroundedStates Vars---//
    #region PlayerGroundedStates Vars
    //--Ints--//
    protected int xInput;
    protected int yInput;
    //--Flags--//
    //Inputs flags
    protected bool isTouchingCeiling;

    private bool jumpInput;
    private bool grabInput;
    private bool dashInput;
    //Check flags
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    #endregion

    //---PlayerGroundedState Construct---//
    #region Construct
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    #endregion

    //---Override Functions---//
    #region Override Functions
    public override void Enter()
    {
        base.Enter();

        //Resets
        player.JumpState.ResetAmountOfJumps();
        player.DashState.ResetCanDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Read the inputs used
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;

        //Condition that check the Attack Inputs
        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        //Condition that check the flag of "Jump" to change thse state to "JumpState"
        else if (jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)//---> JumpState
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)//---> InAirState
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (isTouchingWall && grabInput && !isTouchingLedge)//---> WallGrabState
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)//---> DashState
        {
            stateMachine.ChangeState(player.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //Check for Ground
        isGrounded = player.CheckIfGrounded();
        //Check for Wall
        isTouchingWall = player.CheckIfTouchingWall();
        //Check for Ledge
        isTouchingLedge = player.CheckIfTouchingLedge();
        //Check for ceiling
        isTouchingCeiling = player.CheckForCeiling();
    }
    #endregion
}
