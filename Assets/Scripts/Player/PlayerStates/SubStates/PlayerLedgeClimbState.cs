using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;

    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(player.IdleState);
        //Debug.Log(this.GetType().Name + " AnimationFinishTrigger");
        //player.Anim.SetBool("climbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();
        player.RB.gravityScale = 0;
        player.SetVelocityZero();
        player.transform.position = detectedPos;
        cornerPos = player.DetermineCornerPosition();

        startPos.Set(cornerPos.x - (player.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (player.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;
        isHanging = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.RB.gravityScale = 3;
        isHanging = false;

        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
        isAnimationFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(player.dying)
        {
            stateMachine.ChangeState(player.DyingState);
            return;
        }
        if(isClimbing)
        return;
        
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            player.SetVelocityZero();
            player.transform.position = startPos;
            
            
            if (xInput == player.FacingDirection && isHanging && !isClimbing)
            {
                isClimbing = true;
                //Debug.Break();
                //Debug.Log("ledge_wall");
                player.Anim.Play("ledge_wall");
                
            }
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if(jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
            else if((xInput == 0 || xInput != player.FacingDirection) && !isClimbing)
            {
                //Debug.Break();
                player.Anim.Play("ledge_grab");
            }
        }
      
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;
}
