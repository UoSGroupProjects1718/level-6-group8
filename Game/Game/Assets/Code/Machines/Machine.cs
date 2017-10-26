using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MachineType
{
    input,
    conveyer,
    mixer,
    output
}

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
    [Header("Ticks to execute")]
    protected int ticksToExecute;
    protected int tickCounter;
    [SerializeField]
    [Header("Type of Machine")]
    protected MachineType type;
    protected Direction dir;
    protected Tile parent;
    protected LevelController lc;

    public MachineType Type { get { return type; } }
    public Direction GetDirection { get { return dir; } }
    public Tile Parent {
        get { return parent; }
        set { parent = value; }
    }

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

    /// <summary>
    /// Resets the machine: destroys all its children, etc.
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// Adds item to the LevelControllers list of items
    /// </summary>
    /// <param name="item">Item to add</param>
    public void AddItem(ref Item item)
    {
        lc.AddItem(ref item);
    }

    /// <summary>
    /// Destroys the item and removes item from the LevelControllers list of items
    /// </summary>
    /// <param name="item">Item to remove and destroy</param>
    public void RemoveAndDestroyItem(ref Item item)
    {
        lc.RemoveAndDestroyItem(ref item);
    }

    /// <summary>
    /// Destroys a list of items and removes them from the LevelControllers list of items
    /// </summary>
    /// <param name="item">List of item to remove and destroy</param>
    protected void RemoveAndDestroyListOfItems(ref List<Item> list)
    {
        lc.RemoveAndDestroyListOfItems(ref list);
    }

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
