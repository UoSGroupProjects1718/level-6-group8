using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : Machine
{
    List<Item> bufferChildren;
    List<Item> activeChildren;
    Item createdItem;

    void Start()
    {
        lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        bufferChildren = new List<Item>();
        activeChildren = new List<Item>();
        ResetTickCounter();
    }

    /// <summary>
    /// Pass our createdItem over to the neighbour
    /// </summary>
    public override void Tick()
    {
        // Tick count
        tickCounter++;
        if (tickCounter < ticksToExecute) { return; }

        ResetTickCounter();

        // Check if we have anything to pass
        if (createdItem == null) { return; }

        // Get machine im facing
        Machine neighbour = lc.GetNeighbour(parent.X, parent.Y, dir);

        // Neighbour null check
        if (neighbour == null) { return; }

        neighbour.Receive(createdItem);
        createdItem = null;
    }


    public override void Flush()
    {
        // Move our buffer children over to our active chilren
        activeChildren = bufferChildren;

        // Reset our buffer children
        bufferChildren = new List<Item>();
    }

    public override void Execute()
    {
        // Create our item
        if (activeChildren.Count > 0)
        {
            createdItem = CreateItem();

            if (createdItem != null)
            {
                AddItem(ref createdItem);
            }
        }
        
        // Go through our active children and destroy them
        RemoveAndDestroyListOfItems(ref activeChildren);
    }

    private CraftableItem CreateItem()
    {
        // Loop through the craftables
        foreach (var craftable in lc.CraftableItems)
        {

            // To check if we've found the correct item
            bool correctItem = false;

            // For each of our active children
            foreach (var ingredient in activeChildren)
            {
                // If the current craftables ingredients doesnt contain our current ingredient

                // --V this doesn't work currently
                // Probably because we have no operator= for Item.cs
                //if (!craftable.Ingredients.Contains(ingredient))
                //{
                //    break;
                //}

                bool containsThisIngredient = false;
                foreach (var craftableIngredient in craftable.Ingredients)
                {
                    if (craftableIngredient.DisplayName.Equals(ingredient.DisplayName))
                    {
                        containsThisIngredient = true;
                    }
                }
                if (!containsThisIngredient)
                {
                    break;
                }

                // If we get passed all breaks, this is the correct item
                correctItem = true;
            }

            // If this was the right item, return it
            if (correctItem)
            {
                var item = Instantiate(craftable, new Vector3(0, 0, 0), Quaternion.identity);
                return item;
            }
        }

        return null;
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
