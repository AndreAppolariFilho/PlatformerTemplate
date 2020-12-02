using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerInStairState StairState { get; private set; }
    public PlayerJumpInStairState JumpStairState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler;
    public Rigidbody2D RB { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public SpriteRenderer SpriteR { get; private set; }
    #endregion

    #region Check Transforms

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;

    #endregion

    #region Other Variables

    public Vector2 CurrentVelocity;
    public int FacingDirection { get; private set; }    

    private Vector2 workspace;
    #endregion
    public GameObject InteractPanel;
    public bool checkTouchingLadder = false;
    public bool canDisplayInteractPanel = true;
    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "run");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "jump_up");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "jump_up");
        LandState = new PlayerLandState(this, StateMachine, playerData, "idle");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wall_slide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "ledge_climb");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "ledge_climb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "jump_up");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledge_grab");
        DashState = new PlayerDashState(this, StateMachine, playerData, "jump_up");
        StairState = new PlayerInStairState(this, StateMachine, playerData, "ladder_climb");
        JumpStairState = new PlayerJumpInStairState(this, StateMachine, playerData, "jump_up");
    }
    public void StopPlayer()
    { 
    }
    private void Start()
    {
        Anim = GetComponent<Animator>();
        //InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        SpriteR = GetComponent<SpriteRenderer>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");

        FacingDirection = 1;

        StateMachine.Initialize(IdleState);
    }
    public void ActivatePanel()
    {
        InteractPanel.SetActive(true);
    }
    public void DeactivatePanel()
    {
        InteractPanel.SetActive(false);
    }
    private void Update()
    {
        CurrentVelocity = RB.velocity;
        if(GameObject.FindObjectOfType<GameManager>().currentState == GameManager.GameState.PlayerMode)
        { 
            checkTouchingLadder = CheckIfTouchingLadder();
            StateMachine.CurrentState.LogicUpdate();
            canDisplayInteractPanel = true;
        }
        else
        {
            canDisplayInteractPanel = false;
            Anim.Play("idle");
            //StateMachine.CurrentState.LogicUpdate();
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions

    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    #endregion

    #region Check Functions

    public bool CheckIfTouchingLadder()
    {
        //return Physics2D.Raycast(wallCheck.position, Vector2.up, playerData.wallCheckDistance, playerData.whatIsLadder);
        return Physics2D.OverlapCircle(wallCheck.position, playerData.groundCheckRadius, playerData.whatIsLadder);
    }
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    public bool CheckIfFeetTouchingLadder()
    {
        bool touchingPlatform = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("LadderPlatform"))
            {
                touchingPlatform = true;
                break;
            }
        }
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsLadder) && touchingPlatform;
    }
    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public List<Collider2D> GetWallsInClimb()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        colliders.AddRange(Physics2D.OverlapCircleAll(ledgeCheck.position, playerData.groundCheckRadius, playerData.whatIsGround));
        colliders.AddRange(Physics2D.OverlapCircleAll(ledgeCheck.position, playerData.groundCheckRadius, playerData.whatIsGround));
        return colliders;
    }
    public GameObject GetGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
        foreach(Collider2D collider in colliders)
        {
            if(collider.gameObject.CompareTag("LadderPlatform"))
            {
                return collider.gameObject;
            }
        }
        return null;
    }
    public GameObject GetLadder()
    {
        return Physics2D.OverlapCircle(wallCheck.position, playerData.groundCheckRadius, playerData.whatIsLadder).gameObject;
    }
    public GameObject GetLadderFeet()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsLadder).gameObject;
    }
    public Vector2 GetLadderPosition()
    {   
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheck.position, playerData.groundCheckRadius, playerData.whatIsLadder);
        foreach(Collider2D collider in colliders)
        {
            return new Vector2(collider.bounds.center.x, collider.bounds.center.y);
        }
        return new Vector2(0, 0);
    }
    public Vector2 GetLadderFeetPosition()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsLadder);
        foreach (Collider2D collider in colliders)
        {
            return new Vector2(collider.bounds.center.x, collider.bounds.center.y);
        }
        return new Vector2(0, 0);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    #endregion

    #region Other Functions

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        workspace.Set(xDist * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.whatIsGround);
        float yDist = yHit.distance;

        workspace.Set(wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);
        return workspace;
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public void DisableGravity()
    {
        RB.gravityScale = 0;
    }
    public void EnableGravity()
    {
        RB.gravityScale = playerData.gravityScale;
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wallCheck.position, playerData.groundCheckRadius);
    }
    private void Flip()
    {
        FacingDirection *= -1;
        //transform.Rotate(0.0f, 180.0f, 0.0f);
        SpriteR.flipX = !SpriteR.flipX;
    }
    #endregion
}
