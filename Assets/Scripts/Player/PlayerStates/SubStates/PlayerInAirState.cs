using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    //Input
    private int xInput;
    private int yInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;

    //Checks
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;

    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool isJumping;

    private float startWallJumpCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();
        isTouchingLedge = player.CheckIfTouchingLedge();

        if(isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }

        if(!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.LogError("Bla");
        player.RB.constraints = RigidbodyConstraints2D.None;
        player.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;

        CheckJumpMultiplier();
        if(player.dying)
        {
            stateMachine.ChangeState(player.DyingState);
            return;
        }
        if(player.CurrentVelocity.y < 0)
        {
            if(player.CurrentVelocity.y < -5)
            {
                player.Anim.Play("jump_fall_1");
            }
            else
            {
                player.Anim.Play("jump_fall");
            }
            
        }
        
        if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {            
            stateMachine.ChangeState(player.LandState);
        }
        
        else if(isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if(jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            //StopWallJumpCoyoteTime();
            //isTouchingWall = player.CheckIfTouchingWall();
            //player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            //stateMachine.ChangeState(player.WallJumpState);
        }
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        //else if(isTouchingWall && player.CurrentVelocity.y <= 0)
        //{
        //    
        //    player.CurrentVelocity.x = 0;
        //    return;
        //    //stateMachine.ChangeState(player.WallSlideState);
        //}
        else if(dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        if (yInput > 0 && xInput == 0 && player.CheckIfTouchingLadder() && (player.CurrentVelocity.y > -2 || player.CurrentVelocity.y < -10))
        {
            stateMachine.ChangeState(player.StairState);
            player.StairState.SetLadderPosition(player.GetLadder().transform.position);
        }
        else
        {
            if(isTouchingWall)
            {
                if((xInput != 0 && xInput != player.FacingDirection) || xInput == 0)
                {
                    
                    player.CheckIfShouldFlip(xInput);
                    player.SetVelocityXInAir(playerData.velocityX * xInput);
                }
            }
            else
            {
                
                    player.CheckIfShouldFlip(xInput);
                    if(!player.collidedWithWall)
                    {
                        player.SetVelocityXInAir(playerData.velocityX * xInput);
                    }
                    
            }

            //player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            //player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        }

    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                
                player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (player.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }

        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckCoyoteTime()
    {
        if(coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if(wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;
}
