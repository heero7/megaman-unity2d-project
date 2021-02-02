using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerMovementState
{
    private bool isGrounded;
    public bool ClimbingEventEnd { get; set; }

    public PlayerGroundState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void ApplyGravity() => player.Rigidbody.AddForce(Vector2.down * player.Gravity);
    

    public override void OnExecute()
    {
        base.OnExecute();
        horizontalInput = player.InputController.NormalizedInputX;
        isGrounded = player.IsGrounded();

        if (jumpInputPressed && isGrounded)
        {
            movementStateMachine.ChangeState(player.Rising);
            return;
        }
        else if (!isGrounded && !ClimbingEventEnd)
        {
            movementStateMachine.ChangeState(player.Falling);
            return;
        }
        else if (player.InputController.DashPressed)
        {
            movementStateMachine.ChangeState(player.Dash);
            return;
        }
    }

    public override void OnExit()
    {
        ClimbingEventEnd = false;
        base.OnExit();
    }
}