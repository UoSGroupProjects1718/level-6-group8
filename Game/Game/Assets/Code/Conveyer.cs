using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : Machine
{
    Item newChild;
    Item child;

    void Start()
    {
        ResetTickCounter();
    }

    public override void Tick()
    {
        tickCounter++;
        if (tickCounter < maxTicks) { return; }

        ResetTickCounter();
        // Get neighbour
        // Give neighbour to child
    }

    public override void Flush()
    {
        // We have no child to flush
        if (newChild == null) { return; }

        child = newChild;
        newChild = null;
    }

    public override void Execute()
    {
        // Any item modifying behaviour goes here
    }

    public override void Receive(Item newItem)
    {
        if (newChild != null)
        {
            // Error: conveyer has already been given a child
            return;
        }

        newChild = newItem;
    }
}
