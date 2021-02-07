using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private IEnumerator CleanUp(GameObject o)
    {
        yield return new WaitForSeconds(10f);
        Destroy(o);
    }

    private IEnumerator CleanUp(ParticleSystem o, float seconds, Action func)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(o);
        func?.Invoke();
        // Now we can do something.
    }

    public void CleanUpFX(GameObject o)
    {
        StartCoroutine(CleanUp(o));
    }

    // https://forum.unity.com/threads/solved-particle-instance-finished-playing-so-can-i-destroy-it.412234/
    public void DestroyNonLoopParticleSystem(ParticleSystem p, Action func = null)
    {
        var timeDuration = p.main.duration + p.startLifetime;
        StartCoroutine(CleanUp(p, timeDuration, func));
    }
}