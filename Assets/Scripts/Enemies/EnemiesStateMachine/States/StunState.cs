using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    //Data reference
    protected D_StunState stateData;

    //Flags
    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    //---With override, we can reride the function on the father script with out changing the base funtion (yo can override function with the "Virtual")---//
    //-------OVERRIDES-------//
    public override void Enter()
    {
        base.Enter();

        //Stun start off
        isStunTimeOver = false;
        isMovementStopped = false;
        //Set stun
        entity.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();

        //Stun reset
        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Condition that keep track of stuntime
        if(Time.time >= startTime + stateData.stunTime)
        {
            isStunTimeOver = true;      
        }
        //Condition that know when the enemy is grounded and the time of knockback ends to allow another knockback
        if(isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
        {
            isMovementStopped = true;
            entity.SetVelocity(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //Ground Check
        isGrounded = entity.CheckGround();
        //Player Check
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }
    //-------END OVERRIDES-------//

    //-------OTHER FUNCTIONS-------//

    //-------END OTHERS FUNCTIONS-------//
}
