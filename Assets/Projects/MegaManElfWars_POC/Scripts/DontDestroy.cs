using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    protected void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
   