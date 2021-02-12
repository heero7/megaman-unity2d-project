using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Gravity")]
public class GravityAction : StateAction
{
    // Options: Should this read from a value somewhere? 
        // Like the level itself?
        // The Character itself?
    private Vector2 delta;
    public float gravity; // TODO: Make this a float reference
    public override void Perform(PluggableStateMachineController controller)
    {
       ApplyGravity(controller);
    }

    private void ApplyGravity(PluggableStateMachineController controller)
    {
        controller.Player.movementVector.y = -gravity;
    }
}
