using UnityEngine;

/// <summary>
/// Class Singleton. 
/// This class will be the generic blueprint for
/// any manager that needs to only be instantiated
/// once. All singletons will be added to the GameManager
/// GameObject.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private const string GAME_MANAGER = "GameManager";
    private static T instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"More than one of these instances exists, type: {typeof(T)}");
            return;
        }
    }

    public static T Instance
    {
        get
        {

            if (instance == null)
            {
                var gm = GameObject.FindGameObjectWithTag(GAME_MANAGER);
                if (gm == null)
                {
                    gm = CreateGameManager();
                }
                else if (gm.GetComponent<GameManager>() == null)
                {
                    gm.AddComponent<GameManager>();
                }

                instance = gm.GetComponent<T>();
                Debug.Log($"{typeof(T)} Singleton instance created");
            }

            return instance;
        }
    }

    /// <summary>
    /// Create the Game Manager if not found
    /// </summary>
    private static GameObject CreateGameManager()
    {
        var gameManager = new GameObject();
        gameManager.name = "GameManager";
        gameManager.transform.position = new Vector2(0, 0);
        gameManager.tag = GAME_MANAGER;
        gameManager.AddComponent<DontDestroy>();
        gameManager.AddComponent<T>();
        return gameManager;
    }
}
