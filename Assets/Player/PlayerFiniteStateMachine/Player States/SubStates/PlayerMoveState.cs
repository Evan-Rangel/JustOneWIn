using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    //---PlayerMoveState Vars---//
    #region PlayerMoveState Vars
    #endregion

    //---PlayerMoveState Construct---//
    #region Construct
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        //Check if should Flip the face direction
        player.CheckIfShouldFlip(xInput);

        //Set velocity
        player.SetVelocityX(playerData.movementVelocity * xInput);

        //Condition that know when player move on axe "x", if is false then change the state to "IdleState"
        if (xInput == 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.IdleState);
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
