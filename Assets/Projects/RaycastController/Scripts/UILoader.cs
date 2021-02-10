using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    private Keyboard k;

    void Start() => k = Keyboard.current;
    private void Update()
    {
        if (k == null) return;

        if (k.f1Key.wasPressedThisFrame)
        {
            // Load the 
            if (SceneManager.GetSceneByName("RaycastUI").isLoaded == false)
            {
                SceneManager.LoadSceneAsync("RaycastUI", LoadSceneMode.Additive);
            }
            else 
            {
                SceneManager.UnloadSceneAsync("RaycastUI");
            }
        }
    }
}
