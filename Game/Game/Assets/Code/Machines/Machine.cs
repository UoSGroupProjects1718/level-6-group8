﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MachineType
{
    // Default
    input,
    output,

    // Player-spawnable
    conveyer,
    grinder,
    brewer,
    oven,
    slow_conveyer,
    rotate_conveyer
}

[System.Serializable]
public enum Direction
{
    up,
    right,
    down,
    left
}

public abstract class Machine : DimmableObject
{
    [Header("Ticks to execute")]
    [SerializeField]
    protected int ticksToExecute;
    protected int tickCounter;
    
    [Header("Type of Machine")]
    [SerializeField]
    protected MachineType type;
    [SerializeField]
    protected Direction dir;
    protected Tile parent;

    [Header("Cost")]
    [SerializeField]
    protected int cost;

    [Header("Sprite")]
    [SerializeField]
    protected Sprite sprite;

    [Header("Y offset from floor")]
    [SerializeField]
    protected float yOffset;

    [Header("Rotation offset")]
    [SerializeField]
    protected float rotationOffset;

    public MachineType Type { get { return type; } }
    public Direction GetDirection { get { return dir; } }
    public int Cost { get { return cost; } }
    public float YOffset { get { return yOffset; } }
    public float RotationOffset { get { return rotationOffset; } }
    public Sprite Sprite { get { return sprite; } }
    public Tile Parent {
        get { return parent; }
        set { parent = value; }
    }

    void Start ()
    {
        dir = Direction.up;
	}

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            // Raycast to see if we hit
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.transform == this.gameObject.transform)
                {
                    OnTouch();
                }
            }

        }
    }

    /// <summary>
    /// This function is called on the machine when the production line starts running
    /// </summary>
    public abstract void Begin();

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
        //Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
        //Vector2 childPos = new Vector2(child.transform.position.x, child.transform.position.z);

        float duration = LevelController.Instance.TickWaitTime;
        float timeCounter = 0;

        while (timeCounter < duration)
        {
            timeCounter += Time.deltaTime;

            // We do a null check here as the child can be "consumed" by another machine whilst moving along the conveyer
            if (child == null) { break; }

            child.transform.position = Vector3.MoveTowards(child.transform.position,
                    new Vector3(transform.position.x, child.transform.position.y, transform.position.z),
                    timeCounter / duration);
            yield return null;
        }
    }

    /// <summary>
    /// Checks whether or not a machine can receieve an ingredient from a given machine
    /// </summary>
    /// <param name="from">The machine its receiving from</param>
    /// <returns>True if can receive, false otherwise</returns>
    public abstract bool CanReceiveFrom(Machine from);

    /// <summary>
    /// Receive an item and add it to our machines buffer
    /// </summary>
    /// <param name="newItem">The item we are receiving</param>
    /// <param name="from">The machine we're receiving the item from</param>
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

    public void RotateAnticlockwise()
    {
        switch (dir)
        {
            case Direction.up:
                SetDir(Direction.left);
                break;
            case Direction.right:
                SetDir(Direction.up);
                break;
            case Direction.down:
                SetDir(Direction.right);
                break;
            case Direction.left:
                SetDir(Direction.down);
                break;
        }

        EventManager.Instance.AddEvent(EventType.Machine_Rotated);
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

        EventManager.Instance.AddEvent(EventType.Machine_Rotated);
    }

    public void SetDir(Direction newDir)
    {
        dir = newDir;

        switch (dir)
        {
            case Direction.right:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270 + RotationOffset, transform.eulerAngles.z);
                break;
            case Direction.down:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0 + RotationOffset, transform.eulerAngles.z);
                break;
            case Direction.left:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90 + RotationOffset, transform.eulerAngles.z);
                break;
            case Direction.up:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 + RotationOffset, transform.eulerAngles.z);
                break;
        }
        EventManager.Instance.AddEvent(EventType.Machine_Rotated);
    }

    public float GetDirAngle(Direction dir)
    {
        switch (dir)
        {
            case Direction.right:
                return 270 + RotationOffset;
            case Direction.down:
                return 0 + RotationOffset;
            case Direction.left:
                return 90 + RotationOffset;
            case Direction.up:
                return 180 + RotationOffset;
        }
        return 0;
    }

    protected void ResetTickCounter() { tickCounter = 0; }

    #region PCControls

    // This will detect when a user taps on the tile
    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0) && GameManager.Instance.ValidPress())
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                OnTouch();
        }

        // For easier testing
        if (Input.GetMouseButtonDown(1))
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                Rotate();
        }
    }
    #endregion

    private void OnTouch()
    {
        // Return if the production line is running...
        if (LevelController.Instance.Running) { return; }

        // Check that the user is in an appropriate build mode
        // (What do they have selected?)
        switch (LevelController.Instance.BuildStatus)
        {
            // Debugging mode...
            case BuildMode.debugdelete:
                // Only inputs and outputs can be deleted in debug delete mode
                // debug delete mode is not available to the player
                if (type == MachineType.input || type == MachineType.output) { DeleteSelf(); }
                break;

            case BuildMode.rotate:
                // Inputters cannot be rotated
                if (type == MachineType.input) return;

                Rotate();
                break;
            case BuildMode.delete:
                // Don't delete if it's an input or an ouput
                // (they are static and cannot be moved, rotated or deleted)
                if (type == MachineType.input || type == MachineType.output) { return; }
                DeleteSelf();
                break;

            default:
                OnMachinePress();
                break;
        }
    }

    protected abstract void OnMachinePress();

    protected void DeleteSelf()
    {
        // Our parent tiles child will be null.
        parent.SetChild(null, false);

        // Remove this machine from the list of machines.
        LevelController.Instance.RemoveMachine(this);

        // Event
        EventManager.Instance.AddEvent(EventType.Machine_Deleted);

        // Destroy self
        Destroy(gameObject);
    }
}
