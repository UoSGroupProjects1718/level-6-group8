using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Output : Machine
{
    [Header("Output canvas")]
    [SerializeField]
    private UI_OutputPanel outputCanvas;

    private bool itemRecieved;
    private List<Item> bufferChildren;
    private List<Item> activeChildren;

	void Start ()
    {
        itemRecieved = false;
        bufferChildren = new List<Item>();
        activeChildren = new List<Item>();
	}

    // Output does not implement custom MachinePress functionality
    protected override void OnMachinePress() { }

    /// <summary>
    /// Called upon the production line beginning
    /// </summary>
    public override void Begin() { return; }

    /// <summary>
    /// Output inplements no Tick(). Simply returns;
    /// </summary>
    public override void Tick()
    {
        // Destroy our active children, left over from the Execute()
        // Go through our active children and destroy them
        if (activeChildren.Count > 0)
            RemoveAndDestroyListOfItems(ref activeChildren);

        // We will be receiving children in the Tick() stage
        return;
    }

    /// <summary>
    /// Move our buffer children over to our active children
    /// </summary>
    public override void Flush()
    {
        // Move our buffer children over to our active chilren
        activeChildren = bufferChildren;

        // Reset our buffer children
        bufferChildren = new List<Item>();

        // Move the active children towards this output
        if (activeChildren.Count > 0)
        {
            foreach (var child in activeChildren)
            {
                if (child != null)
                {
                    StartCoroutine(MoveChildTowardsMe(child));
                }
            }
        }
    }

    /// <summary>
    /// Check to see if the correct potion was made
    /// Destroy all of our active children.
    /// </summary>
    public override void Execute()
    {
        // Check our active children
        foreach (Item child in activeChildren)
        {
            // This may be changed to only accept the requried item?idk
            LevelController.Instance.LevelFactory.stockpile.AddOrIncrement(child, 1);

            // On the first time we recieve this potion, tell the LevelController
            if (!itemRecieved)
            {
                itemRecieved = true;

                // Set image as complete
                outputCanvas.SetComplete(true);

                LevelController.Instance.ItemCreated(child);
            }
        }
    }

    public override bool CanReceiveFrom(Machine from)
    {
        return true;
    }

    public override void Receive(ref Item newItem)
    {
        // Add this new item into our buffer
        bufferChildren.Add(newItem);
    }

    public override void Reset()
    {
        // Reset if it's not a tutorial
        if (!LevelController.Instance.LevelFactory.IsTutorial)
        {
            itemRecieved = false;

            // Set image as not complete
            outputCanvas.SetComplete(false);
        }
            
        RemoveAndDestroyListOfItems(ref bufferChildren);
        RemoveAndDestroyListOfItems(ref activeChildren);
    }
}
