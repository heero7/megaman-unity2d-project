using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterMovementData movementData;
    public CharacterMovementData MovementData { get => movementData; }
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private float groundCheckDistance = .025f;
    [SerializeField] private float wallCheckDistance = .02f;
    [SerializeField] public float attackWait = .02f;

    #region StateMachines
    // Movement
    public StateMachine<PlayerMovementState> MovementStateMachine { get; private set; }
    public PlayerIdleState Idle { get; private set; }
    public PlayerRunState Run { get; private set; }
    public PlayerRisingState Rising { get; private set; }
    public PlayerFallingState Falling { get; private set; }
    public PlayerWallSlideState WallSlide { get; private set; }
    public PlayerDashState Dash { get; private set; }
    public PlayerClimbingState ClimbingLadder { get; private set; }
    // Action
    public StateMachine<PlayerActionState> ActionStateMachine { get; private set; } // TODO: a lot of power with this accesibility.
    public NoActionState NoAction { get; private set; }
    public MegamanRegularAttack Attack { get; private set; }
    #endregion

    #region Cached Variables
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public PlayerInputController InputController { get; private set; }
    private SpriteRenderer sRenderer;
    private BoxCollider2D bodyCollider;

    private BoxCollider2D upperLadderCollider;
    private BoxCollider2D lowerLadderCollider;
    #endregion

    #region Raycast Variables
    private Vector2 bottomEdgePoint;
    private Vector2 bottomLeftColiderPoint;
    private int verticalRayCount;
    private int horizontalRayCount;
    private float horizontalRaySpacing;
    private float verticalRaySpacing;
    private const float distanceBetweenRays = .15f;
    #endregion

    // TODO: Is there a way to calculate the Gravity from the level instead of from the player?
    public float Gravity { get; private set; }
    public float JumpForce { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    private Vector2 targetVelocity;
    private const float skinWidth = 0.015f;

    // Start is called before the first frame update
    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0; // We will handle gravity with scripting.
        bodyCollider = GetComponent<BoxCollider2D>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
        Animator = GetComponentInChildren<Animator>();
        upperLadderCollider = GameObject.FindGameObjectWithTag("upperLadder").GetComponent<BoxCollider2D>();
        lowerLadderCollider = GameObject.FindGameObjectWithTag("lowerLadder").GetComponent<BoxCollider2D>();
        TriggerLadderColliders(false); // Defaults to off.
        InputController = GetComponent<PlayerInputController>();
        Gravity = 2 * movementData.jumpHeight / Mathf.Pow(movementData.timeToJumpApex, 2); // TODO: Put this in the level object and have Level Manager read this value where needed.
        JumpForce = Gravity * movementData.timeToJumpApex;
        // TODO: Setup upper and lower colliders.
        MovementStateMachine = new StateMachine<PlayerMovementState>();
        ActionStateMachine = new StateMachine<PlayerActionState>();
    }

    void Start()
    {
        FacingDirection = 1;
        SetupRaySpacingValues();
        InitializeStateMachine();
        InitActionStateMachine();
        Attack.charge1 = GameObject.FindGameObjectWithTag("chargeFX1").GetComponent<Animator>();
        Attack.charge2 = GameObject.FindGameObjectWithTag("chargeFX2").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DebugDraw();
        CurrentVelocity = Rigidbody.velocity;
        MovementStateMachine.CurrentState.OnExecute();
        ActionStateMachine.CurrentState.OnExecute();
    }

    void FixedUpdate()
    {
        MovementStateMachine.CurrentState.ApplyGravity();
    }

    public void SetVelocityY(float y)
    {
        targetVelocity.Set(CurrentVelocity.x, y);
        SetVelocity(targetVelocity);
    }

    public void SetVelocityX(float x)
    {
        targetVelocity.Set(x, CurrentVelocity.y);
        SetVelocity(targetVelocity);
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        targetVelocity.Set(angle.x * velocity * direction, angle.y * velocity);
        SetVelocity(targetVelocity);
    }

    private void SetVelocity(Vector2 velocity)
    {
        Rigidbody.velocity = targetVelocity;
        CurrentVelocity = targetVelocity;
    }

    public bool IsGrounded()
    {
        if (!bodyCollider.enabled) return false;

        bottomLeftColiderPoint.Set(bodyCollider.bounds.min.x, bodyCollider.bounds.min.y);

        var rayOrigin = bottomLeftColiderPoint;
        for (var i = 0; i < verticalRayCount; i++)
        {
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);
            if (hit)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsTouchingWall(int horizontalInput)
    {
        var yStart = bodyCollider.bounds.min.y;
        var xStart = horizontalInput == 1 ? bodyCollider.bounds.max.x : bodyCollider.bounds.min.x;
        bottomEdgePoint.Set(xStart, yStart);
        for (var i = 0; i < horizontalRayCount; i++)
        {
            if (i == 0) continue;

            var rayOrigin = bottomEdgePoint;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * horizontalInput * 0.3f, wallCheckDistance, groundLayer);
            Debug.DrawRay(rayOrigin, Vector2.right * horizontalInput * wallCheckDistance, Color.green);
            if (hit.collider != null)
            {
                Debug.Log("We're touching a wall right now.");
                return true;
            }
        }
        return false;
    }

    public bool ClimbLadderRequest()
    {
        //TODO: Change ray cast distance. [up/down]
        var hitInfoUp = Physics2D.Raycast(transform.position, Vector2.up, .5f, ladderLayer);
        var hitInfoDown = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, ladderLayer);
        if (hitInfoUp.collider != null)
        {
            if (InputController.NormalizedInputY == 1)
            {
                return true;
            }
        }
        else if (hitInfoDown.collider != null)
        {
            if (InputController.NormalizedInputY == -1)
            {
                Debug.Log("Requesting to go down the ladder.");
                return true;
            }
        }
        return false;
    }

    public void CheckIfShouldFlip(int goingDirection)
    {
        if (goingDirection != 0 && goingDirection != FacingDirection) Flip();
    }


    public void MoveToTopOfLadder()
    {
        Debug.Log($"Player is moving to position: {upperLadderCollider.transform.position}");
        transform.position = Vector2.Lerp(transform.position, upperLadderCollider.transform.position, 2f);
        Rigidbody.velocity = Vector2.zero;
        SetVelocity(Vector2.zero);
    }

    public void TriggerBodyCollider(bool state) => bodyCollider.enabled = state;
    public void TriggerLadderColliders(bool state)
    {
        upperLadderCollider.enabled = state;
        lowerLadderCollider.enabled = state;
    }
    public void SetGravityScale(float value) => Rigidbody.gravityScale = value;

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    private void InitializeStateMachine()
    {
        Idle = new PlayerIdleState(this, MovementStateMachine, "Idle");
        Run = new PlayerRunState(this, MovementStateMachine, "Run");
        Rising = new PlayerRisingState(this, MovementStateMachine, "Rising");
        Falling = new PlayerFallingState(this, MovementStateMachine, "Falling");
        WallSlide = new PlayerWallSlideState(this, MovementStateMachine, "WallSlide");
        Dash = new PlayerDashState(this, MovementStateMachine, "Dash");
        ClimbingLadder = new PlayerClimbingState(this, MovementStateMachine, "ClimbLadder");
        MovementStateMachine.Init(Idle);
    }

    private void InitActionStateMachine()
    {
        NoAction = new NoActionState(this, ActionStateMachine, MovementStateMachine);
        Attack = new MegamanRegularAttack(this, ActionStateMachine, MovementStateMachine);
        ActionStateMachine.Init(NoAction);
    }

    private void SetupRaySpacingValues()
    {
        var bounds = bodyCollider.bounds;
        bounds.Expand(-skinWidth);
        var width = bounds.size.x;
        var height = bounds.size.y;
        verticalRayCount = Mathf.RoundToInt(width / .1f);
        verticalRaySpacing = width / (verticalRayCount - 1);
        horizontalRayCount = Mathf.RoundToInt(height / distanceBetweenRays);
        horizontalRaySpacing = height / (horizontalRayCount - 1);
    }

    private void DebugDraw()
    {
        // The ground.
        for (var i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(bottomLeftColiderPoint + Vector2.right * verticalRaySpacing * i, Vector2.up * groundCheckDistance, Color.red);
        }

        for (var i = 0; i < horizontalRayCount; i++)
        {
        }
    }
    void OnDrawGizmos()
    {
    }
}
