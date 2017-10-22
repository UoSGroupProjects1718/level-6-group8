using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : Machine
{
    Item bufferChild;
    Item activeChild;

    void Start()
    {
        bufferChild = null;
        activeChild = null;
        lc = GameObject.Find("LevelController").GetComponent<LevelController>();
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
        Machine neighbour = lc.GetNeighbour(parent.X, parent.Y, dir);

        // Null check
        if (neighbour == null) { return; }
        if (activeChild == null) { return; }

        // Give child to neighbour
        neighbour.Receive(activeChild);
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
        activeChild.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    public override void Execute()
    {
        // Any item modifying behaviour goes here
    }

    public override void Receive(Item newItem)
    {
        if (bufferChild != null)
        {
            // Error: conveyer has already been given a child
            return;
        }

        bufferChild = newItem;
    }

    public override void Reset()
    {
        RemoveAndDestroyItem(ref bufferChild);
        RemoveAndDestroyItem(ref activeChild);
    }
}
