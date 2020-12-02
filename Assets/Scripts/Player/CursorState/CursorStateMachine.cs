using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStateMachine 
{
    
        public CursorState CurrentState { get; private set; }

        public void Initialize(CursorState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(CursorState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    
}
