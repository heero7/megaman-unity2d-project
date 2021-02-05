using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private IEnumerator CleanUp(GameObject o)
    {
        yield return new WaitForSeconds(10f);
        Destroy(o);
    }

    public void CleanUpFX(GameObject o)
    {
        StartCoroutine(CleanUp(o));
    }
}