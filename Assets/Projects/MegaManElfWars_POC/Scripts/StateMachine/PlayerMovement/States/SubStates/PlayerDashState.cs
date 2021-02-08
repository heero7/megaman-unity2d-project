using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerMovementState
{
    private float startTime;
    public PlayerDashState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();
        player.SetVelocityX(player.MovementData.dashSpeed * player.FacingDirection);
        AfterImagePool.Instance.RetrieveAfterImageFromPool();

        var isPastDashTime = Time.time > startTime + player.MovementData.dashDuration;
        if (isPastDashTime || horizontalInput == player.FacingDirection * -1 || !player.InputController.DashHeld || player.IsTouchingWall(player.FacingDirection))
        {
            if (player.IsGrounded())
            {
                movementStateMachine.ChangeState(player.Idle);
            }
            else
            {
                movementStateMachine.ChangeState(player.Falling);
            }
            return;
        }

        // Check to see if we're far enough to place an after image.
        // var imageDist = Mathf.Abs(player.transform.position.x - player._lastImageXpos);
        // if (imageDist > player.distanceBetweenAfterImages)
        // {
        //     AfterImagePool.Instance.RetrieveAfterImageFromPool();
        //     player._lastImageXpos = player.transform.position.x;
        // }

        if (player.InputController.JumpPressed && player.IsGrounded())
        {
            player.InputController.UseJumpInput();
            movementStateMachine.ChangeState(player.Rising);
            return;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        // Lock on the Y axis
        player.SetVelocityY(0);
        startTime = Time.time;
    }

    public override string ToString()
    {
        return "Dash";
    }
}
