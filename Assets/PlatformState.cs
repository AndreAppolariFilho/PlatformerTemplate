using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformState
{
    protected ProcesserInput player;
    protected PlatformStateMachine stateMachine;
    

    protected string animBoolName;
    public PlatformState(ProcesserInput player, PlatformStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        
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
