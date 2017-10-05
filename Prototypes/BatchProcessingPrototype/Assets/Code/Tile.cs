using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Placeable child;

    void Start()
    { }

    void Update()
    { }

    public void SetChild()
    {
        if (child != null)
        {
            return;
        }
    }
}
