using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PluggableStateInputController : MonoBehaviour
{
    [NonSerialized] public Vector2 movementInput;
    [NonSerialized] public Vector2 movementVector;
    private Keyboard keyboard;

    private void Start()
    {
        keyboard = Keyboard.current;
    }

    private void Update()
    {
        if (keyboard.aKey.isPressed)
        {
            //Debug.Log("Pressing a");
            movementInput.Set(1,0);
        }
        else
        {
            movementInput.Set(0,0);
        }

        if (keyboard.dKey.isPressed)
        {
            //Debug.Log("Pressing d");
            movementInput.Set(0,1);
        }
        else
        {
            movementInput.Set(0,0);
        }
    }
}
