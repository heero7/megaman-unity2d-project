using UnityEngine;

public class PlayerJumpState : PlayerMovementState
{
    public bool DashJumping { get; set; }
    public PlayerJumpState(PlayerController player, BardentStateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void ApplyGravity() => player.Rigidbody2D.AddForce(Vector2.down * player.Gravity);

    public override void OnExecute()
    {
        base.OnExecute();
        player.CheckIfShouldFlip(horizontalInput);
        player.SetVelocityX(player.MovementData.runSpeed * horizontalInput);

        if (player.ClimbLadderRequest())
        {
            player.SetVelocityX(0);
            player.SnapToLadderLocation();
            movementStateMachine.ChangeState(player.ClimbingLadder);
            return;
        }
    }
}