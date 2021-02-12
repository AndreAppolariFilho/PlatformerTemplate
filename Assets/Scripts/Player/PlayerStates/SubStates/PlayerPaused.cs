using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerPaused : PlayerState
{
    public PlayerPaused(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocityZero();
        player.transform.position = player.computerSpawnPosition.position;
    }
    public override void LogicUpdate()
    {
        if(GameObject.FindObjectOfType<GameManager>().currentState == GameManager.GameState.PlayerMode)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}