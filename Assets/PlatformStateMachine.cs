using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStateMachine 
{
    public PlatformState CurrentState { get; private set; }

    public void Initialize(PlatformState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlatformState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
