using UnityEngine;

/// <summary>
/// State class. Holds the actions to be performed
/// in this current state.
/// </summary>
[CreateAssetMenu(menuName = "PluggableStateMachine/State")]
public class State : ScriptableObject
{
    public StateAction[] actions;
    #region Debug
    public Color debugGizmoColor = Color.gray;
    #endregion

    /// <summary>
    /// Calls logic for this state in the game loop.
    /// </summary>
    /// <param name="controller">Reference to the controller.</param>
    public void UpdateState(PluggableStateController controller) => PerformActions(controller);

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
}
