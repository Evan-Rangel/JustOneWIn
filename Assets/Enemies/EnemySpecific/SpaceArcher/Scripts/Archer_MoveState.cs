using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_MoveState : MoveState
{
    //Data reference
    private SpaceArcher archer;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public Archer_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, SpaceArcher archer) : base(entity, stateMachine, animBoolName, stateData)
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

        //Condition that if is in the range, transition to player detected state
        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(archer.playerDetectedState);
        }
        //Condition thats if not detecting a wall or lwdge then go to Idle State
        else if (isDetectingWall || !isDetectingLedge)
        {
            archer.idleState.SetFlipAfterIdle(true);
            //Transition to Idle
            stateMachine.ChangeState(archer.idleState);
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
