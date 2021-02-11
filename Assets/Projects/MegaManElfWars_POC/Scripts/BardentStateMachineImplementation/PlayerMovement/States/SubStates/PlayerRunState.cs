
public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerController player, BardentStateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();
        player.CheckIfShouldFlip(horizontalInput);
        player.SetVelocityX(player.MovementData.runSpeed * horizontalInput);
        if (horizontalInput == 0 || player.IsTouchingWall(horizontalInput))
        {
            movementStateMachine.ChangeState(player.Idle);
            return;
        }
    }
}