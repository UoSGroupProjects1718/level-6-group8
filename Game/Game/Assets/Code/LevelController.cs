using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    bool running;
    bool canTick;
    int levelWidth, levelHeight;
    int currentlySelected;
    float tickWaitTime;
    Factory levelFactory;
    Tile[,] level;
    List<Machine> machines = new List<Machine>();
    List<Item> items = new List<Item>();

    [Header("Spawnable machines")]
    [SerializeField]
    GameObject[] Spawnables;

    [Header("Ingredients")]
    [SerializeField]
    Ingredient[] ingredients;

    [Header("Craftable items")]
    [SerializeField]
    CraftableItem[] craftableItems;

    public Item[] Ingredients { get { return ingredients; } }
    public CraftableItem[] CraftableItems { get { return craftableItems; } }
    public List<Item> Items { get { return items; } }

    void Start ()
    {
        running = false;
        canTick = true;
        tickWaitTime = 1.0f;
        currentlySelected = -1;

        // DebugLoadLevel();
	}
	
	void Update ()
    {
		if (running)
        {
            if (canTick)
            {
                canTick = false;
                StartCoroutine(TickWait());
                Run();
            }   
        }
	}

    /// <summary>
    /// One call of this function represents one cycle within our production line
    /// </summary>
    private void Run()
    {
        Debug.Log("Runing...");

        // Tick
        foreach (Machine machine in machines)
        {
            machine.Tick();
        }

        // Flush
        foreach (Machine machine in machines)
        {
            machine.Flush();
        }

        // Execute
        foreach (Machine machine in machines)
        {
            machine.Execute();
        }
    }

    private IEnumerator TickWait()
    {
        canTick = false;
        yield return new WaitForSeconds(tickWaitTime);
        canTick = true;
    }

    public void ToggleRunning()
    {
        if (!running)
        {
            running = true;
        }
        else
        {
            StopRunning();
        }
    }

    /// <summary>
    /// Stops the process from running, destroys and removes all items, resets all machines.
    /// </summary>
    private void StopRunning()
    {
        running = false;

        RemoveAndDestroyListOfItems(ref items);

        foreach (Machine machine in machines)
        {
            machine.Reset();
        }
    }

    /// <summary>
    /// Updates which machine we have currently selected to build
    /// </summary>
    /// <param name="i">array of machines indexer</param>
    public void SetCurrentlySelected(int i)
    {
        // If the same button is pressed again,
        if (currentlySelected == i)
        {
            // Unselect the currently selected
            currentlySelected = -1;
        }
        // Otherwise
        else
        {
            // This is our selected item
            currentlySelected = i;
        }
    }

    /// <summary>
    /// Spawns the selected machine on a given (x, y) coordinate
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    public void SpawnOn(int x, int y)
    {
        // Return checks
        if (currentlySelected == -1) { return; }

        // If the tile on this position owns a tile, return
        if (level[y, x].Machine != null) { return; }

        // Instantiate the machine
        Machine machine = Instantiate(Spawnables[currentlySelected], new Vector3(x, 0, y), Quaternion.identity).GetComponent<Machine>();

        // Set its rotation and parent
        machine.SetDir(Direction.up);
        machine.Parent = level[y, x];

        // Add this machine to the level and to our list of machines
        machines.Add(machine);

        // Add this machine as the child of the tile
        level[y, x].SetChild(machine);
    }

    /// <summary>
    /// Returns the machine neighbouring a given (x,y) coordinate and direction
    /// </summary>
    /// <param name="x">the x pos</param>
    /// <param name="y">the y pos</param>
    /// <param name="facing">the given direction</param>
    /// <returns></returns>
    public Machine GetNeighbour(int x, int y, Direction facing)
    {
        // Up
        if (facing == Direction.up && y < levelHeight)
        {
            return level[y + 1, x].Machine;
        }
        // Right
        else if (facing == Direction.right && x < levelWidth)
        {
            return level[y, x + 1].Machine;
        }
        // Down
        else if (facing == Direction.down && y > 0)
        {
            return level[y - 1, x].Machine;
        }
        // Left
        else if (facing == Direction.left && x > 0)
        {
            return level[y, x - 1].Machine;
        }

        return null;
    }

    /// <summary>
    /// Adds an item to our list of items
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ref Item item)
    {
        if (item == null) { return; }
        items.Add(item);
    }

    /// <summary>
    /// Removes an item from the list of items and then destroys it
    /// </summary>
    /// <param name="item"></param>
    public void RemoveAndDestroyItem(ref Item item)
    {
        if (item == null) { return; }

        items.Remove(item);
        Destroy(item.gameObject);
    }

    /// <summary>
    /// Destroys all gameobjects inside a list of items and then empties the list
    /// </summary>
    /// <param name="itemsToDestroy"></param>
    public void RemoveAndDestroyListOfItems(ref List<Item> itemsToDestroy)
    {
        if (itemsToDestroy.Count == 0) { return; }

        while (itemsToDestroy.Count > 0)
        {
            var item = itemsToDestroy[0];
            itemsToDestroy.Remove(item);
            RemoveAndDestroyItem(ref item);       
        }
    }

    /// <summary>
    /// This method loads the level using data from the factory, this includes placing objects in the 
    /// correct positions, etc.
    /// </summary>
    /// <param name="factory"></param>
    public void LoadLevelFromFactory(Factory factory)
    {
        // Remember the current factory we are in
        levelFactory = factory;

        // Grab the width and height
        levelWidth = factory.Width;
        levelHeight = factory.Height;

        // Initialize the array
        level = new Tile[levelWidth, levelHeight];

        // Grab file of this level
        LevelToFile ltf = factory.LoadLevelFromFile();

        // Loop through our level
        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                // Spawn a tile in each position
                Tile tile = Instantiate(Spawnables[0], new Vector3(x, -0.5f, y), Quaternion.identity).GetComponent<Tile>();
                tile.X = x;
                tile.Y = y;

                // Spawn an inputter on top left corner
                if (y == levelHeight - 1 && x == levelWidth - 1)
                {
                    Machine inputter = Instantiate(Spawnables[2]).GetComponent<Machine>();
                    tile.SetChild(inputter);
                    machines.Add(inputter);
                    inputter.SetDir(Direction.down);
                }
                // Spawn an outputt on (0, 0);
                if (y == 0 && x == 0)
                {
                    Machine output = Instantiate(Spawnables[3]).GetComponent<Machine>();
                    tile.SetChild(output);
                    machines.Add(output);
                }

                // If our levelFile exists
                if (ltf != null)
                {
                    // Loop through each machine to see if we need to spawn it
                    foreach (var machineFromFile in ltf.machines)
                    {
                        HandleMachine(y, x, ref tile, machineFromFile);
                    }

                    foreach (var inputFromFile in ltf.inputs)
                    {
                        HandleInput(y, x, ref tile, inputFromFile);                   
                    }
                }

                // Add our tile into our level array
                level[y, x] = tile;
            }
        }
    }

    /// <summary>
    /// This checks a given MachineToFile, checks if it needs to spawn and 
    /// if it doesn, sets variables
    /// </summary>
    /// <param name="y">y position in the level to check</param>
    /// <param name="x">x posision in the level to check</param>
    /// <param name="tile">the tile the machine will be parent of</param>
    /// <param name="machineFromFile">the machine data object loaded from json</param>
    private void HandleMachine(int y, int x, ref Tile tile, MachineToFile machineFromFile)
    {
        // If it is on this position
        if (machineFromFile.y == y && machineFromFile.x == x)
        {
            // Create the machine
            Machine mach;

            // Check which type of machine it is and spawn it
            if (machineFromFile.type.Equals("conveyer"))
            {
                mach = Instantiate(Spawnables[1]).GetComponent<Conveyer>();
            }
            else if (machineFromFile.type.Equals("output"))
            {
                mach = Instantiate(Spawnables[3]).GetComponent<Output>();
            }
            else if (machineFromFile.type.Equals("mixer"))
            {
                mach = Instantiate(Spawnables[4]).GetComponent<Mixer>();
            }
            else
            {
                return;
            }

            // Set it as a child to the tile
            tile.SetChild(mach);

            // Add it to oru list of machines
            machines.Add(mach);

            // Update its direction
            mach.SetDir((Direction)machineFromFile.dir);
        }
    }

    /// <summary>
    /// This checks a given InputToFile, checks if it needs to spawn and 
    /// if it doesn, sets variables
    /// </summary>
    /// <param name="y">y position in the level to check</param>
    /// <param name="x">x posision in the level to check</param>
    /// <param name="tile">the tile the machine will be parent of</param>
    /// <param name="InputFromFile">the input data object loaded from json</param>
    private void HandleInput(int y, int x, ref Tile tile, InputToFile InputFromFile)
    {
        if (InputFromFile.y == y && InputFromFile.x == x)
        {
            // Create a temporary object to be our inputter
            Inputter inputter = Instantiate(Spawnables[2]).GetComponent<Inputter>();

            // If its an inputter then we need to see what ingredient it inputs to the level

            // use the GetAdditionalInfo method to get information on the inputters ingredient
            var ingredientString = InputFromFile.ingredient;

            // Loop through each ingredient to find which one this is
            foreach (Ingredient ingredient in Ingredients)
            {
                if (ingredient.DisplayName.Equals(ingredientString))
                {
                    inputter.SetOutputItem(ingredient);
                }
            }

            // Set it as a child to the tile
            tile.SetChild(inputter);

            // Add it to oru list of machines
            machines.Add(inputter);

            // Update its direction
            inputter.SetDir((Direction)InputFromFile.dir);
        }
    }

    /// <summary>
    /// Calls the factory to save the level to the file
    /// </summary>
    public void SaveLevel()
    {
        levelFactory.SaveLevelToFile(level, levelWidth, levelHeight);
    }

    public void LoadOverworld()
    {
        GameManager.instance.ReturnToOverworld();
    }

    /// <summary>
    /// Instantiates test level for debugging purposes
    /// </summary>
    private void DebugLoadLevel()
    {
        levelWidth = 10;
        levelHeight = 10;

        level = new Tile[levelWidth, levelHeight];

        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                Tile tile = Instantiate(Spawnables[0], new Vector3(x, -0.5f, y), Quaternion.identity).GetComponent<Tile>();
                tile.X = x;
                tile.Y = y;

                // Spawn an inputter on top left corner
                if (y == levelHeight - 1 && x == levelWidth - 1)
                {
                    Machine inputter = Instantiate(Spawnables[2]).GetComponent<Machine>();
                    tile.SetChild(inputter);
                    machines.Add(inputter);
                    inputter.SetDir(Direction.down);
                }
                // Spawn an outputt on (0, 0);
                if (y == 0 && x == 0)
                {
                    Machine output = Instantiate(Spawnables[3]).GetComponent<Machine>();
                    tile.SetChild(output);
                    machines.Add(output);
                }

                level[y, x] = tile;
            }
        }
    }
}
