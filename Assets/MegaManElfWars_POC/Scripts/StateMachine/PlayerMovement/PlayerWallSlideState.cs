using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerMovementState
{
    public PlayerWallSlideState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void ApplyGravity()
    {
        base.ApplyGravity();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExecute()
    {
        base.OnExecute();
        player.SetVelocityY(-player.MovementData.wallSlideSpeed);

        if (player.IsGrounded() && !exitingState)
        {
            movementStateMachine.ChangeState(player.Idle);
            return;
        }

        if (jumpInputPressed && !exitingState)
        {
            player.InputController.UseJumpInput();
            movementStateMachine.ChangeState(player.WallClimb);
            return;
        }

        if (horizontalInput != player.FacingDirection)
        {
            movementStateMachine.ChangeState(player.Falling);
            return;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override string ToString()
    {
        return "WallSlide";
    }
}
