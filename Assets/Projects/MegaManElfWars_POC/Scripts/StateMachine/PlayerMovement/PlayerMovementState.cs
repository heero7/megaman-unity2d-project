public class PlayerMovementState : IState
{
    protected PlayerController player;
    protected int horizontalInput;
    protected int verticalInput;
    protected bool jumpInputPressed;
    protected bool exitingState;
    protected StateMachine<PlayerMovementState> movementStateMachine;
    protected readonly string animationName;

    public PlayerMovementState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName)
    {
        this.player = player;
        this.movementStateMachine = movementStateMachine;
        this.animationName = animationName;
    }

    public PlayerMovementState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine)
    {
        this.player = player;
        this.movementStateMachine = movementStateMachine;
    }

    public virtual void ApplyGravity() { }
    public virtual void OnEnter()
    {
        exitingState = false;
        player.Animator.SetBool(animationName, true);
    }

    public virtual void OnExecute()
    {
        horizontalInput = player.InputController.NormalizedInputX;
        verticalInput = player.InputController.NormalizedInputY;
        jumpInputPressed = player.InputController.JumpPressed;
    }
    public virtual void OnExit()
    {
        exitingState = true;
        player.Animator.SetBool(animationName, false);
    }
}