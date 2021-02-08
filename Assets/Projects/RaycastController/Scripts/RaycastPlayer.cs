using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastPlayer : MonoBehaviour
{
    public FloatReference currentHP;
    public FloatReference maxHP;

    private void Awake()
    {
        currentHP.Value = maxHP.Value;
    }

    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                currentHP.Value--;
            }
        }
    }
}