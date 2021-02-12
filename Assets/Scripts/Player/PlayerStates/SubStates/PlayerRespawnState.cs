using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnState : PlayerState
{
    public PlayerRespawnState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        stateMachine.ChangeState(player.IdleState);
        player.Respawned();
    }
    public override void LogicUpdate()
    {
        //Debug.LogError("Dying State");
    }
}