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

        if (player.IsGrounded())
        {
            movementStateMachine.ChangeState(player.Idle);
            //return;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
