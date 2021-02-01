using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRadius = 2.5f;
    [SerializeField] private float slopeCheckDistance = 2.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PhysicsMaterial2D noFriction, fullFriction;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Vector2 currentMovement;
    private bool movementPressed;
    private float dir;
    private bool runPressed, isOnSlope;
    private Vector2 colliderSize, slopeNormalPerp;
    private float slopeDownwardAngle, slopeDownwardAngleOld, slopeSideAngle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        colliderSize = col.size;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        SlopeCheck();
    }

    private void MovePlayer()
    {
        var speedMult = runPressed ? 1.4f : 1f;


        if (IsGrounded && !isOnSlope)
        {
            rb.velocity = new Vector2(moveSpeed * currentMovement.x * speedMult, 0f);
        }
        else if (IsGrounded && isOnSlope)
        {
            rb.velocity = new Vector2(moveSpeed * slopeNormalPerp.x * -currentMovement.x, moveSpeed * slopeNormalPerp.y * -currentMovement.x);
        }
        else if (!IsGrounded)
        {
            rb.velocity = new Vector2(moveSpeed * currentMovement.x * speedMult, rb.velocity.y);
        }
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0f, colliderSize.y / 2);
        HorizontalSlopeCheck(checkPos);
        VerticalSlopeCheck(checkPos);
    }

    private void VerticalSlopeCheck(Vector2 checkPos)
    {
        var hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);
        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownwardAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownwardAngle != slopeDownwardAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownwardAngleOld = slopeDownwardAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if (isOnSlope && currentMovement.x == 0f) rb.sharedMaterial = fullFriction;
        else rb.sharedMaterial = noFriction;
    }

    private void HorizontalSlopeCheck(Vector2 checkPos)
    {
        var slopeHitF = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, groundLayer);
        var slopeHitB = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, groundLayer);

        if (slopeHitF)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitF.normal, Vector2.up);
        }
        else if (slopeHitB)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitB.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0f;
            isOnSlope = false;
        }
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        currentMovement = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && IsGrounded)
        {
            Debug.Log("Jump pushed this frame.");
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
        if (ctx.performed) Debug.Log("Jump button is being held now.");
        if (ctx.canceled) Debug.Log("Jump has button has been let go.");
    }

    public void OnRun(InputAction.CallbackContext ctx)
    {
        //TODO: Good example for pressing buttons in a single sense.
        runPressed = ctx.ReadValueAsButton();
    }

    private bool IsGrounded => Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayer) && rb.velocity.y <= 0.01f;
}
