﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// This enum is used to control whether the player 
/// is currently building or deleting machines
/// </summary>
public enum BuildStatus
{
    build,
    delete,
    none
}

/// <summary>
/// This class will act as a controller within individual levels.
/// This class only exists within the Game scene, when the player is inside of a factory level.
/// For a class that is global across the entire game, use the GameManager.
/// </summary>
public class LevelController : MonoBehaviour
{
    bool running;
    bool canTick;
    bool hasCorrectPotionHitEnd;
    int tickCounter;
    int levelWidth, levelHeight;
    int currentlySelected;
    float tickWaitTime;
    BuildStatus buildStatus;

    /* This is used to remember which inputter the player clicked on, so we know which inputter
    to change the ingredient of when the player taps a new ingredient from the list */
    Inputter selectedInputter; 

    /* This is the factory that we are currently "inside" of. */
    Factory levelFactory;

    /* This is the current factories level, stored as 2d array of Tiles */
    Tile[,] level;

    /* This is the list of all machines that are currently inside of the factory */
    List<Machine> machines = new List<Machine>();

    /* This is the list of all items currently inside of the factory */
    List<Item> items = new List<Item>();

    [Header("Spawnable machines")]
    [SerializeField]
    GameObject[] Spawnables;

    [Header("Player")]
    [SerializeField]
    Player player;

    public int TickCounter { get { return tickCounter; } }
    public float TickTime { get { return tickWaitTime; } }
    public BuildStatus BuildStatus { get { return buildStatus; } }
    public Player Player { get { return player; } }
    public Inputter SelectedInputter { get { return selectedInputter; } set { selectedInputter = value; } }
    /* A getter property for the factory that we are currently inside of. */
    public Factory LevelFactory { get { return levelFactory; } }
    public List<Item> Items { get { return items; } }

    void Start ()
    {
        running = false;
        canTick = true;
        tickCounter = 0;
        hasCorrectPotionHitEnd = false;
        tickWaitTime = 1.0f;
        currentlySelected = -1;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        tickCounter++;

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

    /// <summary>
    /// This method toggles the production line on/off. 
    /// If toggling off, it takes care of clean up, such as removing all ingredient 
    /// and potions from the machines.
    /// </summary>
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

            // Set build mode to none
            buildStatus = BuildStatus.none;
        }
        // Otherwise, if a new button is pressed
        else
        {
            // This is our selected item
            currentlySelected = i;

            // Build status is now build
            buildStatus = BuildStatus.build;
        }
    }

    /// <summary>
    /// Toggles whether our buildStatus is in delete mode
    /// </summary>
    /// <param name="bs"></param>
    public void ToggleDeleteMode()
    {
        if (buildStatus == BuildStatus.delete)
        {
            buildStatus = BuildStatus.none;
        }
        else
        {
            buildStatus = BuildStatus.delete;
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
        if (buildStatus != BuildStatus.build) { return; }

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
    /// Takes in a machine as refferenceand removes it from 
    /// the list of all machines.
    /// </summary>
    /// <param name="machine"></param>
    public void RemoveMachine(Machine machine)
    {
        machines.Remove(machine);
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
                // If our file doesn't exist, we have to spawn our own inputter
                else
                {
                    // Spawn an inputter on top left corner
                    if (y == levelHeight - 1 && x == levelWidth - 1)
                    {
                        Machine inputter = Instantiate(Spawnables[2]).GetComponent<Machine>();
                        tile.SetChild(inputter);
                        machines.Add(inputter);
                        inputter.SetDir(Direction.down);
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
            foreach (Ingredient ingredient in GameManager.Instance.Ingredients)
            {
                if (ingredient.DisplayName.Equals(ingredientString))
                {
                    inputter.SetOutputItem(ingredient);
                }
            }

            // Set it as a child to the tile
            tile.SetChild(inputter);

            // Add it to our list of machines
            machines.Add(inputter);

            // Update its direction
            inputter.SetDir((Direction)InputFromFile.dir);
        }
    }

    /// <summary>
    /// This method includes any behaviour that occurs when a level has been solved.
    /// </summary>
    public void OnLevelComplete()
    {
        // Only run this behaviour the first time the correct potion hits the output
        if (!hasCorrectPotionHitEnd)
        {
            hasCorrectPotionHitEnd = true;
            Debug.Log(string.Format("You have completed factory: \"{0}\" in {1} ticks by creating: {2}", LevelFactory.FactoryName, tickCounter, LevelFactory.Potion.DisplayName));

            // TODO: My work
            uint factoryScore = CalculateFactoryScore(tickCounter);

            Debug.Log("Score: " + factoryScore);

            levelFactory.Score = factoryScore;
            LevelFactory.TicksToSolve = tickCounter;
            LevelFactory.Solved = true;
            LevelFactory.Stars = 0; //Default value for now
        }
    }

    /// <summary>
    /// Calculates the score of the factory based on the inverse of the tiles occupied
    /// with machines and the ticks taken to complete an action.
    /// </summary>
    /// <returns></returns>
    private uint CalculateFactoryScore(int ticksTaken)
    {
        Debug.Log("Ticks: " + ticksTaken);
        Debug.Log("Tiles taken: " + GetOccupiedTiles());
        const int scoreScale = 100000, tileCountSkew = 1, tickSkew = 1;
        return (uint) Mathf.Floor(scoreScale /
            (tileCountSkew * GetOccupiedTiles() * tickSkew * ticksTaken));
    }

    private int GetOccupiedTiles()
    {
        return level.Cast<Tile>().Count(tile => tile.Machine != null);
    }

    /// <summary>
    /// Calls the factory to save the level to the file
    /// </summary>
    public void SaveLevel()
    {
        levelFactory.SaveLevelToFile(level, levelWidth, levelHeight);
    }

    /// <summary>
    /// This method takes in an Ingredient and sets this as 
    /// the prefab of the currently selected inputter
    /// </summary>
    /// <param name="item">The ingredient item</param>
    public void UpdateSelectedInputtersIngredient(Ingredient item)
    {
        selectedInputter.SetOutputItem(item);
    }

    /// <summary>
    /// Saves the factories stats and then calls the GameManager 
    /// singleton to change Scene to the Overworld
    /// </summary>
    public void LoadOverworld()
    {
        LevelFactory.SaveStatsToFile();
        GameManager.Instance.ReturnToOverworld();
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
