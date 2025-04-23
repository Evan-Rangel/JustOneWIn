using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using UnityEngine;

public class Scarab : Entity
{
    #region States
    public Scarab_IdleState idleState { get; private set; }
    public Scarab_MoveState moveState { get; private set; }
    public Scarab_PlayerDetectedState playerDetectedState { get; private set; }
    public Scarab_ChargeState chargeState { get; private set; }
    public Scarab_LookForPlayerState lookForPlayerState { get; private set; }
    public Scarab_MeleeAttackState meleeAttackState { get; private set; }
    public Scarab_StunState stunState { get; private set; }
    public Scarab_DeadState deadState { get; private set; }
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
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    #endregion

    #region Transform
    [SerializeField]
    private Transform meleeAttackPosition;
    #endregion

    #region Override Functions
    public override void Awake()
    {
        base.Awake();

        idleState = new Scarab_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Scarab_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new Scarab_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        chargeState = new Scarab_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Scarab_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new Scarab_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new Scarab_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Scarab_DeadState(this, stateMachine, "dead", deadStateData, this);

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