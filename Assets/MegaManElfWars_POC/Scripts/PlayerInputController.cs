using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private Vector2 rawMovementInput;
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool DashPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool SpecialAttackPressed { get; private set; }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        rawMovementInput = ctx.ReadValue<Vector2>();
        NormalizedInputX = (int)(rawMovementInput * Vector2.right).normalized.x;
        NormalizedInputY = (int)(rawMovementInput * Vector2.up).normalized.y;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        var value1 = ctx.performed;
        var value2 = ctx.canceled;
        //if (value1) Debug.Log($"Jump [Performed]: {value1}");
        //if (value2) Debug.Log($"Jump [Canceled]: {value2}");
        JumpPressed = ctx.ReadValueAsButton();

    }
    public void OnDash(InputAction.CallbackContext ctx) => DashPressed = ctx.ReadValueAsButton();
    public void OnRegularAttack(InputAction.CallbackContext ctx) 
    {
        AttackPressed = ctx.ReadValueAsButton();
    }
    public void ResetAttack() => AttackPressed = false;
    public void OnSpecialAttack(InputAction.CallbackContext ctx) => SpecialAttackPressed = ctx.ReadValueAsButton();
}