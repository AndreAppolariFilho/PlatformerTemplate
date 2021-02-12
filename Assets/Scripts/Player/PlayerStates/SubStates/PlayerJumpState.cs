using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int amountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseJumpInput();
        player.SetVelocityY(playerData.jumpVelocity);
        if(!player.CheckIfGrounded())
            isAbilityDone = true;
        amountOfJumpsLeft--;
        player.RB.constraints = RigidbodyConstraints2D.None;
        player.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.InAirState.SetIsJumping();
        
    }
    public override void DoChecks()
    {
        base.DoChecks();
        if(player.CurrentVelocity.y > 0)
            isAbilityDone = true;
        
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
            
            player.SetVelocityY(playerData.jumpVelocity);
        
    }

    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}
