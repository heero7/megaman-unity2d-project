using UnityEngine;

/// <summary>
/// State class. Holds the actions to be performed
/// in this current state.
/// </summary>
[CreateAssetMenu(menuName = "PluggableStateMachine/State")]
public class State : ScriptableObject
{
    public StateAction[] actions;
    public Transition[] transitions;
    #region Debug
    public Color debugGizmoColor = Color.gray;
    #endregion

    // Inspector Description
    // TODO: Make this a custom Inspector Window
    [SerializeField] private string description;

    public void OnEnter(PluggableStateMachineController stateMachine) => RunEntrances(stateMachine);

    private void RunEntrances(PluggableStateMachineController stateMachine)
    {
        foreach (StateAction action in actions)
        {
            action.Entrance(stateMachine);
        }
    }

    /// <summary>
    /// Calls logic for this state in the game loop.
    /// </summary>
    /// <param name="controller">Reference to the controller.</param>
    public void UpdateState(PluggableStateMachineController stateMachine)
    {
        PerformActions(stateMachine);
        CheckTransitions(stateMachine);
    }

    /// <summary>
    /// Executes all the actions for the state.
    /// </summary>
    /// <param name="controller">Reference to the controller.</param>
    private void PerformActions(PluggableStateMachineController stateMachine)
    {
        foreach (StateAction v in actions)
        {
            v.Perform(stateMachine);
        }
    }

    /// <summary>
    /// Checks to see if the state machine should remain
    /// in this state or moves to the next one.
    /// </summary>
    /// <param name="stateMachine">StateMachine controller</param>
    private void CheckTransitions(PluggableStateMachineController stateMachine)
    {
        foreach (Transition transition in transitions)
        {   
            var decisions = transition.decisions;
            var success = false;
            foreach (var decision in decisions)
            {
                var decisionSucceeded = decision.Decide(stateMachine);
                success = decisionSucceeded;
                if (!decisionSucceeded) break;
                
            }

            var nextState = success
                ? transition.trueState
                : transition.falseState;

            stateMachine.TransitionToState(nextState);
        }
    }
}
