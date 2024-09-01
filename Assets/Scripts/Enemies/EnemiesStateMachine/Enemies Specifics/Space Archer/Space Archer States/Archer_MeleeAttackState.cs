using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_MeleeAttackState : MeleeAttackState
{
    //Data reference
    private SpaceArcher archer;

    //Constructor
    //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
    public Archer_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, SpaceArcher archer) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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

        //Condition that indicate when the attack is done by the animation ends, then transition to other state
        if (isAnimationFinished)
        {
            //Condition that detected if player is enough close to do the action
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(archer.playerDetectedState);
            }
            else if(!isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(archer.lookForPlayerState);
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

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }
    //-------END OVERRIDES-------//
}
