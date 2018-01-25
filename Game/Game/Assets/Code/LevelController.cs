﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// This enum is used to control whether the player 
/// is currently building or deleting machines
/// </summary>
public enum BuildMode
{
    /* Below are for use in the debug menu, not 
    available to the player: */

    // (only debugDelete can delete input/output tiles, delete cannot).
    debugdelete = 0,    // 0
    input,              // 1
    output,             // 2

    /* Below are for use by the player */

    // Edit options
    rotate,             // 3
    delete,             // 4
    none,               // 5

    // Machine selections
    conveyer,           // 6
    grinder,            // 7
    brewer,             // 8
    oven,               // 9
    slow_conveyer,      // 10
    rotate_conveyer     // 11
}

/// <summary>
/// This class will act as a controller within individual levels.
/// This class only exists within the Game scene, when the player is inside of a factory level.
/// For a class that is global across the entire game, use the GameManager.
/// </summary>
public class LevelController : MonoBehaviour
{
    bool speedUp = false;
    bool running;
    bool canTick;
    bool hasCorrectPotionHitEnd;
    int tickCounter;
    int levelWidth, levelHeight;
    BuildMode buildingMode;

    [Header("Speed variables")]
    [SerializeField]
    float tickWaitTime;
    [SerializeField]
    float tickWaitTimeSpedUp;

    [Header("Floor tile plane")]
    [SerializeField]
    private GameObject floorTile;

    [Header("Wall tile plane")]
    [SerializeField]
    private GameObject wallTile;

    // UI Controller
    private GameObject UI_Controller;

    /* This is used to remember which inputter the player clicked on, so we know which inputter
    to change the ingredient of when the player taps a new ingredient from the list */
    Inputter selectedInputter; 

    /* This is the factory that we are currently "inside" of. */
    Factory factory;

    /* Singleton instance (This singleton gets destroyed when we leave the scene) */
    private static LevelController instance;

    [Header("Spawnable machines")]
    [SerializeField]
    GameObject[] Spawnables;

    public bool Running { get { return running; } }
    public int TickCounter { get { return tickCounter; } }
    public float TickWaitTime { get { if (speedUp) return tickWaitTimeSpedUp; else return tickWaitTime; } }
    public BuildMode BuildStatus { get { return buildingMode; } }
    public Inputter SelectedInputter { get { return selectedInputter; } set { selectedInputter = value; } }
    /* A getter property for the factory that we are currently inside of. */
    public Factory LevelFactory { get { return factory; } }
    public static LevelController Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        running = false;
        canTick = true;
        speedUp = false;
        tickCounter = 0;
        hasCorrectPotionHitEnd = false;
        // DebugLoadLevel();
    }
	
	void Update ()
    {
        // Debug.Log("Completed Items" + levelFactory.stockpile.ItemCount);

        if (running)
        {
            if (canTick)
            {
                canTick = false;

                StartCoroutine(TickWait(TickWaitTime));
                Run();
            }   
        }
	}

    /// <summary>
    /// This function runs once after the level has been loaded in.
    /// </summary>
    public void OnLevelLoad()
    {
        // Set the build mode to none
        SetBuildMode(BuildMode.none);

        // Move the camera to the appropriate position
        Camera.main.transform.position =
            new Vector3(-.5f, Camera.main.transform.position.y, 0);

        // Set the camera bounds
        Camera.main.GetComponent<OrthoCameraDrag>().UpdateCameraBounds(-2, -1, factory.Width -3, factory.Height -2);

        // Spawn the floor
        SpawnFloorTiles(0, levelWidth, 0, levelHeight);
        SpawnWalls(0, levelWidth, 0, levelHeight);

        // UI Functions
        UI_Controller = GameObject.Find("Canvas");
        UI_Controller.GetComponent<GameCanvas>().BuildUI(factory);
        UI_Controller.GetComponent<GameCanvas>().ToggleEntryPanel();
    }

    private void SpawnFloorTiles(int xMin, int xMax, int zMin, int zMax)
    {
        // How long are our planes
        int planeSize = 10;

        // Calculate new boundary values
        int minX = xMin - planeSize;
        int maxX = xMax + planeSize;
        int minZ = zMin - planeSize;
        int maxZ = zMax + planeSize;
        
        for (int z = minZ; z < maxZ; z += planeSize)
        {
            for (int x = minX; x < maxX; x += planeSize)
            {
                Instantiate(floorTile, new Vector3(x, -1, z), Quaternion.identity);
            }
        }
    }

    private void SpawnWalls(int xMin, int xMax, int zMin, int zMax)
    {
        // How big are our walls
        int wallSize = 10;

        // Calculate our new boundary values
        int minX = xMin - wallSize;
        int maxX = xMax + wallSize;
        int minZ = zMin - wallSize;
        int maxZ = zMax + wallSize;

        // Spawn along the Z axis
        for (int z = minZ; z < maxZ; z += wallSize)
        {
            GameObject wall = Instantiate(wallTile, new Vector3(xMax + 2.5f, 4, z), Quaternion.identity);
            wall.transform.Rotate(-90, 90, 0);
        }

        // Spawn along the X axis
        for (int x = minX; x < maxX; x += wallSize)
        {
            GameObject wall = Instantiate(wallTile, new Vector3(x, 4, zMax + 2.5f), Quaternion.identity);
            wall.transform.Rotate(-90, 0, 0);
        }

    }

    /// <summary>
    /// One call of this function represents one cycle within our production line
    /// </summary>
    private void Run()
    {
        tickCounter++;

        // Tick
        foreach (Machine machine in factory.level.machines)
        {
            machine.Tick();
        }

        // Flush
        foreach (Machine machine in factory.level.machines)
        {
            machine.Flush();
        }

        // Execute
        foreach (Machine machine in factory.level.machines)
        {
            machine.Execute();
        }
    }

    private IEnumerator TickWait(float waitTime)
    {
        canTick = false;
        yield return new WaitForSeconds(waitTime);
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
    /// This method toggles whether or not the production
    /// line is running sped up.
    /// </summary>
    public void ToggleSpeedUp()
    {
        speedUp = !speedUp;
    }

    /// <summary>
    /// Stops the process from running, destroys and removes all items, resets all machines.
    /// </summary>
    private void StopRunning()
    {
        running = false;

        RemoveAndDestroyListOfItems(ref factory.level.items);

        foreach (Machine machine in factory.level.machines)
        {
            machine.Reset();
        }
    }

    /// <summary>
    /// Updates which machine we have currently selected to build
    /// </summary>
    /// <param name="i">array of machines indexer</param>
    public void SetBuildMode(int i)
    {
        BuildMode bm = (BuildMode)i;

        // If the same button is pressed again
        if (bm == buildingMode)
        {
            // Set build mode to none
            buildingMode = BuildMode.none;

            GameObject.Find("Canvas").GetComponent<GameCanvas>().Debug_SetBuildModeText(BuildMode.none);
        }
        else
        {
            buildingMode = bm;
            GameObject.Find("Canvas").GetComponent<GameCanvas>().Debug_SetBuildModeText(bm);
        }
    }

    /// <summary>
    /// Updates the current building mode in the level
    /// </summary>
    /// <param name="bm"></param>
    public void SetBuildMode(BuildMode bm)
    {
        int index = (int)bm;
        SetBuildMode(index);
    }

    /// <summary>
    /// Enables or Disables drag script based on parameter
    /// </summary>
    /// <param name="a">True = enabled, False = disabled</param>
    public void EnableDragScript(bool enab)
    {
        Camera.main.GetComponent<OrthoCameraDrag>().enabled = enab;
    }

    /// <summary>
    /// Spawns the selected machine on a given (x, y) coordinate
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    public void SpawnOn(int x, int y)
    {
        // Return checks
        switch (BuildStatus)
        {
            case BuildMode.delete:
            case BuildMode.rotate:
            case BuildMode.none:
                return;
        }

        // If the tile on this position owns a tile, return
        if (factory.level.grid[x, y].Machine != null) { return; }

        // Instantiate the machine
        Machine machine;

        int spawnIndex;
        switch (BuildStatus)
        {
            case BuildMode.conveyer:
                spawnIndex = 3;
                break;
            case BuildMode.grinder:
                spawnIndex = 4;
                break;
            case BuildMode.brewer:
                spawnIndex = 5;
                break;
            case BuildMode.oven:
                spawnIndex = 6;
                break;
            case BuildMode.slow_conveyer:
                spawnIndex = 7;
                break;
            case BuildMode.rotate_conveyer:
                spawnIndex = 8;
                break;

            /*
                For use with the debug menu, these options 
                are not available to the player:
            */
            case BuildMode.input:
                spawnIndex = 1;
                break;
            case BuildMode.output:
                spawnIndex = 2;
                break;
            default:
                return;
        }

        machine = Instantiate(Spawnables[spawnIndex], new Vector3(x, 0, y), Spawnables[spawnIndex].transform.rotation).GetComponent<Machine>();

        // Set its rotation and parent
        machine.SetDir(Direction.up);
        machine.Parent = factory.level.grid[x, y];

        // Add this machine to the level and to our list of machines
        factory.level.machines.Add(machine);

        // Add this machine as the child of the tile
        factory.level.grid[x, y].SetChild(machine);
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
            return factory.level.grid[x, y + 1].Machine;
        }
        // Right
        else if (facing == Direction.right && x < levelWidth)
        {
            return factory.level.grid[x + 1, y].Machine;
        }
        // Down
        else if (facing == Direction.down && y > 0)
        {
            return factory.level.grid[x, y - 1].Machine;
        }
        // Left
        else if (facing == Direction.left && x > 0)
        {
            return factory.level.grid[x - 1, y].Machine;
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
        factory.level.items.Add(item);
    }

    /// <summary>
    /// Removes an item from the list of items and then destroys it
    /// </summary>
    /// <param name="item"></param>
    public void RemoveAndDestroyItem(ref Item item)
    {
        if (item == null) { return; }

        factory.level.items.Remove(item);
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
        factory.level.machines.Remove(machine);
    }

    /// <summary>
    /// This method loads the level using data from the factory, this includes placing objects in the 
    /// correct positions, etc.
    /// </summary>
    /// <param name="factory"></param>
    public void LoadLevelFromFactory(Factory factory)
    {
        // Remember the current factory we are in
        this.factory = factory;

        // Grab the width and height
        levelWidth = factory.Width;
        levelHeight = factory.Height;

        // Grab file of this level
        LevelToFile ltf = factory.LoadLevelFromFile();

        // Initialize the level
        this.factory.level = new Level();

        // Initialize the array
        this.factory.level.grid = new Tile[levelWidth, levelHeight];

        Transform tileHolder = transform.Find("TileHolder");
        Transform machineHolder = transform.Find("MachineHolder");

        // Loop through our level
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                // Spawn a tile in each position
                Tile tile = Instantiate(Spawnables[0], new Vector3(x, -0.5f, y), Quaternion.identity).GetComponent<Tile>();
                tile.Y = y;
                tile.X = x;
                tile.gameObject.transform.SetParent(tileHolder);


                // Default it to active
                tile.SetActiveStatus(true);

                // Add our tile into our level array
                this.factory.level.grid[x, y] = tile;
            }
        }

        // If our levelFile exists
        if (ltf != null)
        {
            // Loop through each machine and spawn it
            foreach (var machineFromFile in ltf.machines)
            {
                HandleMachine(machineFromFile);
            }

            foreach (var inputFromFile in ltf.inputs)
            {
                HandleInput(inputFromFile);
            }

            // And spawn the output
            foreach (var machine in factory.DefaultMachines)
            {
                if (machine.machineType == MachineType.output)
                {
                    MachineToFile mach = new MachineToFile();

                    mach.dir = machine.dir;
                    mach.type = machine.machineType.ToString();
                    mach.x = machine.x;
                    mach.y = machine.y;

                    HandleMachine(mach);
                }
            }
        }
        // Otherwise, spawn our default machines...
        else
        {
            foreach (var machine in factory.DefaultMachines)
            {
                if (machine.machineType == MachineType.input)
                {
                    InputToFile input = new InputToFile();

                    input.dir = machine.dir;
                    input.type = machine.machineType.ToString();
                    input.x = machine.x;
                    input.y = machine.y;
                    input.ingredient = "";

                    HandleInput(input);
                }
                else
                {
                    MachineToFile mach = new MachineToFile();

                    mach.dir = machine.dir;
                    mach.type = machine.machineType.ToString();
                    mach.x = machine.x;
                    mach.y = machine.y;

                    HandleMachine(mach);
                }


            }
        }

        // Deactivate any tiles that need deactivating
        foreach (Vector2 inactiveTile in factory.InactiveTiles)
        {
            factory.level.grid[(int)inactiveTile.x, (int)inactiveTile.y].SetActiveStatus(false);
        }
    }

    /// <summary>
    /// This checks a given MachineToFile, checks if it needs to spawn and 
    /// if it doesn, sets variables
    /// </summary>
    /// <param name="machineFromFile">the machine data object loaded from json</param>
    private void HandleMachine(MachineToFile machineFromFile)
    {
        Transform machineHolder = transform.Find("MachineHolder");

        Machine mach;

        // Check which type of machine it is and spawn it
        if (machineFromFile.type.Equals("output"))
        {
            mach = Instantiate(Spawnables[2]).GetComponent<Output>();
        }
        else if (machineFromFile.type.Equals("conveyer"))
        {
            mach = Instantiate(Spawnables[3]).GetComponent<Conveyer>();
        }
        else if (machineFromFile.type.Equals("grinder"))
        {
            mach = Instantiate(Spawnables[4]).GetComponent<PestleMortar>();
        }
        else if (machineFromFile.type.Equals("brewer"))
        {
            mach = Instantiate(Spawnables[5]).GetComponent<Brewer>();
        }
        else if (machineFromFile.type.Equals("oven"))
        {
            mach = Instantiate(Spawnables[6]).GetComponent<Oven>();
        }
        else if (machineFromFile.type.Equals("slow_conveyer"))
        {
            mach = Instantiate(Spawnables[7]).GetComponent<Conveyer>();
        }
        else if (machineFromFile.type.Equals("rotate_conveyer"))
        {
            mach = Instantiate(Spawnables[8]).GetComponent<RotatingConveyer>();
        }
        else
        {
            return;
        }

        // Set it as a child to the tile
        factory.level.grid[machineFromFile.x, machineFromFile.y].SetChild(mach);

        // Add it to our list of machines
        factory.level.machines.Add(mach);

        // Update its direction
        mach.SetDir((Direction)machineFromFile.dir);

        // It is a child of our MachineHolder
        mach.gameObject.transform.SetParent(machineHolder);
    }

    /// <summary>
    /// This checks a given InputToFile, checks if it needs to spawn and 
    /// if it doesn, sets variables
    /// </summary>
    /// <param name="InputFromFile">the input data object loaded from json</param>
    private void HandleInput(InputToFile InputFromFile)
    {
        // Create a temporary object to be our inputter
        Inputter inputter = Instantiate(Spawnables[1]).GetComponent<Inputter>();

        // If its an inputter then we need to see what ingredient it inputs to the level

        // use the GetAdditionalInfo method to get information on the inputters ingredient
        var ingredientString = InputFromFile.ingredient;

        if (ingredientString == "")
        {
            inputter.SetOutputItem(null);
        }
        else
        {
            // Loop through each ingredient to find which one this is
            foreach (Ingredient ingredient in GameManager.Instance.Ingredients)
            {
                if (ingredient.DisplayName.Equals(ingredientString))
                {
                    inputter.SetOutputItem(ingredient);
                }
            }
        }

        // Set it as a child to the tile
        factory.level.grid[InputFromFile.x, InputFromFile.y].SetChild(inputter);

        // Add it to our list of machines
        factory.level.machines.Add(inputter);

        // Update its direction
        inputter.SetDir((Direction)InputFromFile.dir);
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

            // Calculate factory score
            uint factoryScore = CalculateFactoryScore(tickCounter);

            /* Give the player a currency reward upon level
            completion or level improvement */
            uint oldScore = factory.Score;
            Debug.Log(string.Format("Previous score: {0}", oldScore));
            Debug.Log(string.Format("new score: {0}", factoryScore));

            /* Give the player however much they improved their score by
            (This will be the full amount if the level was previously unsolved) */
            if (factoryScore > oldScore)
            {
                uint scoreDifference = factoryScore - oldScore;
                Debug.Log(string.Format("New high score! Giving player {0} currency reward", scoreDifference));
                GameManager.Instance.Player.AddPrimaryMoney(scoreDifference);
            }

            // Calculate potions per minute
            float ppm = 0;
            if (tickCounter > 0)
            {
                float timeToMakePotion = tickCounter * tickWaitTime;
                ppm = 60 / timeToMakePotion;
            }

            // Update factory stats
            factory.Score = factoryScore;
            factory.TicksToSolve = tickCounter;
            factory.Solved = true;
            factory.Stars = 0; //Default value for now
            factory.TotalMachineCost = factory.level.CalculateTotalMachineCost;
            factory.PotionsPerMinute = ppm;

            // Debug output
            Debug.Log(string.Format("Total machine cost: {0}", LevelFactory.TotalMachineCost));
            Debug.Log("Score: " + factoryScore);
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
        return factory.level.grid.Cast<Tile>().Count(tile => tile.Machine != null);
    }

    /// <summary>
    /// Calls the factory to save the level to the file
    /// </summary>
    public void SaveLevel()
    {
        factory.SaveLevelToFile(factory.level.grid, levelWidth, levelHeight);
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
        LevelFactory.stockpile.SaveToFile();
        GameManager.Instance.ReturnToOverworld();
    }

}
