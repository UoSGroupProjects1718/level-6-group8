using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : Machine
{
    Item activeChild;
    List<Item> bufferChildren;

    void Start()
    {
        bufferChildren = new List<Item>();
        activeChild = null;
        ResetTickCounter();
    }

    /// <summary>
    /// Called upon the production line beginning
    /// </summary>
    public override void Begin() { return; }

    /// <summary>
    /// Give our activeChild to the machine we are facing's buffer
    /// </summary>
    public override void Tick()
    {
        // If we have an active child, start ticking
        if (activeChild != null)
        {
            // Tick count
            tickCounter++;
            if (tickCounter < ticksToExecute) { return; }

            ResetTickCounter();

            // Get the machine im facing
            Machine neighbour = LevelController.Instance.GetNeighbour(parent.X, parent.Y, GetDirection);

            // Null check
            if (neighbour == null) { return; }

            // Give children to neighbour
            if (neighbour.CanReceiveFrom(this))
            {
                neighbour.Receive(ref activeChild);
                activeChild = null;
            }
        }
    }

    public override void Flush()
    {
        // Check if we have a buffer to flush
        if (bufferChildren.Count == 0) { return; }

        // 1 buffer child
        else if (bufferChildren.Count == 1)
        {
            // Shove our buffer child into our active child
            activeChild = bufferChildren[0];

            // Clear bufferchildren
            bufferChildren.Clear();

            // Move our active childs potition to this conveyer
            //activeChild.gameObject.transform.position = new Vector3(transform.position.x, activeChild.ProductionLine_YHeight, transform.position.z);
            StartCoroutine(MoveChildTowardsMe(activeChild));
        }

        // More
        else
        {
            foreach (Item child in bufferChildren)
            {
                // Move our active childs potition to this conveyer
                StartCoroutine(MoveChildTowardsMe(child));
            }
        }
    }


    public override void Execute()
    {
        // If we need to spawn waste
        if (bufferChildren.Count > 1)
        {
            // Destroy our current bufferChild
            RemoveAndDestroyListOfItems(ref bufferChildren);

            // Spawn waste
            activeChild = Instantiate(GameManager.Instance.Waste.gameObject).GetComponent<Item>();
            activeChild.transform.position = new Vector3(transform.position.x, activeChild.ProductionLine_YHeight, transform.position.z);
            activeChild.transform.Rotate(activeChild.ProductionLine_Rotation);
            activeChild.transform.localScale = activeChild.ProductionLine_Scale;

            // Add it to our list of items
            AddItem(ref activeChild);
        }

        bufferChildren.Clear();
    }

    public override bool CanReceiveFrom(Machine from)
    {
        return true;
    }

    public override void Receive(ref Item newItem)
    {
        bufferChildren.Add(newItem);
    }

    public override void Reset()
    {
        RemoveAndDestroyListOfItems(ref bufferChildren);
        RemoveAndDestroyItem(ref activeChild);
    }
}
