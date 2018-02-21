using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimmableObject : MonoBehaviour
{
    static float percentToDim = .75f;

    public void Dim()
    {
        Renderer ren = GetComponent<Renderer>();
        ren.material.color = new Color(
            ren.material.color.r * (1 - percentToDim),
            ren.material.color.g * (1 - percentToDim),
            ren.material.color.b * (1 - percentToDim),
            ren.material.color.a);
    }

    public virtual void Brighten()
    {
        GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    }
}