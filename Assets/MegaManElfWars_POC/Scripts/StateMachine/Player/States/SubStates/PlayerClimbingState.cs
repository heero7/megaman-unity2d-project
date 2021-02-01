﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : PlayerMovementState
{
    private bool climbingEventEnd; // Perfect place to practice events
    private FinishedClimbingEventPosition endingPosition;

    public PlayerClimbingState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
        climbingEventEnd = false;
        LadderController.FinishedClimbingEvent += StoppedClimbing;
    }

    public override void OnExecute()
    {
        base.OnExecute();
        player.SetVelocityY(verticalInput * player.MovementData.ladderClimbSpeed);
        if (player.CurrentVelocity.y != 0.0f) player.Animator.speed = 1; // Do an additional check. If we're going down, play it backwards.
        else player.Animator.speed = 0;

        if (jumpInputPressed)
        {
            movementStateMachine.ChangeState(player.Falling);
            player.Animator.SetBool("Falling", true); // Force the animation.
            player.TriggerBodyCollider(true);
            // TODO: Might need to trigger the colliders early.
            // Wait until the main body part is not touching the 
            // ground.
            return;
        }
        else if (climbingEventEnd)
        {
            movementStateMachine.ChangeState(player.Idle);
            if (endingPosition == FinishedClimbingEventPosition.Top) player.MoveToTopOfLadder();
            player.TriggerLadderColliders(true);
            return;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        player.TriggerBodyCollider(true);
        player.TriggerLadderColliders(false);
        player.Animator.speed = 1;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        climbingEventEnd = false;

        // Turn off stuff 
        // Turn off the main collider
        player.TriggerBodyCollider(false);
        player.TriggerLadderColliders(true);
    }

    public void StoppedClimbing(FinishedClimbingEventPosition ladderPos)
    {
        Debug.Log($"Stopped climbing ladder event. Player reached the {ladderPos}");
        climbingEventEnd = true;
    }
}
