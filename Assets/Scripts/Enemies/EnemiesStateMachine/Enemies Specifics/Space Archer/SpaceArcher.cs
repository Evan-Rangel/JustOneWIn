using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceArcher : Entity
{
    //Defind States of the Enemy
    public Archer_IdleState idleState { get; private set; }
    public Archer_MoveState moveState { get; private set; }
    public Archer_PlayerDetectedState playerDetectedState { get; private set; }
    //public Scarab_ChargeState chargeState { get; private set; }
    public Archer_LookForPlayerState lookForPlayerState { get; private set; }
    public Archer_MeleeAttackState meleeAttackState { get; private set; }
    public Archer_StunState stunState { get; private set; }
    public Archer_DeadState deadState { get; private set; }
    public Archer_DodgeState dodgeState { get; private set; }
    public Archer_RangeAttackState rangeAttackState { get; private set; }

    //States that the Enemy will have
    [Header("Enemy States")]
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;
    //[SerializeField]
    //private D_ChargeState chargeStateData;
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

    //Transform Values
    [Header("Enemy Melee Attack Position")]
    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangeAttackPosition;

    //-------OVERRIDES-------//
    public override void Start()
    {
        base.Start();

        idleState = new Archer_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Archer_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new Archer_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        //chargeState = new Scarab_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Archer_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new Archer_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new Archer_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Archer_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new Archer_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangeAttackState = new Archer_RangeAttackState(this, stateMachine, "rangeAttack", rangeAttackPosition, rangeAttackStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);
        if(isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if(isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        else if (CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(lookForPlayerState);
        }
        else if(!CheckPlayerInMinAgroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }
    //-------END OVERRIDES-------//

    //-------OTHER FUNCTIONS-------//
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //Zone will enemy damage
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
    //-------END OTHERS FUNCTIONS-------//
}
