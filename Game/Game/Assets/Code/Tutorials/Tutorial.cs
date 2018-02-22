using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    // Our progress through the tutorial
    protected int progress;

    public void Start()
    {
        Reset();
    }

    //! A method to progress to the next 'Step' of the tutorial
    //! E.g. display the next bit of text, show an icon, move the camera, etc...
    public abstract void Progress(EventType _event);

    public void Reset()
    {
        progress = 0;
    }
}