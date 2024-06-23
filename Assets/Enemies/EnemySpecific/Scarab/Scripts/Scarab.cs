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

    //-------OVERRIDES-------//
    public override void Start()
    {
        base.Start();

        idleState = new Scarab_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Scarab_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new Scarab_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        chargeState = new Scarab_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Scarab_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);

        stateMachine.Initialize(moveState);
    }
    //-------END OVERRIDES-------//
}
