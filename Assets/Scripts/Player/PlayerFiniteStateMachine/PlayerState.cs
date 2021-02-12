using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    protected string animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Anim.Play(animBoolName);
        startTime = Time.time;
        //Debug.Log(this.GetType().Name+" "+animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
        Debug.Log("ENTERING   "+this.GetType());
    }

    public virtual void Exit()
    {
        //player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
        if(player.InputHandler.PausedInput)
        {
            GameObject.FindObjectOfType<GameManager>().PauseGame();
        }

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
        player.SlopeCheck(player.InputHandler.NormInputX);
    }

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
    

}
