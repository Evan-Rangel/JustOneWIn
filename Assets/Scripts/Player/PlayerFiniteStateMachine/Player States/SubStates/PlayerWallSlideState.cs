using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerWallSlideState : PlayerTouchingWallState
    {
        //---PlayerWallSlideState Vars---//
        #region PlayerWallSlideState Vars
        #endregion

        //---PlayerWallSlideState Construct---//
        #region Construct
        public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {

        }
        #endregion

        //---Override Functions---//
        #region Override Functions
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //Condition that check that we still in the state
            if (!isExitingState)
            {
                //Set velocity when slide
                Movement?.SetVelocityY(-playerData.wallSlideVelocity);

                //Condition that transition back to the "WallGrabState" if we are wallSliding
                if (grabInput && yInput == 0f)
                {
                    stateMachine.ChangeState(player.WallGrabState);
                }
            }
        }
        #endregion

        //---Other Functions---//
        #region Other Functions

        #endregion
    }
}