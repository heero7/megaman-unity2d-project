using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : PlayerMovementState
{
    public bool climbingEventEnd; // Perfect place to practice events
    public PlayerClimbingState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();
        player.SetVelocityY(verticalInput * player.MovementData.ladderClimbSpeed);
        if (player.CurrentVelocity.y > 0.01f) player.Animator.speed = 0;
        else player.Animator.speed = 0;
    }

    public override void OnExit()
    {
        base.OnExit();
        player.TriggerBodyCollider(true);
        player.SetGravityScale(1);
        player.TriggerLadderColliders(false);

    }

    public override void OnEnter()
    {
        base.OnEnter();

        // Turn off stuff 
        // Turn off the main collider
        player.TriggerBodyCollider(false);
        player.SetGravityScale(0);
        player.TriggerLadderColliders(true);
    }
}
