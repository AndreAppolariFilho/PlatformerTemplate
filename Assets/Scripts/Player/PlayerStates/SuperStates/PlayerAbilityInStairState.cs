using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityInStairState : PlayerState
{
    protected bool isAbilityDone;

    private bool isGrounded;
    private int xInput;
    private int yInput;
    public PlayerAbilityInStairState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
        this.player.SpriteR.sortingOrder = 1;
    }

    public override void Exit()
    {
        base.Exit();
        this.player.SpriteR.sortingOrder = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        if(player.dying)
        {
            stateMachine.ChangeState(player.DyingState);
            return;
        }
        if (isAbilityDone)
        {
            if (isGrounded && player.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if(player.CurrentVelocity.y < 0.01f && yInput > 0 && player.CheckIfTouchingLadder())
            {
                stateMachine.ChangeState(player.StairState);
                player.StairState.SetLadderPosition(player.GetLadder().transform.position);
            }
            else if((player.CurrentVelocity.y < 0.01f && yInput < 0) || !player.CheckIfTouchingLadder())
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
