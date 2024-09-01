using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    //Data reference
    protected D_ChargeState stateData;

    //Flags
    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    //---With override, we can reride the function on the father script with out changing the base funtion (yo can override function with the "Virtual")---//
    //-------OVERRIDES-------//
    public override void Enter()
    {
        base.Enter();

        isChargeTimeOver = false;

        //Set velocity of the charge
        entity.SetVelocity(stateData.chargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Condition thats if the time while the scarab is charging then stop that
        if(Time.time >= startTime + stateData.chargeTime)
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //Check Range
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();    
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //Check Range
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();

        //Check for a wall or ledge to stop the charge when reach one of those
        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();

        //Check for Attack
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }
    //-------END OVERRIDES-------//

    //-------OTHER FUNCTIONS-------//

    //-------END OTHERS FUNCTIONS-------//
}
