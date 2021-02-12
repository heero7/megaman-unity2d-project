using System;
public class BardentStateMachine<T> where T : IBardentState
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
        Type type; 
        Previous = CurrentState.ToString();
        CurrentState.OnExit();
        CurrentState = next;
        CurrentState.OnEnter();
    }
}