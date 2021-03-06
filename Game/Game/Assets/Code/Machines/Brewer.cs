﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brewer : Machine
{
    List<Item> bufferChildren;
    List<Item> activeChildren;
    Item createdItem;

    void Start()
    {
        bufferChildren = new List<Item>();
        activeChildren = new List<Item>();
        ResetTickCounter();
    }

    // Brewer does not implement custom MachinePress functionality
    protected override void OnMachinePress() { }

    /// <summary>
    /// Called upon the production line beginning
    /// </summary>
    public override void Begin() { return; }

    /// <summary>
    /// Pass our createdItem over to the neighbour
    /// </summary>
    public override void Tick()
    {
        /* Firslty go through and delete our children that are left over from our previous Execute().
         We have already used them in the previous Execute() to create a new potion, however
         we cannot delete them in the execute because we need to see them go into the mixer in the world
         therefore, we delete them in the Tick() */
        /* Essentially, we are deleting our left over junk before we pass on our createdItem later in this method */
        RemoveAndDestroyListOfItems(ref activeChildren);

        // Tick count
        tickCounter++;
        if (tickCounter < ticksToExecute) { return; }

        ResetTickCounter();

        // Check if we have anything to pass
        if (createdItem == null) { return; }

        // Get machine im facing
        Machine neighbour = LevelController.Instance.GetNeighbour(parent.X, parent.Y, dir);

        // Neighbour null check
        if (neighbour == null) { return; }

        neighbour.Receive(ref createdItem);
        createdItem = null;
    }


    public override void Flush()
    {
        // Move our buffer children over to our active chilren
        activeChildren = bufferChildren;

        // Reset our buffer children
        bufferChildren = new List<Item>();

        // Move the active children towards this mixer
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

    public override void Execute()
    {
        // Create our item
        if (activeChildren.Count > 0)
        {
            StartCoroutine(SpawnPotionAtEndOfTick());
        }
    }

    private IEnumerator SpawnPotionAtEndOfTick()
    {
        float waitTime = LevelController.Instance.TickWaitTime;
        yield return new WaitForSeconds(waitTime);

        createdItem = CreateItem();

        if (createdItem != null)
        {
            AddItem(ref createdItem);

            // Event
            EventManager.Instance.AddEvent(EventType.Brewer_Execute);
        }
    }

    private CraftableItem CreateItem()
    {
        List<string> allIngredients = new List<string>();

        // Check all of our active chilren;
        foreach (Item item in activeChildren)
        {
            // Anything other than compounds will "ruin" the output
            if (item.ItemType != ItemType.compound)
            {

                return null;
            }

            foreach (var ingredient in item.GetComponent<Compound>().Components)
            {
                allIngredients.Add(ingredient);
            }
        }

        // Loop through all craftable items
        foreach (var potion in GameManager.Instance.Potions)
        {
            // Before we do any checks, ensure we have the number of ingredients that this potion requires
            if (potion.Ingredients.Count == allIngredients.Count)
            {
                bool correctPotion = false;

                // For each ingredient the potion requires...
                foreach (Ingredient potionIngredient in potion.Ingredients)
                {
                    // Check to see if we have this...
                    //if (activeChildren.Contains(potionIngredient)) { correctPotion = true; }
                    //else {correctPotion = false; break; }

                    foreach (var ingredient in allIngredients)
                    {
                        correctPotion = potionIngredient.DisplayName.Equals(ingredient) ? true : false;
                        if (correctPotion) break;
                    }
                    if (!correctPotion) break;
                }

                if (correctPotion)
                {
                    // Instantiate the item
                    CraftableItem item = Instantiate(potion).GetComponent<CraftableItem>();

                    // Posotio, rotate and scale the item
                    item.transform.position = new Vector3(transform.position.x, item.ProductionLine_YHeight, transform.position.z);
                    item.transform.localRotation = Quaternion.Euler(item.ProductionLine_Rotation);
                    item.transform.localScale = (item.ProductionLine_Scale);

                    return item;
                }
            }
        }

        // If we loop through every potion and don't find 
        return null;
    }

    public override bool CanReceiveFrom(Machine from)
    {
        // Return false if 'from' is the machine infront of the brewer depending
        // on how we're facing. 
        // Otherwise, return true

        switch (dir)
        {
            case Direction.right:
                if (from.Parent.X == parent.X + 1) return false;
                return true;

            case Direction.left:
                if (from.Parent.X == parent.X - 1) return false;
                return true;

            case Direction.up:
                if (from.Parent.Y == parent.Y + 1) return false;
                return true;

            case Direction.down:
                if (from.Parent.Y == parent.Y - 1) return false;
                return true;

            default:
                return false;
        }
    }

    public override void Receive(ref Item newItem)
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
