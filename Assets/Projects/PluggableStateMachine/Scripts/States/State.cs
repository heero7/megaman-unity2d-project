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

    /// <summary>
    /// Calls logic for this state in the game loop.
    /// </summary>
    /// <param name="controller">Reference to the controller.</param>
    public void UpdateState(PluggableStateController controller)
    {
        PerformActions(controller);
        CheckTransitions(controller);
    }

    /// <summary>
    /// Executes all the actions for the state.
    /// </summary>
    /// <param name="controller">Reference to the controller.</param>
    private void PerformActions(PluggableStateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Perform(controller);
        }
    }

    private void CheckTransitions(PluggableStateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            var decision = transitions[i].decision;
            bool decisionSucceeded = decision.Decide(controller);

            if (decisionSucceeded)
            {
                controller.TransitionToState(transitions[i].trueState);
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
}
