using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

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
    public PlayerDyingState DyingState { get; private set; }
    public PlayerRespawnState RespawnState { get; private set; }
    public PlayerInComputerState InComputerState { get; private set; }
    public PlayerSlopeWalk SlopeWalkState {get; private set; }
    public PlayerInCutscene InCutscene { get ; private set; }
    [SerializeField]
    public PlayerData playerData;
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
    public GameObject respawnPoint;
    public bool dying = false;
    public bool collidedWithWall = false;
    private Vector2 workspace;
    #endregion
    public GameObject InteractPanel;
    public bool checkTouchingLadder = false;
    public bool canDisplayInteractPanel = true;
    public bool checkTouchingWall = false;
    public bool checkTouchingLedge = false;
    public bool isOnSlope = false;
    public Vector2 capsuleColliderSize;
    public Vector2 slopeNormalPerp;
    public Vector3 m_Velocity;
    public Transform computerSpawnPosition;
    private PlayableDirector director;
    public float lastSlopeAngle;
    public float slopeSideAngle = 0; 
    public float angle = 0;
    public float slopeDownAngle;
    public bool isGrounded = false;
    [SerializeField]
    public PhysicsMaterial2D noFriction;
    [SerializeField]
    public PhysicsMaterial2D fullFriction;
    public CharacterController2D controller;
    #region Unity Callback Functions
    private void Awake()
    {
        lastSlopeAngle = 0;
        StateMachine = new PlayerStateMachine();
        director = GetComponent<PlayableDirector>();
        director.played += DirectorPlayed;
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "run");
        SlopeWalkState = new PlayerSlopeWalk(this, StateMachine, playerData, "run");
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
        DyingState = new PlayerDyingState(this, StateMachine, playerData, "death");
        RespawnState = new PlayerRespawnState(this, StateMachine, playerData, "respawn");
        InComputerState = new PlayerInComputerState(this, StateMachine, playerData, "in_computer");
        InCutscene = new PlayerInCutscene(this, StateMachine, playerData, "walk_to_door_1");
        capsuleColliderSize = GetComponent<CapsuleCollider2D>().size;
        controller = GetComponent<CharacterController2D>();
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
    public void InitCutscene()
    {
        director.Play();
    }
    public void DirectorPlayed(PlayableDirector obj)
    { 
        StateMachine.ChangeState(InCutscene);

    }
    public void DirectorStopped(PlayableDirector obj)
    {
        StateMachine.ChangeState(IdleState);
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
        checkTouchingLadder = CheckIfTouchingLadder();
        if(GameObject.FindObjectOfType<GameManager>().currentState != GameManager.GameState.PauseMode)
        {
            StateMachine.CurrentState.LogicUpdate();
        }
        NormalizeSlope ();
        if(GameObject.FindObjectOfType<GameManager>().currentState == GameManager.GameState.PlayerMode)
        {  
            canDisplayInteractPanel = true;
        }
        else
        {
            canDisplayInteractPanel = false;
            //Anim.Play("idle");
            SetVelocityZero();
            //StateMachine.CurrentState.LogicUpdate();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.gameObject.layer == LayerMask.NameToLayer("wall"))
        {
            collidedWithWall = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other) 
    {
        if(other.collider.gameObject.layer == LayerMask.NameToLayer("wall"))
        {
            collidedWithWall = false;
        }
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
        /*
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));
         RaycastHit2D hit = Physics2D.Raycast(checkPos, -transform.up, playerData.slopeCheckDistance, playerData.whatIsGround);
        if (hit.collider != null) { //if we hit something do stuff
            Debug.Log(hit.normal);
    
            angle = Mathf.Abs(Mathf.Atan2(hit.normal.x, hit.normal.y)*Mathf.Rad2Deg); //get angle
            if(angle > 0)
            {
                if(isGrounded)
                {
                    Vector2 position = new Vector2(this.transform.position.x * Mathf.Cos(angle * Mathf.Deg2Rad), this.transform.position.x * Mathf.Sin(angle * Mathf.Deg2Rad
                    ));
                    Debug.Log("position "+position);
                    //Debug.LogError(hit.point.y + " "+this.transform.position.y);
                    //Debug.LogError((SpriteR.bounds.center.y/(2*5)));
                    this.transform.position = new Vector2(this.transform.position.x, hit.point.y - (GetComponent<CapsuleCollider2D>().bounds.center.y/(2*5) ) -0.25f);
                    //this.transform.position = position;
                }

            }
        
        }
        */
        NormalizeSlope ();

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
    public void MoveOnSlope(float velocity, float xInput)
    {
        /*
        if (CheckIfGrounded() && !isOnSlope) //if not on slope
        {
            
            workspace.Set(velocity * xInput, 0.0f);
            RB.velocity = workspace;
        }
        else if (CheckIfGrounded() && isOnSlope) //If on slope
        {
            workspace.Set(velocity * slopeNormalPerp.x * -xInput, velocity * slopeNormalPerp.y * -xInput);
            RB.velocity = workspace;
        }
        //else if (!CheckIfGrounded()) //If in air
        //{
        //    workspace.Set(velocity * xInput, RB.velocity.y);
        //    RB.velocity = workspace;
        //}
        */
        //Debug.Log(xInput);
        //Debug.LogError(40*xInput*Time.deltaTime);
        
        CurrentVelocity.Set(100*xInput, 0);
        controller.Move(velocity*xInput, false, false);
    }
    public void SetVelocityXInAir(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityX(float velocity)
    {
        //if(velocity == 0)
        //{
        //    if(CheckIfGrounded())
        //    {
        //        if(angle > 0)
        //        {
        //            RB.constraints = RigidbodyConstraints2D.FreezeAll;
        //            RB.velocity = Vector2.zero; 
        //        }
        //    }
        //}
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));
        RaycastHit2D hit = Physics2D.Raycast(checkPos, -transform.up, 2.5f, playerData.whatIsGround);
        workspace.Set(velocity, CurrentVelocity.y);
        if (hit.collider != null) 
        {
            Debug.DrawLine(checkPos, hit.point, Color.red);
            Vector2 moveAlongGround = new Vector2 (hit.normal.y, -hit.normal.x);
            workspace = moveAlongGround * velocity;
        }
        
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetToComputerState(Transform computerPosition)
    {
        if(StateMachine.CurrentState != InComputerState)
        {
            computerSpawnPosition = computerPosition;
            this.transform.position = computerPosition.position;
            StateMachine.ChangeState(InComputerState);
        }
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
        CapsuleCollider2D boxCollider2d = GetComponent<CapsuleCollider2D>();
        //float extraHeightText = 0.5f;
        float extraHeightText = 1;

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, new Vector2(boxCollider2d.bounds.size.x -0.5f , boxCollider2d.bounds.size.y), 0f, Vector2.down, extraHeightText, playerData.whatIsGround);
        Color rayColor;
        if (raycastHit.collider != null) {
            rayColor = Color.green;
        } else {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);

        //Debug.Log(raycastHit.collider);
        isGrounded = raycastHit.collider != null;
        return raycastHit.collider != null;
        /*
        Vector2 min = GetComponent<CapsuleCollider2D>().bounds.min;
        Vector2 max = GetComponent<CapsuleCollider2D>().bounds.max;
        min = new Vector2(min.x, groundCheck.position.y + playerData.groundCheckRadius);
        max = new Vector2(max.x, groundCheck.position.y - playerData.groundCheckRadius);
        Vector2 dir = (max - min).normalized ;
        Vector2 perpDir = Vector3.Cross(dir, Vector3.right) ;
        Vector2 midPoint = (min + max) / 2f ;
        Vector2 offsetPoint = midPoint  ;
        isGrounded = Physics2D.BoxCast(offsetPoint, new Vector3(max.x - min.x,playerData.groundCheckRadius,playerData.groundCheckRadius * 2), playerData.groundCheckRadius, Vector2.down, playerData.whatIsGround) != null;
        return isGrounded;
        */
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
    void NormalizeSlope () {
        // Attempt vertical normalization
        float slopeFriction = 0;
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));
        if (isGrounded) {
            RaycastHit2D hit = Physics2D.Raycast(checkPos, -transform.up, playerData.slopeCheckDistance, playerData.whatIsGround);
            
            if (hit.collider != null && Mathf.Abs(hit.normal.x) > 0.1f) {
                Rigidbody2D body = GetComponent<Rigidbody2D>();
                // Apply the opposite force against the slope force 
                // You will need to provide your own slopeFriction to stabalize movement
                //body.velocity = new Vector2(body.velocity.x - (hit.normal.x * slopeFriction), body.velocity.y);
//
                ////Move Player up or down to compensate for the slope below them
                //Vector3 pos = transform.position;
                //pos.y += -hit.normal.x * Mathf.Abs(body.velocity.x) * Time.deltaTime * (body.velocity.x - hit.normal.x > 0 ? 1 : -1);
                //transform.position = pos;
                //Debug.Break();
               // this.transform.position = new Vector2(this.transform.position.x, hit.point.y - (GetComponent<CapsuleCollider2D>().bounds.center.y/(2) ));
            }
        }
    }
    public bool CheckIfTouchingWall()
    {
        checkTouchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        checkTouchingLedge = Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
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
    public void  Died() 
    {
        dying = true;
    }
    public void Respawned()
    {
        dying = false;
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
    public void SlopeCheck(float xInput)
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));
         RaycastHit2D hit = Physics2D.Raycast(checkPos, -Vector2.up); //cast downwards
        if (hit.collider != null) { //if we hit something do stuff
            //Debug.Log(hit.normal);
            
            slopeDownAngle = Mathf.Abs(Mathf.Atan2(hit.normal.x, hit.normal.y)*Mathf.Rad2Deg); //get angle
            //SlopeCheckHorizontal(checkPos);
            //SlopeCheckVertical(checkPos, xInput);
        }
        
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, playerData.slopeCheckDistance, playerData.whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, playerData.slopeCheckDistance, playerData.whatIsGround); 
        
        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos, float xInput)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, playerData.slopeCheckDistance, playerData.whatIsGround);
        
        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }                       

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        //if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        //{
        //    canWalkOnSlope = false;
        //}
        //else
        //{
        //    canWalkOnSlope = true;
        //}

        //if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
        //{
        //    rb.sharedMaterial = fullFriction;
        //}
        //else
        //{
        //    rb.sharedMaterial = noFriction;
        //}

        if (isOnSlope && xInput == 0.0f)
        {
            RB.sharedMaterial = fullFriction;
        }
        else
        {
            RB.sharedMaterial = noFriction;
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
        //Gizmos.DrawWireSphere(wallCheck.position, playerData.groundCheckRadius);
        //Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        Vector2 min = GetComponent<CapsuleCollider2D>().bounds.min;
        Vector2 max = GetComponent<CapsuleCollider2D>().bounds.max;
        min = new Vector2(min.x, groundCheck.position.y + playerData.groundCheckRadius);
        max = new Vector2(max.x, groundCheck.position.y - playerData.groundCheckRadius);
        Vector2 dir = (max - min).normalized ;
        Vector2 perpDir = Vector3.Cross(dir, Vector3.right) ;
        Vector2 midPoint = (min + max) / 2f ;
        Vector2 offsetPoint = midPoint  ;
        Gizmos.DrawWireCube(offsetPoint, new Vector3(max.x - min.x,playerData.groundCheckRadius,playerData.groundCheckRadius * 2));
    }
    private void Flip()
    {
        FacingDirection *= -1;
        //transform.Rotate(0.0f, 180.0f, 0.0f);
        SpriteR.flipX = !SpriteR.flipX;
    }
    #endregion
}
