using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerJumpState
{
    private bool isTouchingWallAndDirectionHeld;

    public PlayerFallingState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();
        isTouchingWallAndDirectionHeld = player.IsTouchingWall(horizontalInput);
        if (player.IsGrounded())
        {
            movementStateMachine.ChangeState(player.Idle);
            return;
        }

        if (isTouchingWallAndDirectionHeld)
        {
            movementStateMachine.ChangeState(player.WallSlide);
            return;
        }
    }
}
