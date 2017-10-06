using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Square
{
    Placeable child;

    void Start()
    { }

    void Update()
    { }


    public Placeable GetChild() { return child; }

    public void SetChild(Placeable newChild)
    {
        if (child != null)
        {
            return;
        }

        child = newChild;
        child.SetXY(xPos, yPos);
        child.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f); 
    }
}
