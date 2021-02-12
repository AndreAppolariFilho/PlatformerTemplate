using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInStairState : PlayerState
{
    
    private bool jumpInput;

    private int xInput;
    private int yInput;
    private bool isMoving;
    private bool isLeft = true;
    private bool hasColliderInFeet = false;
    private PlatformEffector2D colliderFeetEffector;
    private Vector2 ladderPosition;
    private float timeToSwitch = 0.2f;
    private float currentTime = 0;
    private HashSet<Collider2D> toIgnore = new HashSet<Collider2D>();
    public PlayerInStairState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        
        //player.Anim.SetBool("climbLedge", false);
        isMoving = false;
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        //isMoving = true;
    }
    public void SetColliderFeetEffector(PlatformEffector2D effector)
    {
        colliderFeetEffector = effector;
        hasColliderInFeet = true;
    }
    public void SetLadderPosition(Vector2 position)
    {
        ladderPosition = new Vector2(position.x, player.transform.position.y);
        player.transform.position = ladderPosition;
    }
    public override void LogicUpdate()
    {
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        if(player.dying)
        {
            stateMachine.ChangeState(player.DyingState);
            return;
        }
        if(yInput > 0f)
        {
            player.SetVelocityY(playerData.ladderGrimbVelocity);
            isMoving = true;
            if(currentTime > timeToSwitch)
            { 
                isLeft = !isLeft;
                currentTime = 0;
            }

        }
        else if(yInput < 0)
        {
            player.SetVelocityY(-1* playerData.ladderGrimbVelocity);
            isMoving = true;
            if (currentTime > timeToSwitch)
            {
                isLeft = !isLeft;
                currentTime = 0;
            }

        }
        else
        {
            player.SetVelocityZero();
            
        }
        if(!player.CheckIfTouchingLadder() )
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if(jumpInput)
        {
            if(yInput < 0)
            {
                player.SetVelocityY(0);
                stateMachine.ChangeState(player.InAirState);
            }
            else { 
                stateMachine.ChangeState(player.JumpStairState);
            }
        }
        else if(player.CheckIfGrounded() && yInput < 0 && player.GetLadderFeet().CompareTag("NormalLadder"))
        {
            stateMachine.ChangeState(player.IdleState);
        }
        if(isLeft)
        {
            player.Anim.Play("ladder_climb_left");
        }
        else
        {
            player.Anim.Play("ladder_climb_right");
        }
        currentTime += Time.deltaTime;
        toIgnore.UnionWith(player.GetWallsInClimb());
        //foreach(Collider2D collider in toIgnore)
        //{
        //    collider.isTrigger = true;
        //}
        
    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocityZero();
        player.DisableGravity();
        
        this.player.SpriteR.sortingOrder = 1;
        
            
    }
    public override void Exit()
    {
        base.Exit();
        player.EnableGravity();
        player.transform.position = new Vector2(ladderPosition.x, player.transform.position.y);
        this.player.SpriteR.sortingOrder = 0;
        if(hasColliderInFeet)
        {
            colliderFeetEffector.rotationalOffset = 0;
            colliderFeetEffector = null;
            hasColliderInFeet = false;
        }
        //foreach (Collider2D collider in toIgnore)
        //{
        //    collider.isTrigger = false;
        //}
    }
 }
