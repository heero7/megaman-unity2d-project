using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRisingState : PlayerJumpState
{
    public PlayerRisingState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();
        if (player.CurrentVelocity.y <= 0f)
        {
            movementStateMachine.ChangeState(player.Falling);
            return;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetVelocityY(player.JumpForce);
    }
}
