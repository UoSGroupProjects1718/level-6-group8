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

public abstract class Machine : Tile
{
    public Direction GetDirection
    {
        get { return dir; }
    }
    protected Direction dir;

	void Start ()
    {
        dir = Direction.up;
	}

    protected int tickCounter;
    protected int maxTicks;

    public abstract void Execute();
    public abstract void Tick();
    public abstract void Flush();
    public abstract void Receive(Item newItem);

    public void Rotate()
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
    }


    protected void ResetTickCounter() { tickCounter = 0; }
}
