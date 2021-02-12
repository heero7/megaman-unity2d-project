using UnityEngine;

/// <summary>
/// Decision class. All subclasses will be responsible
/// for making a decision.
/// </summary>
public abstract class Decision : ScriptableObject
{
    public abstract bool Decide(PluggableStateMachineController stateMachine);
}
