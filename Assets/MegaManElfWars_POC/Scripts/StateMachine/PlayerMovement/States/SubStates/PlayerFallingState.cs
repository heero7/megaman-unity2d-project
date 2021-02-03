using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerJumpState
{
    private bool isTouchingWallAndDirectionHeld;
    private int timesDashedWhileFalling;

    public PlayerFallingState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();
        isTouchingWallAndDirectionHeld = player.IsTouchingWall(horizontalInput);

        if (player.Rising.DashJumping)
        {
            player.SetVelocityX(player.CurrentVelocity.x * player.MovementData.DashJumpAerialSpeed);
        }

        if (player.IsGrounded())
        {
            timesDashedWhileFalling = 0;
            movementStateMachine.ChangeState(player.Idle);
            return;
        }

        if (isTouchingWallAndDirectionHeld)
        {
            movementStateMachine.ChangeState(player.WallSlide);
            return;
        }

        if (player.InputController.DashPressed && timesDashedWhileFalling == 0 && !player.Rising.DashJumping)
        {
            // Maybe check if we were Wall jumping previously to prevent this from happening.
            // But only cehck if we 
            timesDashedWhileFalling++;
            player.InputController.UseDashInput();
            movementStateMachine.ChangeState(player.Dash);
            return;
        }
    }
}
