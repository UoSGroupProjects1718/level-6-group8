using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingConveyer : Machine
{
    bool rotate;
    bool firstRun;
    int rotateCounter;
    Item bufferChild;
    Item activeChild;
    Direction originalDirection;
    List<Direction> directionsToFace;

	void Start ()
    {
        bufferChild = null;
        activeChild = null;
        rotate = false;
        firstRun = true;
        directionsToFace = new List<Direction>();
        ResetTickCounter();	
	}

    private List<Direction> GetClockwiseSpinFrom(Direction dir)
    {
        List<Direction> dirs = new List<Direction>();

        switch (dir)
        {
            case Direction.up:
                dirs.Add(Direction.right);
                dirs.Add(Direction.down);
                dirs.Add(Direction.left);
                break;

            case Direction.right:
                dirs.Add(Direction.down);
                dirs.Add(Direction.left);
                dirs.Add(Direction.up);
                break;

            case Direction.down:
                dirs.Add(Direction.left);
                dirs.Add(Direction.up);
                dirs.Add(Direction.right);
                break;

            case Direction.left:
                dirs.Add(Direction.up);
                dirs.Add(Direction.right);
                dirs.Add(Direction.down);
                break;
        }

        return dirs;
    }

    private Direction GetOppositeDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.up:
                return Direction.down;
            case Direction.right:
                return Direction.left;
            case Direction.down:
                return Direction.up;
            case Direction.left:
            default:
                return Direction.right;
        }
    }

    /// <summary>
    /// Calculates which directions it has to face based on its neighbours
    /// </summary>
    public override void Begin()
    {
        // Remember the original direction
        originalDirection = GetDirection;
        rotateCounter = 0;

        // Clear our directions to face
        directionsToFace.Clear();

        // Get a list of a full spin relative to our facing direction
        List<Direction> dirs = GetClockwiseSpinFrom(originalDirection);

        // Add in our first dir
        directionsToFace.Add(originalDirection);

        // Check every other dir
        foreach (Direction direction in dirs)
        {
            // Get the neighbour in this direction
            Machine dirNeighbour = LevelController.Instance.GetNeighbour(parent.X, parent.Y, direction);

            // If its not null and not facing us (thus not giving us items)
            if (dirNeighbour != null && dirNeighbour.GetDirection != GetOppositeDirection(direction))
            {
                // We will rotate to this direction
                directionsToFace.Add(direction);
            }
        }

        // Finally, reset our rotation
        SetDir(originalDirection);
    }

    /// <summary>
    /// Give our activeChild to the machine we are facing's buffer
    /// </summary>
    public override void Tick()
    {
        // Tick count
        tickCounter++;
        if (tickCounter < ticksToExecute) { return; }

        ResetTickCounter();

        // Get machine im facing
        Machine neighbour = LevelController.Instance.GetNeighbour(parent.X, parent.Y, dir);

        // Null check
        if (neighbour == null) { return; }
        if (activeChild == null) { return; }

        // Give child to neighbour
        neighbour.Receive(ref activeChild);
        activeChild = null;
    }

    public override void Flush()
    {
        // Check if we have a buffer to flush
        if (bufferChild == null) { return; }

        // Shove our buffer child into our active child
        activeChild = bufferChild;

        // Set our buffer to null
        bufferChild = null;

        // Move our active childs potition to this conveyer
        //activeChild.gameObject.transform.position = new Vector3(transform.position.x, activeChild.ProductionLine_YHeight, transform.position.z);
        StartCoroutine(MoveChildTowardsMe(activeChild));

        // Rotate
        rotate = true;
    }

    public override void Execute()
    {
        if (rotate)
        {
            rotate = false;

            // Move onto the next rotation
            rotateCounter++;

            if (rotateCounter == directionsToFace.Count)
                rotateCounter = 0;

            // Face this direction
            SetDir(directionsToFace[rotateCounter]);


        }
    }

    public override void Receive(ref Item newItem)
    {
        // If we already have a child
        if (bufferChild != null)
        {
            // We are recieving multiple children
            // Therefore, our "child" is contaminated and turns to waste..

            // Destroy our current bufferChild
            RemoveAndDestroyItem(ref bufferChild);

            // Destroy the newItem we got passed
            RemoveAndDestroyItem(ref newItem);

            // Spawn waste as our new bufferChild
            bufferChild = Instantiate(GameManager.Instance.Waste.gameObject).GetComponent<Item>();
            bufferChild.transform.position = new Vector3(transform.position.x, bufferChild.ProductionLine_YHeight, transform.position.z);

            // Add it to our list of items
            AddItem(ref activeChild);
        }
        // Else...
        else
        {
            bufferChild = newItem;
        }
    }

    public override void Reset()
    {
        // Reset the machine back to its original rotation
        SetDir(originalDirection);

        RemoveAndDestroyItem(ref bufferChild);
        RemoveAndDestroyItem(ref activeChild);
    }
}
