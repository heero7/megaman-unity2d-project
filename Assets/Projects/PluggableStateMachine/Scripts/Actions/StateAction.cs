using UnityEngine;

/// <summary>
/// Action class. Base class for all other actions.
/// These are called from states.
/// </summary>
public abstract class StateAction : ScriptableObject, IStateAction
{
    // TODO: Rename this class to Action. But place this in a namespace.

    /// <summary>
    /// Does the logic for when the action begins.
    /// These methods do not need to be overriden.
    /// </summary>
    public virtual void Entrance(PluggableStateMachineController statemachine) {}

    /// <summary>
    /// Does the logic for this action.
    /// Required to be overriden.
    /// </summary>
    public abstract void Perform(PluggableStateMachineController statemachine);

    /// <summary>
    /// Does the logic for when the action ends.
    /// These methods do not need to be overriden.
    /// </summary>
    public virtual void Exit(PluggableStateMachineController statemachine) {}
}
