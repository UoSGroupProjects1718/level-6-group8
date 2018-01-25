using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingConveyer : Machine
{
    Item bufferChild;
    Item activeChild;
    bool rotate;
    Orientation orientation;

    private enum Orientation
    {
        normal,
        rotated
    }

	void Start ()
    {
        bufferChild = null;
        activeChild = null;
        rotate = false;
        ResetTickCounter();	
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
        // If we passed an item this cycle
        if (rotate)
        {
            // Rotate
            switch (orientation)
            {
                case Orientation.normal:
                    orientation = Orientation.rotated;
                    Rotate();
                    break;
                case Orientation.rotated:
                    orientation = Orientation.normal;
                    RotateAnticlockwise();
                    break;
            }
        }

        rotate = false;
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
            bufferChild.transform.position = new Vector3(5, transform.position.y + 0.5f, 5);

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
        RemoveAndDestroyItem(ref bufferChild);
        RemoveAndDestroyItem(ref activeChild);
    }
}
