using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MachineType
{
    input,
    conveyer,
    pestlemortar,
    brewer,
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
    [Header("Ticks to execute")]
    [SerializeField]
    protected int ticksToExecute;
    protected int tickCounter;
    
    [Header("Type of Machine")]
    [SerializeField]
    protected MachineType type;
    protected Direction dir;
    protected Tile parent;

    [Header("Cost")]
    [SerializeField]
    protected int cost;

    public MachineType Type { get { return type; } }
    public Direction GetDirection { get { return dir; } }
    public int Cost { get { return cost; } }
    public Tile Parent {
        get { return parent; }
        set { parent = value; }
    }

    void Start ()
    {
        dir = Direction.up;
        // lc = GameObject.Find("LevelController").GetComponent<LevelController>();
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
    /// Takes in a child and moves this child towards the machine over the duration of the tick
    /// </summary>
    /// <param name="child">The child to move</param>
    /// <returns></returns>
    protected IEnumerator MoveChildTowardsMe(Item child)
    {
        Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 childPos = new Vector2(child.transform.position.x, child.transform.position.z);

        float duration = LevelController.Instance.TickWaitTime;
        float timeCounter = 0;

        while (timeCounter < duration)
        {
            timeCounter += Time.deltaTime;

            // We do a null check here as the child can be "consumed" by another machine whilst moving along the conveyer
            if (child == null) { break; }

            child.transform.position = Vector3.MoveTowards ( child.transform.position,
                    new Vector3(transform.position.x, child.transform.position.y, transform.position.z),
                    timeCounter / duration );
            yield return null;
        }
    }

    /// <summary>
    /// Receive an item and add it to our machines buffer
    /// </summary>
    /// <param name="newItem">The item we are receiving</param>
    public abstract void Receive(ref Item newItem);

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
        LevelController.Instance.AddItem(ref item);
    }

    /// <summary>
    /// Destroys the item and removes item from the LevelControllers list of items
    /// </summary>
    /// <param name="item">Item to remove and destroy</param>
    public void RemoveAndDestroyItem(ref Item item)
    {
        LevelController.Instance.RemoveAndDestroyItem(ref item);
    }

    /// <summary>
    /// Destroys a list of items and removes them from the LevelControllers list of items
    /// </summary>
    /// <param name="item">List of item to remove and destroy</param>
    protected void RemoveAndDestroyListOfItems(ref List<Item> list)
    {
        LevelController.Instance.RemoveAndDestroyListOfItems(ref list);
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
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            // If we're in delete mode
            if (LevelController.Instance.BuildStatus == BuildStatus.delete)
            {
                DeleteSelf();
            }
        }

        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            Rotate();
        }
    }

    protected void DeleteSelf()
    {
        // Our parent tiles child will be null.
        parent.SetChild(null);

        // Remove this machine from the list of machines.
        LevelController.Instance.RemoveMachine(this);

        // Destroy self
        Destroy(gameObject);
    }
}
