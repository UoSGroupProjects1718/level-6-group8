﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Output : Machine
{
    List<Item> bufferChildren;
    List<Item> activeChildren;

	void Start ()
    {
        lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        bufferChildren = new List<Item>();
        activeChildren = new List<Item>();
	} 

    /// <summary>
    /// Output inplements no Tick(). Simply returns;
    /// </summary>
    public override void Tick()
    {
        // We will be receiving children during the tick() stage
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
            // If we have the required potion
            if (child.DisplayName.Equals(lc.LevelFactory.Potion.DisplayName))
            {
                // Level complete
                lc.OnLevelComplete();
            }
        }

        // Go through our active children and destroy them
        RemoveAndDestroyListOfItems(ref activeChildren);

        // --V Old code for when we had currency
        //foreach (var item in activeChildren)
        //{
        //    lc.Player.Money += item.Cost;
        //}
    }

    public override void Receive(Item newItem)
    {
        // Add this new item into our buffer
        bufferChildren.Add(newItem);
    }

    public override void Reset()
    {
        RemoveAndDestroyListOfItems(ref bufferChildren);
        RemoveAndDestroyListOfItems(ref activeChildren);
    }
}
