using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Direction
{
    up,
    right,
    down,
    left
}

public abstract class Machine : MonoBehaviour
{
    [SerializeField]
    protected Direction dir;
    public Direction GetDirection { get { return dir; } }

    [SerializeField]
    protected Tile parent;
    public Tile Parent {
        get { return parent; }
        set { parent = value; }
    }

    protected int tickCounter;

    [SerializeField]
    [Header("Ticks to execute")]
    protected int ticksToExecute;

    void Start ()
    {
        dir = Direction.up;
	}

    /// <summary>
    /// Pass item to next machines buffer
    /// </summary>
    public abstract void Tick();

    /// <summary>
    /// Flushes the buffer to the machines main item
    /// </summary>
    public abstract void Flush();

    /// <summary>
    /// Perform operations on the machines item
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// Receive an item and add it to our machines buffer
    /// </summary>
    /// <param name="newItem">The item we are receiving</param>
    public abstract void Receive(Item newItem);

    public void Rotate()
    {
        switch (dir)
        {
            case Direction.up:
                SetDir(Direction.right);
                break;
            case Direction.right:
                SetDir(Direction.down);
                break;
            case Direction.down:
                SetDir(Direction.left);
                break;
            case Direction.left:
                SetDir(Direction.up);
                break;
        }
    }

    public void SetDir(Direction newDir)
    {
        dir = newDir;

        switch (dir)
        {
            case Direction.right:
                transform.eulerAngles = new Vector3(0, 0, -90);
                break;
            case Direction.down:
                transform.eulerAngles = new Vector3(0, 90, -90);
                break;
            case Direction.left:
                transform.eulerAngles = new Vector3(0, 180, -90);
                break;
            case Direction.up:
                transform.eulerAngles = new Vector3(0, -90, -90);
                break;
        }
    }

    protected void ResetTickCounter() { tickCounter = 0; }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Rotate();
        }
    }
}
