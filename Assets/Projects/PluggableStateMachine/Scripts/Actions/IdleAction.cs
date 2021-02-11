using UnityEngine;
using UnityEngine.InputSystem;

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
        Debug.Log("Doing.. nothing.");
    }
}