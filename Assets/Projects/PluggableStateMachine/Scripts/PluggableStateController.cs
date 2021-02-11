using RaycastPhysics;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PluggableStateController : MonoBehaviour
{
    public State currentState;
    public State remainState; // The state we remain in if we cannot transition.
    [SerializeField] private CharacterData characterData;
    public CharacterData CharacterData { get => characterData; }
    #region Debug
    [SerializeField] private float wireSphereRadius = 1f;
    #endregion
    public Controller2D CharacterController { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        CharacterController = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update() => currentState.UpdateState(this);

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
