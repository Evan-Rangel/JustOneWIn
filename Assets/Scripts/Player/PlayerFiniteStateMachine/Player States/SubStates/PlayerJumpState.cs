using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    //---PlayerJumpState Vars---//
    #region PlayerJumpState Vars
    private int amountOfJumpsLeft;
    #endregion

    //---PlayerJumpState Construct---//
    #region Construct
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        //Set Vars with Data
        amountOfJumpsLeft = playerData.amountOfJumps;
    }
    #endregion

    //---Override Functions---//
    #region Override Functions
    public override void Enter()
    {
        base.Enter();

        //Use the Jump Inputs
        player.InputHandler.UseJumpInput();
        //Set velocity
        player.SetVelocityY(playerData.jumpVelocity);
        //Set Flags
        isAbilityDone = true;
        //Deacrese Jumps
        amountOfJumpsLeft--;
        //Call functions
        player.InAirState.SetIsJumping();
        
    }
    #endregion

    //---Other Functions---//
    #region Other Functions
    public bool CanJump()
    {
        //Condition tha check if there ledt jumps to be used
        if (amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmountOfJumps() => amountOfJumpsLeft = playerData.amountOfJumps;

    public void DeecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
    #endregion
}
