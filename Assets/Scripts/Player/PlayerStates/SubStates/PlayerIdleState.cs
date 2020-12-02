using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //Debug.Log(player.CheckIfTouchingLadder() + " " + Time.time);
        if(xInput != 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        if(yInput > 0 && player.CheckIfTouchingLadder())
        {
            stateMachine.ChangeState(player.StairState);
            player.StairState.SetLadderPosition(player.GetLadder().transform.position);
        }
        if(yInput < 0 && player.CheckIfFeetTouchingLadder() && player.GetLadderFeet().CompareTag("ExitLadder"))
        {
            stateMachine.ChangeState(player.StairState);
            PlatformEffector2D effector = player.GetGround().GetComponent<PlatformEffector2D>();
            effector.rotationalOffset = 180;
            player.StairState.SetColliderFeetEffector(effector);
            player.StairState.SetLadderPosition(player.GetLadderFeet().transform.position);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
