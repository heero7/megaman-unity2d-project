using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private Vector2 rawMovementInput;
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool DashPressed { get; private set; }
    public bool DashHeld { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool AttackHeld { get; private set; }
    public bool SpecialAttackPressed { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f; // How long the input is true, before its false.
    private float whenJumpInputWasPressed; // This is the time the jump input was pressed

    private void Update()
    {
        CheckJumpInputHoldTime();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        rawMovementInput = ctx.ReadValue<Vector2>();
        NormalizedInputX = (int)(rawMovementInput * Vector2.right).normalized.x;
        NormalizedInputY = (int)(rawMovementInput * Vector2.up).normalized.y;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            JumpPressed = true;
            whenJumpInputWasPressed = Time.time; // Record when the jump input was pressed
        }
    }

    public void UseJumpInput() => JumpPressed = false;

    // This method will stop from holding the jump button down
    // to spam jumping.
    private void CheckJumpInputHoldTime()
    {
        // This means that after this time has passed set the flag to false.
        // The player must use the Jump button OR else the game will just reset the flag.
        if (Time.time >= whenJumpInputWasPressed + inputHoldTime) JumpPressed = false;
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            DashPressed = true;
        }

        if (ctx.performed)
        {
            DashHeld = true;
        }

        if (ctx.canceled)
        {
            DashHeld = false;
        }
    }
    public void UseDashInput() => DashPressed = false;
    public void OnRegularAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            AttackPressed = true;
        }

        if (ctx.performed)
        {
            AttackHeld = true;
        }

        if (ctx.canceled)
        {
            AttackHeld = false;
        }
    }
    public void UseAttackInput() => AttackPressed = false;
    public void OnSpecialAttack(InputAction.CallbackContext ctx) => SpecialAttackPressed = ctx.ReadValueAsButton();
}