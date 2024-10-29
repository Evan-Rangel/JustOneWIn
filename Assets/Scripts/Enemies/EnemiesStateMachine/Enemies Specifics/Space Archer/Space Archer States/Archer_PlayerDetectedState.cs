using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class Archer_PlayerDetectedState : PlayerDetectedState
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;

        private SpaceArcher archer;
        #endregion

        #region Constructor
        public Archer_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, SpaceArcher archer) : base(entity, stateMachine, animBoolName, stateData)
        {
            this.archer = archer;
        }
        #endregion

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

            //Condition that detected if player is enough close to do the action
            if (performCloseRangeAction)
            {
                if (Time.time >= archer.dodgeState.startTime + archer.dodgeStateData.dodgeCooldown)//Dodge
                {
                    stateMachine.ChangeState(archer.dodgeState);
                }
                else//MeleeAttack
                {
                    stateMachine.ChangeState(archer.meleeAttackState);
                }
            }
            else if (performLongRangeAction) //Condition that if not a player detected, then trasition back to Idle
            {
                stateMachine.ChangeState(archer.rangeAttackState);
            }
            /*
            else if (!isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(archer.lookForPlayerState);
            }
            */
            else if (!isDetectingLedge)
            {
                Movement.Flip();
                stateMachine.ChangeState(archer.moveState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        #endregion
    }
}