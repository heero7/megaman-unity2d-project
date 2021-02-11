using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/MoveFowardAction")]
public class HorizontalMovementAction : StateAction
{
    public override void Perform(PluggableStateController controller)
    {
        Move(controller);
    }

    // Create a virtual input manager like Roundbear?
    // Link: https://www.youtube.com/watch?v=HVwss7AFfaA&list=PLWYGofN_jX5BupV2xLjU1HUvujl_yDIN6&index=15
        // This is interesting, because this solves the problem of creating
        // a whole separate slew of Actions/States/Transitions/Conditions.
        // There only has to be one predefined number of actions, anyone can
        // do and you just have to give them the option. 
    private void Move(PluggableStateController controller)
    {
        var currentKeyboard = Keyboard.current;
        if (currentKeyboard == null) return; // Or maybe get out?.. actually checking for plugged in controllers doesn't belong here.
        var direction = 0;
        if (currentKeyboard.aKey.isPressed) direction = -1;
        else if (currentKeyboard.dKey.isPressed) direction = 1;
        // If neither.. well it's 0.

        // TODO: There should be an abstraction for this Move() method in the PluggableStateController,
        // so that every action does not have do this.
        var velocityOnXAxis = controller.CharacterData.moveSpeed * direction;
        var delta = new Vector2(velocityOnXAxis, 0);
        controller.CharacterController.Move(delta * Time.deltaTime, Vector2.zero);
    }
}
