using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : Machine
{
    Item bufferChild;
    Item activeChild;
    Item createdItem;

    void Start()
    {
        bufferChild = null;
        activeChild = null;
        ResetTickCounter();
    }

    // Oven does not implement custom MachinePress functionality
    protected override void OnMachinePress() { }

    /// <summary>
    /// Called upon the production line beginning
    /// </summary>
    public override void Begin() { return; }

    /// <summary>
    /// Give our activeChild to the machine we are facing's buffer
    /// </summary>
    public override void Tick()
    {
        /* Firstly go and delete our active child that is left over from our previous Execute().
         We have already used them in the previous Execute() to create a new potion, however
         we cannot delete them in the execute because we need to see them go into the oven in the world
         therefore, we delete them in the Tick() */
        /* Essentially, we are deleting our left over junk before we pass on our createdItem later in this method */
        RemoveAndDestroyItem(ref activeChild);

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

        // Check neighbour can receive
        if (neighbour.CanReceiveFrom(this))
        {
            // Pass
            neighbour.Receive(ref createdItem);
            createdItem = null;
        }
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
    }

    public override void Execute()
    {
        // Cook our ingredient
        if (activeChild != null)
        {
            StartCoroutine(SpawnCookedIngredientAtEndOfTick());
        }
    }

    private IEnumerator SpawnCookedIngredientAtEndOfTick()
    {
        // Wait
        float waitTime = LevelController.Instance.TickWaitTime;
        yield return new WaitForSeconds(waitTime);

        if (activeChild != null)
        {
            // Cook our ingredient
            createdItem = CookIngredient();
        }

        // If it's not null
        if (createdItem != null)
        {
            // Add our new ingredient
            AddItem(ref createdItem);
        }
    }

    private Ingredient CookIngredient()
    {
        // If this Item object is an Ingredient
        if (activeChild.ItemType == ItemType.rawIngredient)
        {
            // If it's cooked variant isnt null, return it
            if (activeChild.GetComponent<Ingredient>().CookedVariant != null)
            {
                // Instantiate the booked ingredient
                Ingredient cooked = Instantiate(activeChild.GetComponent<Ingredient>().CookedVariant).GetComponent<Ingredient>();

                // Position, rotate and scale the item
                cooked.transform.position = new Vector3(transform.position.x, cooked.ProductionLine_YHeight, transform.position.z);
                cooked.transform.localRotation = Quaternion.Euler(cooked.ProductionLine_Rotation);
                cooked.transform.localScale = (cooked.ProductionLine_Scale);

                return cooked;
            }
        }

        // Otherwise, instantiate a burnt ingredient
        Ingredient burnt = Instantiate(GameManager.Instance.BurntIngredient).GetComponent<Ingredient>();

        // Posotio, rotate and scale the item
        burnt.transform.position = new Vector3(transform.position.x, burnt.ProductionLine_YHeight, transform.position.z);
        burnt.transform.localRotation = Quaternion.Euler(burnt.ProductionLine_Rotation);
        burnt.transform.localScale = (burnt.ProductionLine_Scale);

        return burnt;
    }

    public override bool CanReceiveFrom(Machine from)
    {
        switch (dir)
        {
            case Direction.left:
                if (from.Parent.X == parent.X + 1) return true;
                return false;

            case Direction.right:
                if (from.Parent.X == parent.X - 1) return true;
                return false;

            case Direction.down:
                if (from.Parent.Y == parent.Y + 1) return true;
                return false;

            case Direction.up:
                if (from.Parent.Y == parent.Y - 1) return true;
                return false;

            default:
                return true;
        }
    }

    public override void Receive(ref Item newItem)
    {
        bufferChild = newItem;
    }

    public override void Reset()
    {
        RemoveAndDestroyItem(ref bufferChild);
        RemoveAndDestroyItem(ref activeChild);
    }
}
