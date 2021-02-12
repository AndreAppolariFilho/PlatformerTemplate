using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlopeWalk : PlayerState
{
    public PlayerSlopeWalk(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Entered SlopeWalk");
    }

    public override void Exit()
    {
        base.Exit();
        //player.CurrentVelocity.y = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.CheckIfShouldFlip(player.InputHandler.NormInputX);

        

        //if(!player.isOnSlope)
        //{
        //    stateMachine.ChangeState(player.MoveState);
        //}

        //if(player.InputHandler.NormInputX == 0 && player.CheckIfGrounded())
        //{
        //    stateMachine.ChangeState(player.IdleState);
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.MoveOnSlope(playerData.movementVelocity, player.InputHandler.NormInputX);
    }
}

