using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarab_StunState : StunState
{
    //Data reference
    private Scarab scarab;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public Scarab_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Scarab scarab) : base(entity, stateMachine, animBoolName, stateData)
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

        //Condition that check the stuntime to change the state
        if(isStunTimeOver)
        {
            if(performCloseRangeAction)
            {
                stateMachine.ChangeState(scarab.meleeAttackState);
            }
            else if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(scarab.chargeState);
            }
            else
            {
                scarab.lookForPlayerState.SetTurnImmediately(true);
                stateMachine.ChangeState(scarab.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }
    //-------END OVERRIDES-------//
}
