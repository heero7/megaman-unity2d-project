using UnityEngine;

/// <summary>
/// Action class. Base class for all other actions.
/// These are called from states.
/// </summary>
public abstract class StateAction : ScriptableObject    //TODO: Rename this to Action. But place this in a namespace.
{
    /// <summary>
    /// Does the logic for this action.
    /// </summary>
    /// <param name="controller">Reference to the controller.</param>
    public abstract void Perform(PluggableStateController controller);
}
