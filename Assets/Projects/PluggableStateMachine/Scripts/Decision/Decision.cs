using UnityEngine;

/// <summary>
/// Decision class. All subclasses will be responsible
/// for checking if the state machine should transition
/// states.
/// </summary>
public abstract class Decision : ScriptableObject
{
    public abstract bool Decide(PluggableStateController controller);
}
