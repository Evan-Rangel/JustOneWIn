using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarab : Entity
{
    //Defind States of the Enemy
    public Scarab_IdleState idleState {  get; private set; }
    public Scarab_MoveState moveState { get; private set; }
    public Scarab_PlayerDetectedState playerDetectedState { get; private set; }
    public Scarab_ChargeState chargeState { get; private set; }
    public Scarab_LookForPlayerState lookForPlayerState { get; private set; }
    public Scarab_MeleeAttackState meleeAttackState { get; private set; }
    public Scarab_StunState stunState { get; private set; }
    public Scarab_DeadState deadState { get; private set; }

    //States that the Enemy will have
    [Header("Enemy States")]
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;

    //Transform Values
    [SerializeField]
    private Transform meleeAttackPosition;

    //-------OVERRIDES-------//
    public override void Start()
    {
        base.Start();

        idleState = new Scarab_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Scarab_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new Scarab_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        chargeState = new Scarab_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Scarab_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new Scarab_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new Scarab_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Scarab_DeadState(this, stateMachine, "dead",deadStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);
        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
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
