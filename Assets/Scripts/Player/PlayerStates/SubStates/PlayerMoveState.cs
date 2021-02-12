using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.CheckIfShouldFlip(xInput);
        player.RB.constraints = RigidbodyConstraints2D.None;
        player.RB.constraints = RigidbodyConstraints2D.FreezeRotation;

        player.SetVelocityX(playerData.movementVelocity * xInput);

        if(xInput == 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        //if(xInput != 0)
        //{
        //    if(player.isOnSlope)
        //    {
        //        stateMachine.ChangeState(player.SlopeWalkState);
        //    }
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        //player.SetVelocityX(playerData.movementVelocity * xInput);
    }
}
