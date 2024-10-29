using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class RangeAttackState : AttackState
    {
        #region References 
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
        private Movement movement;
        private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
        private CollisionSenses collisionSenses;

        protected D_RangeAttackState stateData;
        #endregion

        #region Projectiles
        protected GameObject projectile;
        protected Projectile projectileScript;
        #endregion

        #region Constructor
        public RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
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

            projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.FireProjectil(stateData.projectileSpeed, stateData.projectileTravelDistance, stateData.projectileDamage);

        }

        public override void FinishAttack()
        {
            base.FinishAttack();
        }
        #endregion
    }
}