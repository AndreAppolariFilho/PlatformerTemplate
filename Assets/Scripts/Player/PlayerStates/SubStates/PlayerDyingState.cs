using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDyingState : PlayerState
{
    public PlayerDyingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit() {
        base.Exit();

        player.transform.position = player.respawnPoint.gameObject.transform.position;
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Respawned();
    }
    public override void LogicUpdate()
    {
        //Debug.LogError("Dying State");
        if(!player.dying)
        {
            stateMachine.ChangeState(player.RespawnState);
        }
    }
}