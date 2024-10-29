using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class MeleeAttackState : AttackState
    {
        #region References
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;

        protected D_MeleeAttack stateData;
        #endregion

        #region Flags
        #endregion

        #region Transforms
        #endregion

        #region Construct
        public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData) : base(entity, stateMachine, animBoolName, attackPosition)
        {
            this.stateData = stateData;
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
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void TriggerAttack()
        {
            base.TriggerAttack();

            //Detectd Objects
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

            //Damage Player
            foreach (Collider2D collider in detectedObjects)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.Damage(stateData.attackDamage);
                }

                IKnockBackable knockbackable = collider.GetComponent<IKnockBackable>();

                //Condition that check if the player attack something that is considerable "knockbackable" then put that in the list
                if (knockbackable != null)
                {
                    knockbackable.KnockBack(stateData.knoackbackAngle, stateData.knoackbackStrength, Movement.FacingDirection);
                }
            }
        }

        public override void FinishAttack()
        {
            base.FinishAttack();
        }
        #endregion
    }
}
