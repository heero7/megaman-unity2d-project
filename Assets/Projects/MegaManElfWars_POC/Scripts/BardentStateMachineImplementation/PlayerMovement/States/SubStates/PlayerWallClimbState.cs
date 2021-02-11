using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerMovementState
{
    private float startTime;
    public PlayerWallClimbState(PlayerController player, BardentStateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void ApplyGravity() => player.Rigidbody2D.AddForce(Vector2.down * player.Gravity);

    public override void OnEnter()
    {
        startTime = Time.time;
        base.OnEnter();
        player.SetVelocity(player.MovementData.wallJumpVelocity, player.MovementData.WallJumpAngle, player.FacingDirection * -1);
    }

    public override void OnExecute()
    {
        base.OnExecute();
        if (Time.time > startTime + player.MovementData.wallClimbNoControlTimer && !exitingState)
        {
            player.SetVelocityX(0);
            // TODO: If there's a future bug reported, here's where double jump isn't getting registered.
            movementStateMachine.ChangeState(player.Falling);
            return;
        }
    }

    public override void OnExit()
    {
        startTime = 0;
        base.OnExit();
    }
}
