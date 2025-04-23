using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using UnityEngine;

public class SpaceArcher : Entity
{
    #region States
    public Archer_IdleState idleState { get; private set; }
    public Archer_MoveState moveState { get; private set; }
    public Archer_PlayerDetectedState playerDetectedState { get; private set; }
    public Archer_LookForPlayerState lookForPlayerState { get; private set; }
    public Archer_MeleeAttackState meleeAttackState { get; private set; }
    public Archer_StunState stunState { get; private set; }
    public Archer_DeadState deadState { get; private set; }
    public Archer_DodgeState dodgeState { get; private set; }
    public Archer_RangeAttackState rangeAttackState { get; private set; }
    #endregion

    #region References
    [Header("Enemy States")]
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    public D_DodgeState dodgeStateData;//Hay que ver como cambiar esto
    [SerializeField]
    public D_RangeAttackState rangeAttackStateData;
    #endregion

    #region Transform
    [Header("Enemy Melee Attack Position")]
    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangeAttackPosition;
    #endregion

    #region Override Functions
    public override void Awake()
    {
        base.Awake();

        idleState = new Archer_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Archer_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new Archer_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        lookForPlayerState = new Archer_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new Archer_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new Archer_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Archer_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new Archer_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangeAttackState = new Archer_RangeAttackState(this, stateMachine, "rangeAttack", rangeAttackPosition, rangeAttackStateData, this);

        stats.Poise.OnCurrentValueZero += HandlePoiseZero;
    }

    private void HandlePoiseZero()
    {
        stateMachine.ChangeState(stunState);
    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
    }

    private void OnDestroy()
    {
        stats.Poise.OnCurrentValueZero -= HandlePoiseZero;
    }
    //-------END OVERRIDES-------//
    #endregion

    #region Other Functions
    //-------OTHER FUNCTIONS-------//
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //Zone will enemy damage
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
    //-------END OTHERS FUNCTIONS-------//
    #endregion
}
