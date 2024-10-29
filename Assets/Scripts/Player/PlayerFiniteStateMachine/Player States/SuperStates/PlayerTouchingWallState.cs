using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerTouchingWallState : PlayerState
    {
        //---PlayerTouchingWallState Vars---//
        #region References
        protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;
        #endregion

        #region Integers
        protected int xInput;
        protected int yInput;
        #endregion

        #region Flags
        //Flags
        protected bool isGrounded;
        protected bool isTouchingWall;
        protected bool isTouchingLedge;

        protected bool grabInput;
        protected bool jumpInput;
        #endregion

        //---PlayerTouchingWallState Construct---//
        #region Construct
        public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

            //Equal to input Axes
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            //Equal Inputs
            grabInput = player.InputHandler.GrabInput;
            jumpInput = player.InputHandler.JumpInput;

            //Condition that check the Jump Input
            if (jumpInput)
            {
                player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
            }
            //Condition to change State depending if we are in wall, or stop be in wall or activate the grabe state or climb state
            else if (isGrounded && !grabInput)//---> IdleState
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (!isTouchingWall || (xInput != Movement?.FacingDirection && !grabInput))//---> InAirState
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (isTouchingWall && !isTouchingLedge)//---> LedgeClimbState
            {
                stateMachine.ChangeState(player.LedgeClimbState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public override void DoChecks()
        {
            base.DoChecks();

            if (CollisionSenses)
            {
                //Equal to functions
                isGrounded = CollisionSenses.Ground;
                isTouchingWall = CollisionSenses.WallFront;
                isTouchingLedge = CollisionSenses.LedgeHorizontal;
            }


            //Condition that detect if there is a ledge to climb to set the detected pos
            if (isTouchingWall && !isTouchingLedge)
            {
                player.LedgeClimbState.SetDetectedPosition(player.transform.position);
            }
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
        }
        #endregion
    }
}