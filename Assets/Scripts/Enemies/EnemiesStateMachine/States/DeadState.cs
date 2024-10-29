using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class DeadState : State
    {
        //Data reference
        protected D_DeadState stateData;

        //Flags

        //Constructor
        //---This means is it's going to pass the entity state machine and animation variables that we get when we call this contructor on to our base clase which is "State" so now if we want to add anything else to the construcot, we can go ahead and do that.---//
        public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(entity, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }

        //---With override, we can reride the function on the father script with out changing the base funtion (yo can override function with the "Virtual")---//
        //-------OVERRIDES-------//
        public override void Enter()
        {
            base.Enter();

            //Instantiate the particles
            GameObject.Instantiate(stateData.deathbloodParticle, entity.transform.position, stateData.deathbloodParticle.transform.rotation);
            GameObject.Instantiate(stateData.deathChunckParticle, entity.transform.position, stateData.deathChunckParticle.transform.rotation);

            entity.gameObject.SetActive(false);
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
        }
        //-------END OVERRIDES-------//

        //-------OTHER FUNCTIONS-------//

        //-------END OTHERS FUNCTIONS-------//
    }
}