using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// IdleAction class. Controller remains in idle,
/// and is able to transition to many states.
/// </summary>
[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/IdleAction")]
public class IdleAction : StateAction
{
    private Keyboard k;
    public override void Perform(PluggableStateController controller)
    {
        Idle();
    }

    private void Idle()
    {
        // Nothing is being done here. 
        // The only thing this will do is probably just play an animation.
    }
}