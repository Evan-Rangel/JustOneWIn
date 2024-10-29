using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class Entity : MonoBehaviour
    {
        #region References
        private Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }
        private Movement movement;

        public Core Core { get; private set; }
        public FiniteStateMachine stateMachine;

        protected Stats stats;

        //D_Entity 
        [Header("Base Enemy Data")]
        public D_Entity entityData;
        #endregion

        #region Integers
        public int lastDamageDirection { get; private set; }
        #endregion

        #region Floats
        private float currentHealth;
        private float currentStunResistance;
        private float lastDamageTime;
        #endregion

        #region Flags
        protected bool isStunned;
        protected bool isDead;
        #endregion

        #region Components
        public Animator animator { get; private set; }
        public AnimationToStateMachine atsm { get; private set; }
        #endregion

        #region Transforms
        //Detectors
        [Header("Detectors")]
        [SerializeField]
        [Tooltip("This will detect a wall to enemy turn around")]
        private Transform wallCheck;
        [SerializeField]
        [Tooltip("This will detect a ledge to enemy avoid fall")]
        private Transform ledgeCheck;
        [SerializeField]
        [Tooltip("This will detect a GameObject with the tag and layer -Player-")]
        private Transform playerCheck;
        [SerializeField]
        [Tooltip("This will detect the ground to do thinks like friction, and stop the enemy")]
        private Transform groundCheck;
        #endregion

        #region Vectors
        private Vector2 velocityWorkspace;
        #endregion

        #region Virtual Functions
        //Virtual Start
        public virtual void Awake()//"Virtual" means that this can be redefind in the derived classes
        {
            Core = GetComponentInChildren<Core>();

            stats = Core.GetCoreComponent<Stats>();

            currentHealth = entityData.maxHealth;
            currentStunResistance = entityData.stunResistance;

            //Initialice Reference
            animator = GetComponent<Animator>();
            atsm = GetComponent<AnimationToStateMachine>();

            stateMachine = new FiniteStateMachine();//Every entity have his own state machine, that a instance of finite state machine.
        }

        //Virtual Update
        public virtual void Update()
        {
            Core.LogicUpdate();

            stateMachine.currentState.LogicUpdate();

            animator.SetFloat("yVelocity", Movement.RB.velocity.y);

            //Condition that take track of the stunRecovery to indicate that stun is over
            if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
            {
                ResetStunResistance();
            }
        }

        //Virtual FixedUpdate
        public virtual void FixedUpdate()
        {
            stateMachine.currentState.PhysicsUpdate();//Physics in FixedUpdate to avoid frame errors
        }

        //Virtual CheckPlayerInMinAgroRange
        public virtual bool CheckPlayerInMinAgroRange()
        {
            return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
        }

        //Virtual CheckPlayerInMaxAgroRange
        public virtual bool CheckPlayerInMaxAgroRange()
        {
            return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
        }

        //Virtual CheckPlayerInCloseRangeAction
        public virtual bool CheckPlayerInCloseRangeAction()
        {
            return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
        }

        //Virtual DamageHop
        public virtual void DamageHop(float velocity)
        {
            velocityWorkspace.Set(Movement.RB.velocity.x, velocity);
            Movement.RB.velocity = velocityWorkspace;
        }
        //Virtual ResetStunResistance
        public virtual void ResetStunResistance()
        {
            isStunned = false;
            currentStunResistance = entityData.stunResistance;
        }

        //Virtual OnDrawGizmos
        public virtual void OnDrawGizmos()
        {
            if (Core != null)
            {
                //Wall Detector
                Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * entityData.wallCheckDistance));
                //Ledge Detector
                Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * Movement.FacingDirection * entityData.ledgeCheckDistance));
                //Detect Player to Melee Attack Detector
                Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * entityData.minAgroDistance), 0.2f);
                //Detect Player Detector
                Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * entityData.minAgroDistance), 0.2f);
                Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * entityData.maxAgroDistance), 0.2f);
            }
        }
        #endregion
    }
}