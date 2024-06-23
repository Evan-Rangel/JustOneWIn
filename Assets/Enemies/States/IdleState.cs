using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    //Data reference
    protected D_IdleState stateData;

    //Flag
    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isPlayerInMinAgroRange;

    //Idle Time
    protected float idleTime;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    //---With override, we can reride the function on the father script with out changing the base funtion (yo can override function with the "Virtual")---//
    //-------OVERRIDES-------//
    public override void Enter()
    {
        base.Enter();

        //Stop Moving
        entity.SetVelocity(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();


    }

    public override void Exit()
    {
        base.Exit();

        //Condition taht wheb ends, then can flip the character
        if (flipAfterIdle)
        {
            entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Conditions thats if the time pass the idle time then the idle is over
        if(Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //Check Range
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }
    //-------END OVERRIDES-------//

    //-------OTHER FUNCTIONS-------//
    //Function SetFlipAfterIdle
    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    //Funtion SetRandomIdleTime
    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
    //-------END OTHERS FUNCTIONS-------//
}
