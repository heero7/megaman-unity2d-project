﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRisingState : PlayerJumpState
{
    public PlayerRisingState(PlayerController player, BardentStateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();
        if (DashJumping)
        {
            AfterImagePool.Instance.RetrieveAfterImageFromPool();
            player.SetVelocityX(player.CurrentVelocity.x * player.MovementData.DashJumpAerialSpeed);
        }

        if (player.CurrentVelocity.y <= 0f)
        {
            movementStateMachine.ChangeState(player.Falling);
            return;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if(player.InputController.DashHeld)
        {
            DashJumping = true;
            player.SetVelocityX(player.CurrentVelocity.x * player.MovementData.DashJumpAerialSpeed);
        }
        player.SetVelocityY(player.JumpForce);
    }
}
