using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerMovementState
{
    private bool isGrounded;
    public bool ClimbingEventEnd { get; set; }

    public PlayerGroundState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void ApplyGravity() => player.Rigidbody2D.AddForce(Vector2.down * player.Gravity);
    

    public override void OnExecute()
    {
        base.OnExecute();
        horizontalInput = player.InputController.NormalizedInputX;
        isGrounded = player.IsGrounded();

        if (jumpInputPressed && isGrounded)
        {
            player.InputController.UseJumpInput();
            movementStateMachine.ChangeState(player.Rising);
            return;
        }
        else if (!isGrounded && !ClimbingEventEnd)
        {
            Debug.DebugBreak();
            Debug.Log($"Previous State: {movementStateMachine.Previous}");
            movementStateMachine.ChangeState(player.Falling);
            return;
        }
        else if (player.InputController.DashPressed)
        {
            player.InputController.UseDashInput();
            movementStateMachine.ChangeState(player.Dash);
            return;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetVelocityX(0);
    }

    public override void OnExit()
    {
        ClimbingEventEnd = false;
        base.OnExit();
    }
}