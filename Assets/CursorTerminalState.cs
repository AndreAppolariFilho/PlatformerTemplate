using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTerminalState
{
    // Start is called before the first frame update
    protected CursorTerminal player;
    protected CursorTerminalStateMachine stateMachine;
    

    protected string animBoolName;
    public CursorTerminalState(CursorTerminal player, CursorTerminalStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;

        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }
    public virtual void LogicUpdate()
    {


    }
}
