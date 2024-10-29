using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerWallClimbState : PlayerTouchingWallState
    {
        //---PlayerWallClimbState Vars---//
        #region PlayerWallClimbState Vars
        #endregion

        //---PlayerWallClimbState Construct---//
        #region Construct
        public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
                //Set velocity of the climb
                Movement?.SetVelocityY(playerData.wallClimbVelocity);

                //Condition that chenge the state back to the "WallGrabState"
                if (yInput != 1)
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