public class StateMachine<T> where T : IState
{
    public T CurrentState { get; private set; }
    public string Previous { get; private set; }

    public void Init(T initialState)
    {
        CurrentState = initialState;
        CurrentState.OnEnter();
        Previous = string.Empty;
    }

    public void ChangeState(T next)
    {
        Previous = CurrentState.ToString();
        CurrentState.OnExit();
        CurrentState = next;
        CurrentState.OnEnter();
    }
}