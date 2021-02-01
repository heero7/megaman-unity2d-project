using UnityEngine;

public class PlayerJumpState : PlayerMovementState
{
    public PlayerJumpState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void ApplyGravity() => player.Rigidbody.AddForce(Vector2.down * player.Gravity);

    public override void OnExecute()
    {
        base.OnExecute();
        player.CheckIfShouldFlip(horizontalInput);
        player.SetVelocityX(player.MovementData.runSpeed * horizontalInput);
    }
}