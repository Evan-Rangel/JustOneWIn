using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    //---PlayerWallGrabState Vars---//
    #region PlayerWallGrabState Vars
    private Vector2 holdPosition;
    #endregion

    //---PlayerWallGrabState Construct---//
    #region Construct
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    #endregion

    //---Override Functions---//
    #region Override Functions
    public override void Enter()
    {
        base.Enter();

        //Equal to position of the player to take track of that 
        holdPosition = player.transform.position;

        //Call the function to hold the postion at the start
        HoldPosition();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Condition tha check the flags that indicate iwe stop be in a state, likes "JumpState"
        if (!isExitingState)
        {
            //Call the function to hold the postion and here to continuos hold will the input grab is pressed
            HoldPosition();

            //Condition that check if we are pushing the yInput up to change the state to "WallClimbState"
            if (yInput > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            else if (yInput < 0 || !grabInput)
            {
                stateMachine.ChangeState(player.WallSlideState);
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

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
    #endregion

    //---Other Functions---//
    #region Other Functions
    private void HoldPosition()
    {
        player.transform.position = holdPosition;
        //Set velocity when grab a wall
        //--EXTRA COMMENT--// => No idea why the camera works with tha player velocity, so if we dont make the velocity to 0 the camera will fall
        player.SetVelocityX(0);
        player.SetVelocityY(0);
    }
    #endregion
}
