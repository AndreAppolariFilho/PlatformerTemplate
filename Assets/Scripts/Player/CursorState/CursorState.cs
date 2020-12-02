using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorState
{
    // Start is called before the first frame update
    protected Cursor player;
    protected CursorStateMachine stateMachine;
    

    protected string animBoolName;
    public CursorState(Cursor player, CursorStateMachine stateMachine,  string animBoolName)
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
