using RaycastPhysics;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PluggableStateMachineController : MonoBehaviour
{
    [SerializeField] private State currentState;
    [SerializeField] private State remainState; // The 'null' state, it is used to compare to and the machine will relize it needs to stay in the current state.
    [SerializeField] private float wireSphereRadius = 1f;

    public Controller2D CharacterController { get; private set; }
    public Player Player { get; private set; }

    private void Start()
    {
        CharacterController = GetComponent<Controller2D>();
        Player = GetComponent<Player>();
        currentState.OnEnter(this);
    }

    private void Update() => currentState.UpdateState(this);

    public void TransitionToState(State next)
    {
        if (next != remainState)
        {
            currentState = next;
        }
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.debugGizmoColor;
            Gizmos.DrawWireSphere(transform.position, wireSphereRadius);
        }
    }
}
