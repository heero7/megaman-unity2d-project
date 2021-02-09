using RaycastPhysics;
using UnityEngine;
using UnityEngine.InputSystem;


public class RaycastPlayer : MonoBehaviour
{
    #region HP
    public FloatReference currentHP;
    public FloatReference maxHP;
    #endregion
    #region Jumping
    public FloatReference jumpHeight;
    public FloatReference timeToJumpApex;
    public float gravity;
    private float jumpVelocity;
    #endregion
    public bool snappyMovement;
    public FloatReference moveSpeed;
    private float horizontal, vertical;
    private Vector2 velocity;
    private float velocityXSmoothing;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .25f;
    private Controller2D controller;
    private bool jump;

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
    }

    public void OnJumpInput()
    {
        jump = true;
    }


    private void Awake()
    {
        currentHP.Value = maxHP.Value;
    }

    private void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    // This version.. is for without a state machine.
    // From Sebastians video, this will get very large and hectic.
    private void Update()
    {
        if (controller.CollisionInfo.Above || controller.CollisionInfo.Below)
        {
            velocity.y = 0;
        }

        if (Keyboard.current.jKey.wasPressedThisFrame && controller.CollisionInfo.Below)
        {
            //jump = false;
            velocity.y = jumpVelocity;
        }

        // TODO: You can probably do ice this way.
        // Then calculate if you were moving last frame, then apply some more horizontal force.
        var targetVelocityX = horizontal * moveSpeed.Value;
        if (!snappyMovement)
        {
            // Adds some smoothing in the X direction. This removes the snappiness.
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, controller.CollisionInfo.Below ? accelerationTimeGrounded : accelerationTimeGrounded);
        }
        else
        {
            velocity.x = targetVelocityX;
        }



        // Apply gravity. if you want.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        DamagePlayer();
    }

    private void DamagePlayer()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                currentHP.Value -= 10;
            }
        }
    }
}
