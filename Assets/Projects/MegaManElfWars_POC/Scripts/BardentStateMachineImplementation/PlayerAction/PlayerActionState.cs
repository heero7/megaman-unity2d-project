public class PlayerActionState : IBardentState
{
    protected PlayerController player;
    protected BardentStateMachine<PlayerActionState> actionStateMachine;
    protected BardentStateMachine<PlayerMovementState> movementStateMachine;

    protected readonly string RegularAttackAnimName = "RegularAttack";
    protected bool _attackPressed;
    protected bool _attackHeld;

    private readonly string stateAnimationName;

    public PlayerActionState(
        PlayerController player,
        BardentStateMachine<PlayerActionState> actionStateMachine,
        BardentStateMachine<PlayerMovementState> movementStateMachine
    )
    {
        this.player = player;
        this.actionStateMachine = actionStateMachine;
        this.movementStateMachine = movementStateMachine;
    }

    public virtual void OnEnter()
    {
        //player.Animator.SetBool("StateAnimationName", true);
    }

    public virtual void OnExecute() 
    {
        _attackPressed = player.InputController.AttackPressed;
        _attackHeld = player.InputController.AttackHeld;
    }

    public virtual void OnExit()
    {
        //player.Animator.SetBool("StateAnimationName", false);
    }
}