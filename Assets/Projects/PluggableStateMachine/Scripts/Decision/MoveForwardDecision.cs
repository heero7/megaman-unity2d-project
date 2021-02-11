using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// MoveForwardDecision class. Condition if 
/// the controller should move forward [horizontally (-x or +x)].
/// </summary>
[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/MoveForward")]
public class MoveForwardDecision : Decision
{
    public override bool Decide(PluggableStateController controller)
    {
        var commandReceived = HasReceivedHorizontalCommand(controller);
        return commandReceived;
    }

    private bool HasReceivedHorizontalCommand(PluggableStateController controller)
    {
        var currentKeyboard = Keyboard.current;
        return currentKeyboard.aKey.isPressed || currentKeyboard.dKey.isPressed;
    }
}
