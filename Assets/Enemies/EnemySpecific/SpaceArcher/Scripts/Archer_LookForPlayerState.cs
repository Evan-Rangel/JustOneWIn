using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_LookForPlayerState : LookForPlayerState
{
    //Data reference
    private SpaceArcher archer;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public Archer_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData, SpaceArcher archer) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.archer = archer;
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

        //Condition that know when we detect the player then chenge the state
        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(archer.playerDetectedState);
        }
        else if (isAllTurnsTimeDone)//When ends the time of look then move
        {
            stateMachine.ChangeState(archer.moveState);
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
