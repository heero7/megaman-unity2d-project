using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// IdleAction class. Controller remains in idle,
/// and is able to transition to many states.
/// </summary>
[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/IdleAction")]
public class IdleAction : StateAction
{
    public override void Perform(PluggableStateMachineController controller)
    {
        Idle();
    }

    private void Idle()
    {
    }
}