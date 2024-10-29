using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerInAirState : PlayerState
    {

        //---PlayerInAirState Vars---//
        #region References
        protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;
        #endregion

        #region Integers
        private int xInput;
        #endregion

        #region Flags
        //Inputs flags
        private bool jumpInput;
        private bool jumpInputStop;
        private bool grabInput;
        private bool dashInput;
        //Check flags
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isTouchingWallBack;
        private bool oldIsTouchingWall;
        private bool oldIsTouchingWallBack;
        private bool isTouchingLedge;
        //Other flags
        private bool coyoteTime;
        private bool wallJumpCoyoteTime;
        private bool isJumping;
        //--Floats--//
        private float startWallJumpCoyoteTime;
        #endregion

        //---PlayerGroundedState Construct---//
        #region Construct
        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

            //Set to false when we end
            oldIsTouchingWall = false;
            oldIsTouchingWallBack = false;
            isTouchingWall = false;
            isTouchingWallBack = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //Check CoyoteTime
            CheckCoyoteTime();
            CheckWallJumpCoyoteTime();

            //Update th xInput to move in air
            xInput = player.InputHandler.NormInputX;
            jumpInput = player.InputHandler.JumpInput;
            jumpInputStop = player.InputHandler.JumpInputStop;
            grabInput = player.InputHandler.GrabInput;
            dashInput = player.InputHandler.DashInput;

            //Check for time jumping
            CheckJumpMultiplier();

            //Condition that check the Attack Inputs
            if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
            {
                stateMachine.ChangeState(player.PrimaryAttackState);
            }
            else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
            {
                stateMachine.ChangeState(player.SecondaryAttackState);
            }
            //Condition that check if we stop moving in the "Y" axe and we are touching the ground to change the state to "LandState"
            else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)//---> LandState
            {
                stateMachine.ChangeState(player.LandState);
            }
            else if (isTouchingWall && !isTouchingLedge && !isGrounded)//---> LedgeClimbState
            {
                stateMachine.ChangeState(player.LedgeClimbState);
            }
            else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))//---> WallJumpState
            {
                StopWallJumpCoyoteTime();
                isTouchingWall = CollisionSenses.WallFront;
                player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
            }
            else if (jumpInput && player.JumpState.CanJump())//---> JumpState
            {
                stateMachine.ChangeState(player.JumpState);
            }
            else if (isTouchingWall && grabInput && isTouchingLedge)//---> WallGrabState
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if (isTouchingWall && xInput == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0)//---> WallSlideState
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
            else if (dashInput && player.DashState.CheckIfCanDash())//---> DashState
            {
                stateMachine.ChangeState(player.DashState);
            }
            else
            {
                Movement?.CheckIfShouldFlip(xInput);
                Movement?.SetVelocityX(playerData.movementVelocity * xInput);

                player.Animator.SetFloat("yVelocity", Movement.CurrentVelocity.y);
                player.Animator.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public override void DoChecks()
        {
            base.DoChecks();

            //Old Checks
            oldIsTouchingWall = isTouchingWall;
            oldIsTouchingWallBack = isTouchingWallBack;

            if (CollisionSenses)
            {
                //Check ground
                isGrounded = CollisionSenses.Ground;
                //Check wall
                isTouchingWall = CollisionSenses.WallFront;
                isTouchingWallBack = CollisionSenses.WallBack;
                isTouchingLedge = CollisionSenses.LedgeHorizontal;
            }

            //Condition that check if the detector of wall is activate ande de detector of ledge is not activate, this will mean that we are in a wall butt, up ahead are space that we can climb
            if (isTouchingWall && !isTouchingLedge)
            {
                player.LedgeClimbState.SetDetectedPosition(player.transform.position);
            }

            //Condition that check if Coyote time is posible to doit
            if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall && oldIsTouchingWallBack))
            {
                StartWallJumpCoyoteTime();
            }
        }
        #endregion

        //---Other Functions---//
        #region Other Functions
        private void CheckJumpMultiplier()
        {
            //Condition that check if we are jumping and with this knoww when stop pushing the jump input to do a variation in the hieght of the jump depending on the time the input is pressed
            if (isJumping)
            {
                if (jumpInputStop)
                {
                    Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                    isJumping = false;
                }
                else if (Movement?.CurrentVelocity.y <= 0f)
                {
                    isJumping = false;
                }
            }
        }
        private void CheckCoyoteTime()
        {
            //Condition that check the time left to know if we can activate the coyote time
            if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
            {
                coyoteTime = false;
                player.JumpState.DeecreaseAmountOfJumpsLeft();
            }
        }

        private void CheckWallJumpCoyoteTime()
        {
            //Condition that check the time left to know if we can activate the coyote time
            if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
            {
                wallJumpCoyoteTime = false;
            }
        }

        public void StartCoyoteTime() => coyoteTime = true;

        public void StartWallJumpCoyoteTime()
        {
            wallJumpCoyoteTime = true;
            startWallJumpCoyoteTime = Time.time;
        }

        public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

        public void SetIsJumping() => isJumping = true;
        #endregion
    }
}