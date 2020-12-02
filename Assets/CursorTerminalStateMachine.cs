using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTerminalStateMachine
{
    // Start is called before the first frame update
    public CursorTerminalState CurrentState { get; private set; }

    public void Initialize(CursorTerminalState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(CursorTerminalState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
