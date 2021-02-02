using UnityEngine;
public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();

        if (horizontalInput != 0 && !player.IsTouchingWall(horizontalInput))
        {
            movementStateMachine.ChangeState(player.Run);
            return;
        }

        var result = player.ClimbLadderRequest();
        if (result)
        {
            movementStateMachine.ChangeState(player.ClimbingLadder);
            return;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetVelocityX(0);
        player.SetVelocityY(0);
    }
}