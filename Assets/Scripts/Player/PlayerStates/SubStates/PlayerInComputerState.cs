using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInComputerState : PlayerState
{
    public PlayerInComputerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocityZero();
        player.transform.position = player.computerSpawnPosition.position;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        player.transform.position = player.computerSpawnPosition.position;
        if(GameObject.FindObjectOfType<GameManager>().currentState == GameManager.GameState.PlayerMode)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
