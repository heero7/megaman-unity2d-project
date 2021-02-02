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

        var isPastDashTime = Time.time > startTime + player.MovementData.dashDuration;
        if (isPastDashTime)
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

        //var pastDashTime = Time.time > StartTime + characterData.DashDuration;
        // if (horizontalInput == player.FacingDirection * -1 || !dashReleased || pastDashTime || player.IsTouchingWall(player.FacingDirection)) // This is the opposite of where we're going.
        // {
        //     //if (pastDashTime) player.Anim.SetBool("DashingFullGrounded", true);
        //     // TODO: Maybe add something similar to landing state here.

        //     // Check where to go.

        //     if (isGrounded) stateMachine.ChangeState(stateMachine.Idle);
        //     else stateMachine.ChangeState(stateMachine.Falling);
        //     return;
        // }

        // if (jumpPressed && isGrounded)
        // {
        //     player.InputHandler.JumpPressedExecuted();
        //     stateMachine.ChangeState(stateMachine.Rising);
        // }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        startTime = Time.time;
    }
}
