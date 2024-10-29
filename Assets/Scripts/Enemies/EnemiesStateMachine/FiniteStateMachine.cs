using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class FiniteStateMachine
    {
        //States
        public State currentState { get; private set; }

        //Function Initialize
        public void Initialize(State startingState)//This will take the fisrt state when the game starts
        {
            currentState = startingState;
            currentState.Enter();
        }

        //Funtion ChangeState
        public void ChangeState(State newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }

    }
}