using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarab_PlayerDetectedState : PlayerDetectedState
{
    //Data reference
    private Scarab scarab;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public Scarab_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, Scarab scarab) : base(entity, stateMachine, animBoolName, stateData)
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
        //Condition that detected if player is enough close to do the action
        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(scarab.meleeAttackState);
        }
        else if (performLongRangeAction) //Condition that if not a player detected, then trasition back to Idle
        {
            stateMachine.ChangeState(scarab.chargeState);
        }
        else if(!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(scarab.lookForPlayerState);
        }
        else if (!isDetectingLedge)
        {
            entity.Flip();
            stateMachine.ChangeState(scarab.moveState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    //-------END OVERRIDES-------//
}
