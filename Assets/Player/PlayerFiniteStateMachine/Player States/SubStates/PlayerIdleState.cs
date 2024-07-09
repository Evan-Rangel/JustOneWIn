using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        player.SetVelocityX(0f);

        //Condition that know when player move on axe "x", if is true then change the state to "MoveState"
        if (xInput != 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.MoveState);
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
