using UnityEngine;

/// <summary>
/// IsGroundedDecision class. Checks if the controller
/// is touching the ground.
/// </summary>
[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/IsGrounded")]
public class IsGroundedDecision : Decision
{
    public override bool Decide(PluggableStateController controller) => IsGrounded(controller);

    private bool IsGrounded(PluggableStateController controller) => controller.CharacterController.CollisionInfo.Below;
}
