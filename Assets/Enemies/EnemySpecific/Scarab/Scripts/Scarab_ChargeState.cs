using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarab_ChargeState : ChargeState
{
    //Data reference
    private Scarab scarab;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public Scarab_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Scarab scarab) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.scarab = scarab;
    }

    //---With override, we can reride the function on the father script with out changing the base funtion (yo can override function with the "Virtual")---//
    //-------OVERRIDES-------//
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

        //Condition to avoid that the Charge State don't follow the action of stop to a wall or ledge
        if(!isDetectingLedge || isDetectingWall)
        {
            //LookForPlayer
            stateMachine.ChangeState(scarab.lookForPlayerState);
        }      
        else if (isChargeTimeOver)//Condition that if the time ends then check por the player and attack him
        {
            if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(scarab.playerDetectedState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    //-------END OVERRIDES-------//
}
