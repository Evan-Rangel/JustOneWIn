using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class State
    {
        #region References
        protected FiniteStateMachine stateMachine;
        protected Entity entity;
        protected Core core;
        #endregion

        #region Floats
        public float startTime { get; protected set; }
        #endregion

        #region Strings
        protected string animBoolName;
        #endregion

        #region Construct
        public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
        {
            //Note for my self, "this" is to identify the var with the same name
            this.entity = entity;
            this.stateMachine = stateMachine;
            this.animBoolName = animBoolName;
            core = entity.Core;
        }
        #endregion

        #region Virtual Functions
        //Virtual Enter
        public virtual void Enter()//"Virtual" means that this can be redefind in the derived classes
        {
            startTime = Time.time;//Every time this enter function gets called on whatever state it is it's going to store the star time and any other state reference this without having to set the start time in each one of our state's
            entity.animator.SetBool(animBoolName, true);//For our different State we don't have to care about setting the animation parameters true or false, we insted add more parameters if we need to, but the base parameter is going to be there, we just have to change the name and that will change in our animator
            DoChecks();

        }
        //Virtual Exit
        public virtual void Exit()//"Virtual" means that this can be redefind in the derived classes
        {
            entity.animator.SetBool(animBoolName, false);//Stops the animation
        }
        //Virtual LogicUpdate
        public virtual void LogicUpdate()//"Virtual" means that this can be redefind in the derived classes
        {

        }
        //Virtual PhysicsUpdate
        public virtual void PhysicsUpdate()//"Virtual" means that this can be redefind in the derived classes
        {
            DoChecks();
        }
        //Virtual DoChecks
        public virtual void DoChecks()
        {

        }
        #endregion
    }
}
