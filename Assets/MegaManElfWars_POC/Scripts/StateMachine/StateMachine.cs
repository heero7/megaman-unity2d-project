public class StateMachine<T> where T : IState
{
    public T CurrentState { get; private set; }

    public void Init(T initialState)
    {
        CurrentState = initialState;
        CurrentState.OnEnter();
    }

    public void ChangeState(T next)
    {
        CurrentState.OnExit();
        CurrentState = next;
        CurrentState.OnEnter();
    }
}