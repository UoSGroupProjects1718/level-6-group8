using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Square
{
    Placeable child;
    int x, y;

    public void SetXY(int newX, int newY)
    {
        x = newX;
        y = newY;
    }

    void Start()
    { }

    void Update()
    { }

    public void SetChild(Placeable newChild)
    {
        if (child != null)
        {
            return;
        }

        child = newChild;
        child.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
    }
}
