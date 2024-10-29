using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class PlayerStateMachine
    {
        //---Player States Machine Vars---//
        //-Getters and Setters-//
        public PlayerState CurrentState { get; private set; }

        //---States Machine Controlls Functions---//
        public void Initialize(PlayerState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
        //---End of State Machine Controlls Functions---//
    }
}