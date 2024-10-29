using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerLedgeClimbState : PlayerState
    {
        //---PlayerLedgeClimbState Vars---//
        #region References
        protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;
        #endregion

        #region PlayerLedgeClimbState Vars
        //Vectors
        private Vector2 detectedPos;
        private Vector2 cornerPos;
        private Vector2 startPos;
        private Vector2 stopPos;
        private Vector2 workSpace;
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
            Movement?.SetVelocityZero();
            player.transform.position = detectedPos;
            //Determinate the corner position
            cornerPos = DetermineCornerPosition();

            //Set the pos we start the ledge climb and when should stop
            startPos.Set(cornerPos.x - (Movement.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
            stopPos.Set(cornerPos.x + (Movement.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

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
                Movement.SetVelocityZero();
                player.transform.position = startPos;

                //Condition that check if we are hanging on a ledge and, if that true and press some input forward then climb, if teh input is down then stop hanging
                if (xInput == Movement.FacingDirection && isHanging && !isClimbing)//---> Input is: (Right), then Climb
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
            isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * Movement.FacingDirection * 0.015f), Vector2.up, playerData.standColliderHeight, CollisionSenses.WhatIsGround);

            player.Animator.SetBool("isTouchingCeiling", isTouchingCeiling);
        }

        private Vector2 DetermineCornerPosition()
        {
            RaycastHit2D xHit = Physics2D.Raycast(CollisionSenses.WallCheck.position, Vector2.right * Movement.FacingDirection, CollisionSenses.WallCheckDistance, CollisionSenses.WhatIsGround);
            float xDist = xHit.distance;
            workSpace.Set((xDist + 0.015f) * Movement.FacingDirection, 0f);
            //workSpace.Set(xDist * FacingDirection, 0f);
            RaycastHit2D yHit = Physics2D.Raycast(CollisionSenses.LedgeCheckHorizontal.position + (Vector3)(workSpace), Vector2.down, CollisionSenses.LedgeCheckHorizontal.position.y - CollisionSenses.WallCheck.position.y + 0.015f, CollisionSenses.WhatIsGround);
            float yDist = yHit.distance;

            workSpace.Set(CollisionSenses.WallCheck.position.x + (xDist * Movement.FacingDirection), CollisionSenses.LedgeCheckHorizontal.position.y - yDist);

            return workSpace;
        }
        #endregion
    }
}