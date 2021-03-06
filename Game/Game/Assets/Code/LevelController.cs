﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;
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

    /* This is used to remember which inputter the player clicked on, so we know which inputter
    to change the ingredient of when the player taps a new ingredient from the list */
    private Inputter selectedInputter; 

    /* This is the factory that we are currently "inside" of. */
    private Factory factory;

    /* Singleton instance (This singleton gets destroyed when we leave the scene) */
    private static LevelController instance;

    /* The gameObjects that hold the Tiles and Machines respectively.
    This is kept so that we have a refference to the tiles/machines for dimming during tutorials 
    to bring certain machines to the front of the visual hierarchy. */
    private Transform tileHolder;
    private Transform machineHolder;
    private Transform decorationHolder;

    /* A dictionary to remember which potions have and have not yet been 
    created in the level */
    private Dictionary<string /*item name*/, bool /*made*/> potionsCreated = new Dictionary<string, bool>();
    /* An int to remember how many outputs have recieved their desired item */
    private int outputsComplete;
    /* An int to remember how many outputs there are in the level */
    private int numberOfOutputs;

    [Header("Tile")]
    [SerializeField]
    GameObject tilePrefab;

    [Header("Spawnable machines")]
    [SerializeField]
    Machine[] machines;

    public bool Running { get { return running; } }
    public int TickCounter { get { return tickCounter; } }
    public float TickWaitTime { get { if (speedUp) return tickWaitTimeSpedUp; else return tickWaitTime; } }
    public BuildMode BuildStatus { get { return buildingMode; } }
    public Inputter SelectedInputter { get { return selectedInputter; } set { selectedInputter = value; } }
    public Transform TileHolder { get { return tileHolder; } }
    public Transform MachineHolder { get { return machineHolder; } }
    public Transform DecorationHolder { get { return decorationHolder; } }

    //! A getter property for the factory that we are currently inside of.
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

        // Find parent transforms
        tileHolder = transform.Find("TileHolder");
        machineHolder = transform.Find("MachineHolder");
        decorationHolder = transform.Find("DecorationHolder");
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

                Run();
            }   
        }
	}

    /// <summary>
    /// Returns a new machine that matches the given machine type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Machine GetMachine(MachineType type)
    {
        foreach (var machine in machines)
        {
            if (machine.Type == type)
            {
                return machine;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns a new machine that matches the given build mode.
    /// If an invalid build mode is given, null is returned
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Machine GetMachine(BuildMode bm)
    {
        switch (bm)
        {
            case BuildMode.brewer:
                return GetMachine(MachineType.brewer);

            case BuildMode.conveyer:
                return GetMachine(MachineType.conveyer);

            case BuildMode.grinder:
                return GetMachine(MachineType.grinder);

            case BuildMode.input:
                return GetMachine(MachineType.input);

            case BuildMode.output:
                return GetMachine(MachineType.output);

            case BuildMode.oven:
                return GetMachine(MachineType.oven);

            case BuildMode.rotate_conveyer:
                return GetMachine(MachineType.rotate_conveyer);

            case BuildMode.slow_conveyer:
                return GetMachine(MachineType.slow_conveyer);

            default:
                return null;
        }
    }

    /// <summary>
    /// This function runs once after the level has been loaded in.
    /// </summary>
    public void OnLevelLoad()
    {
        // Set the build mode to none
        SetBuildMode(BuildMode.none);

        // Move the camera to the center of the level
        Camera.main.transform.position = new Vector3((factory.Width / 2) - 3, Camera.main.transform.position.y, (factory.Height / 2)-2);

        // Set the camera bounds
        Camera.main.GetComponent<OrthoCameraDrag>().UpdateCameraBounds(-2, -1, factory.Width -3, factory.Height -2);

        // Spawn the floor
        SpawnFloorTiles(0, levelWidth, 0, levelHeight);
        SpawnWalls(0, levelWidth, 0, levelHeight);

        // UI Functions
        GameCanvas.Instance.BuildUI(factory);

        // Event
        EventManager.Instance.AddEvent(EventType.Enter_Factory);

        // Set up the dictionary to remember our potions
        foreach (Item item in factory.Targets)
        {
            // Don't add the same item twice
            if (potionsCreated.ContainsKey(item.DisplayName)) continue;

            potionsCreated.Add(item.DisplayName, false);
        }

        // If tutorial, Progress()
        if (LevelFactory.IsTutorial)
        {
            // Reset the tutorial
            LevelFactory.Tutorial.Reset();

            // Fire an EnterFactory event
            EventManager.Instance.AddEvent(EventType.Enter_Factory);

            GameCanvas.Instance.ToggleLevelUI();
        }
        // Else, show the entry pannel
        else
        {
            GameCanvas.Instance.ToggleEntryPanel();
        }
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
                GameObject tile = Instantiate(floorTile, new Vector3(x, -1, z), Quaternion.identity);
                tile.transform.SetParent(decorationHolder);
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
            wall.transform.SetParent(decorationHolder);
        }

        // Spawn along the X axis
        for (int x = minX; x < maxX; x += wallSize)
        {
            GameObject wall = Instantiate(wallTile, new Vector3(x, 4, zMax + 2.5f), Quaternion.identity);
            wall.transform.Rotate(-90, 0, 0);
            wall.transform.SetParent(decorationHolder);
        }
    }

    /// <summary>
    /// One call of this function represents one cycle within our production line
    /// </summary>
    private void Run()
    {
        tickCounter++;

        // Event
        EventManager.Instance.AddEvent(EventType.Conveyor_Execute);

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

        // Wait for tick time
        StartCoroutine(TickWait(TickWaitTime));

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
            // Hide mayor dialogue
            GameCanvas.Instance.CloseMessage();

            // Start production line
            StartRunning();
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

    public void OutputReceived(Item created)
    {
        // Another output is complete
        outputsComplete++;

        // We have now created this item
        potionsCreated[created.DisplayName] = true;

        // Check if this is every potion
        bool allItems = true;
        foreach (var pair in potionsCreated)
        {
            if (pair.Value == false)
            {
                allItems = false;
                break;
            }
        }

        // If it is, fire off a Level Solved event
        if (allItems && outputsComplete >= numberOfOutputs)
        {
            EventManager.Instance.AddEvent(EventType.Level_Solved);
        }
    }

    /// <summary>
    /// Starts the production line, calls the Begin() function on all machines
    /// </summary>
    private void StartRunning()
    {
        // Reset the ticks
        tickCounter = 0;

        // The production line is now running
        running = true;

        // None of our items have been created (reset)
        var keys = new List<string>(potionsCreated.Keys);
        foreach (var key in keys)
        {
            potionsCreated[key] = false;
        }

        // No outputs have been complete
        outputsComplete = 0;

        // Begin all of our machines
        foreach (Machine machine in factory.level.machines)
        {
            machine.Begin();
        }
    }

    /// <summary>
    /// Stops the process from running, destroys and removes all items, resets all machines.
    /// </summary>
    private void StopRunning()
    {
        // The production line is no longer running
        running = false;

        // Remove all items on the conveyor belts
        RemoveAndDestroyListOfItems(ref factory.level.items);

        // Reset every machine
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

            GameCanvas.Instance.Debug_SetBuildModeText(BuildMode.none);
        }
        else
        {
            buildingMode = bm;
            GameCanvas.Instance.Debug_SetBuildModeText(bm);
            // GameObject.Find("Canvas").GetComponent<GameCanvas>().Debug_SetBuildModeText(bm);
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
        Machine machine = GetMachine(BuildStatus);
        machine = Instantiate(machine, new Vector3(x, y, 0), machine.transform.rotation);

        // Set its rotation and parent
        machine.SetDir(Direction.up);
        machine.Parent = factory.level.grid[x, y];

        // Add this machine to the level and to our list of machines
        factory.level.machines.Add(machine);

        // Add this machine as the child of the tile
        factory.level.grid[x, y].SetChild(machine, true);
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
        if (facing == Direction.up && y < levelHeight -1)
        {
            return factory.level.grid[x, y + 1].Machine;
        }
        // Right
        else if (facing == Direction.right && x < levelWidth -1)
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

        // Loop through our level
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                // Spawn a tile in each position
                Tile tile = Instantiate(tilePrefab, new Vector3(x, -0.5f, y), Quaternion.identity).GetComponent<Tile>();
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
                    // We have another output in the level
                    numberOfOutputs++;

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

                    // We have another output in the level
                    if (machine.machineType == MachineType.output)
                    {
                        numberOfOutputs++;
                    }
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
            mach = Instantiate(GetMachine(MachineType.output)).GetComponent<Output>();
        }
        else if (machineFromFile.type.Equals("conveyer"))
        {
            mach = Instantiate(GetMachine(MachineType.conveyer)).GetComponent<Conveyer>();
        }
        else if (machineFromFile.type.Equals("grinder"))
        {
            mach = Instantiate(GetMachine(MachineType.grinder)).GetComponent<PestleMortar>();
        }
        else if (machineFromFile.type.Equals("brewer"))
        {
            mach = Instantiate(GetMachine(MachineType.brewer)).GetComponent<Brewer>();
        }
        else if (machineFromFile.type.Equals("oven"))
        {
            mach = Instantiate(GetMachine(MachineType.oven)).GetComponent<Oven>();
        }
        else if (machineFromFile.type.Equals("slow_conveyer"))
        {
            mach = Instantiate(GetMachine(MachineType.slow_conveyer)).GetComponent<Conveyer>();
        }
        else if (machineFromFile.type.Equals("rotate_conveyer"))
        {
            mach = Instantiate(GetMachine(MachineType.rotate_conveyer)).GetComponent<RotatingConveyer>();
        }
        else
        {
            return;
        }

        // Set it as a child to the tile
        factory.level.grid[machineFromFile.x, machineFromFile.y].SetChild(mach, false);

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
        Inputter inputter = Instantiate(GetMachine(MachineType.input)).GetComponent<Inputter>();

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
        factory.level.grid[InputFromFile.x, InputFromFile.y].SetChild(inputter, false);

        // Add it to our list of machines
        factory.level.machines.Add(inputter);

        // Update its direction
        inputter.SetDir((Direction)InputFromFile.dir);
    }

    /// <summary>
    /// Dims the textures colour of all Tiles in the level
    /// </summary>
    public void DimTiles()
    {
        for (int i = 0; i < tileHolder.childCount; i++)
        {
            tileHolder.GetChild(i).GetComponent<DimmableObject>().Dim();
        }
    }

    /// <summary>
    /// Brightens the textures colour of all Tiles in the level
    /// </summary>
    public void BrightenTiles()
    {
        for (int i = 0; i < tileHolder.childCount; i++)
        {
            tileHolder.GetChild(i).GetComponent<Tile>().Brighten();
        }
    }

    /// <summary>
    /// Dims the textures colour of all Machines in the level
    /// </summary>
    public void DimMachines()
    {
        for (int i = 0; i < machineHolder.childCount; i++)
        {
            machineHolder.GetChild(i).GetComponent<DimmableObject>().Dim();
        }
    }

    /// <summary>
    /// Brightens the textures colour of all Machines in the level
    /// </summary>
    public void BrightenMachines()
    {
        for (int i = 0; i < machineHolder.childCount; i++)
        {
            machineHolder.GetChild(i).GetComponent<DimmableObject>().Brighten();
        }
    }

    /// <summary>
    /// Dims all decoration objects within the Level scene
    /// </summary>
    public void DimFactory()
    {
        for (int i = 0; i < decorationHolder.childCount; i++)
        {
            decorationHolder.GetChild(i).GetComponent<DimmableObject>().Dim();
        }
    }

    /// <summary>
    /// Brightens all decoration objects within the Level scene
    /// </summary>
    public void BrightenFactory()
    {
        for (int i = 0; i < decorationHolder.childCount; i++)
        {
            decorationHolder.GetChild(i).GetComponent<DimmableObject>().Brighten();
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
            // We have completed the factory
            hasCorrectPotionHitEnd = true;
            Debug.Log(string.Format("You have completed factory: \"{0}\" in {1} ticks.", LevelFactory.FactoryName, tickCounter));

            // Return factory to its original speed
            if (speedUp)
                ToggleSpeedUp();

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
                uint starDifference = factory.CalculateStarsFromScore(factoryScore) -
                                      factory.CalculateStarsFromScore(oldScore);
                Debug.Log("New high score!");
                Debug.Log(string.Format("DEPRECATED: Giving player {0} currency reward", scoreDifference));
                if (AuthServices.isSignedIn)
                {
                    var user = FirebaseAuth.DefaultInstance.CurrentUser;
                    DBManager dbm = new DBManager();
                    dbm.WriteScore(factoryScore, factory.FactoryId, user);
                }

                GameManager.Instance.Player.AddStars(starDifference);
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

            // Display the  Final Score Screen
            GameCanvas.Instance.EnableScoreScreen((int)factoryScore, TickCounter);

            EventManager.Instance.AddEvent(EventType.Level_Solved);
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
    /// unless its a tutorial
    /// </summary>
    public void SaveLevel()
    {
        // Don't save if it's a tutorial level
        if (!LevelFactory.IsTutorial)
        {
            factory.SaveLevelToFile(factory.level.grid, levelWidth, levelHeight);
        }
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