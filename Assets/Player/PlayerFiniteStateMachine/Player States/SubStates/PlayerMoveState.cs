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

        //Condition that check that we are not exiting a state
        if (!isExitingState)
        {
            //Condition that know when player not move on axe "x", if is true then change the state to "MoveState"
            if (xInput == 0)//---> IdleState
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (yInput == -1)//---> CrouchIdleState
            {
                stateMachine.ChangeState(player.CrouchMoveState);
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
