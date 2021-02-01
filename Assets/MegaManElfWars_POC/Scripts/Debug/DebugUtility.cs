using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DebugUtility : MonoBehaviour
{
    [SerializeField] Text movementState, actionState, facingDirection;
    [SerializeField] private PlayerController player;
    private Gamepad gamepad;
    private Keyboard keyboard;

    private void Start() 
    {
        keyboard = Keyboard.current;
        gamepad = Gamepad.current;
    }
    // Update is called once per frame
    void Update()
    {
        movementState.text = player.MovementStateMachine.CurrentState.ToString();
        facingDirection.text = player.FacingDirection.ToString();
        if (keyboard == null && gamepad == null) Debug.LogWarning("No recognized input is connected.");
        if ((keyboard != null && keyboard.rKey.isPressed) || (gamepad != null && gamepad.selectButton.isPressed))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
