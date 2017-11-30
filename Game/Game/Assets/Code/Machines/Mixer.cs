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
        bufferChildren = new List<Item>();
        activeChildren = new List<Item>();
        ResetTickCounter();
    }

    /// <summary>
    /// Pass our createdItem over to the neighbour
    /// </summary>
    public override void Tick()
    {
        /* Firslt,y go through and delete our children that are left over from our previous Execute().
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
        }
    }

    private CraftableItem CreateItem()
    {
        // Loop through all craftable items
        foreach (var potion in GameManager.Instance.CraftableItems)
        {
            // Before we do any checks, ensure we have the number of ingredients that this potion requires
            if (potion.Ingredients.Count == activeChildren.Count)
            {
                bool correctPotion = false;

                // For each ingredient the potion requires...
                foreach (Ingredient potionIngredient in potion.Ingredients)
                {
                    // Check to see if we have this...
                    //if (activeChildren.Contains(potionIngredient)) { correctPotion = true; }
                    //else {correctPotion = false; break; }

                    foreach (var child in activeChildren)
                    {
                        correctPotion = potionIngredient.DisplayName.Equals(child.DisplayName) ? true : false;
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
