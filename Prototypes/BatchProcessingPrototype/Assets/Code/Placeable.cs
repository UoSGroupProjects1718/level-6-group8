using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up,
    right,
    down,
    left
}

public class Placeable : MonoBehaviour
{
    [SerializeField]
    Direction dir;

    public Direction getDir() { return dir; }

    void Start()
    { }

    void Update()
    { }

    private void Rotate()
    {
        switch (dir)
        {
            case Direction.up:
                dir = Direction.right;
                
                break;
            case Direction.right:
                dir = Direction.down;
                break;
            case Direction.down:
                dir = Direction.left;
                break;
            case Direction.left:
                dir = Direction.up;
                break;
        }
        transform.Rotate(new Vector3(0, 0, 1), -90);
    }

    private void OnMouseOver()
    {
        // Right mouse click
        if (Input.GetMouseButtonDown(1))
        {
            Rotate();
        }
    }
}