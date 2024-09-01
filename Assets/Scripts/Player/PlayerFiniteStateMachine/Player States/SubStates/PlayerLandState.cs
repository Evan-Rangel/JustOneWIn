using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    //---PlayerLandState Vars---//
    #region PlayerLandState Vars
    #endregion

    //---PlayerLandState Construct---//
    #region Construct
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    #endregion

    //---Override Functions---//
    #region Override Functions
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Condition tha check the flags that indicate iwe stop be in a state, likes "JumpState"
        if (!isExitingState)
        {
            //Condition if we have an xInput, then transition to move "MoveState" and if not then just run the land animations and "IdleState"
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else if (isAnimationFinished)
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
