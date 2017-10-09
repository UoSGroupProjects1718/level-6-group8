using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoPause : MonoBehaviour
{
    public static bool pause = false;

    private IEnumerator Start()
    {
        DoStart();

        while (Application.isPlaying)
        {
            if (!pause)
                DoUpdate();

            yield return null;

            // You could even have a time variable here
        }
    }

    // That's where it happens !
    protected abstract void DoUpdate();

    protected abstract void DoStart();
}

