using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    //Data reference
    protected D_MoveState stateData;

    //Detectors flags
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    //---With override, we can reride the function on the father script with out changing the base funtion (yo can override function with the "Virtual")---//
    //-------OVERRIDES-------//
    public override void Enter()
    { 
        base.Enter();//"base" means that when this enter funtion gets called it's going to also call the enter funtion in our base class, which is "State", So if we want to change to completely the functionality of a function we can just remove this based on enter and the code inside of our "State" base won't get call.
        //Entity Movement
        entity.SetVelocity(stateData.movementSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void DoChecks()
    {
        base.DoChecks();

        //Equal to same function
        isDetectingWall = entity.CheckWall();
        isDetectingLedge = entity.CheckLedge();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }
    //-------END OVERRIDES-------//
}
