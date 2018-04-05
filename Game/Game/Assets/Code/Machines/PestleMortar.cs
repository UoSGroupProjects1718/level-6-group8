using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestleMortar : Machine
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

    // PestleMortar does not implement custom MachinePress functionality
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

        // Pass item
        if (neighbour.CanReceiveFrom(this))
        {
            neighbour.Receive(ref createdItem);
            createdItem = null;
        }
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

        createdItem = CreateCompound();

        if (createdItem != null)
        {
            AddItem(ref createdItem);

            // Event
            EventManager.Instance.AddEvent(EventType.Grinder_Execute);
        }
    }

    private Compound CreateCompound()
    {
        List<string> allIngredients = new List<string>();

        // Check all of our active chilren;
        foreach (Item item in activeChildren)
        {
            // Anything other than raw ingredients will "ruin" the output
            if (item.ItemType != ItemType.rawIngredient)
            {
                
                return null;
            }

            allIngredients.Add(item.DisplayName);
        }

        // Output a compound containing the our active children

        // Instantiate the compound
        Compound compound = Instantiate(GameManager.Instance.Compound).GetComponent<Compound>();

        // Give it its components
        compound.SetComponents(allIngredients);

        // Posotion, rotate and scale the item
        compound.transform.position = new Vector3(transform.position.x, compound.ProductionLine_YHeight, transform.position.z);
        compound.transform.localRotation = Quaternion.Euler(compound.ProductionLine_Rotation);
        compound.transform.localScale = (compound.ProductionLine_Scale);

        // return it
        return compound;
    }

    public override bool CanReceiveFrom(Machine from)
    {
        switch (dir)
        {
            case Direction.up:
            case Direction.down:

                if (from.Parent.X == parent.X - 1 || from.Parent.X == parent.X + 1) return true;
                return false;

            case Direction.left:
            case Direction.right:
                if (from.Parent.Y == parent.Y + 1 || from.Parent.Y == parent.Y - 1) return true;
                return false;

            default:
                return true;
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
