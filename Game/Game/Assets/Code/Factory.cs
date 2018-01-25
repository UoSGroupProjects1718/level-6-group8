using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

/// <summary>
/// This class is used to serialize and deserialize factory data to/from json
/// </summary>
public class FactoryStats
{
    public bool unlocked;
    public bool solved;
    public int ticksToSolve;
    public uint score;
    public uint stars;
    public float potionsPerMinute;
}

/// <summary>
/// The Factory class will have an array of type DefaultMachines.
/// Each object of this class holds data about which machine the 
/// factory has by default, its x,y position and its direction.
/// For example, a Factory will have an output on (10, 10).
/// </summary>
[Serializable]
public class DefaultMachine
{
    public MachineType machineType;
    public int x;
    public int y;
    public Direction dir;
}

public class Factory : MonoBehaviour
{
    private bool solved;   
    private bool unlocked;
    private uint score;
    private uint stars;
    private int ticksToSolve;
    private int totalMachineCost;
    private float potionsPerMinute;

    public bool Solved { get { return solved; } set { solved = value; } }
    public uint Score { get { return score; } set { score = value; } }
    public uint Stars { get { return stars; } set { stars = value; } }
    public int TicksToSolve { get { return ticksToSolve; } set { ticksToSolve = value; } }
    public int TotalMachineCost { get { return totalMachineCost; } set { totalMachineCost = value; } }
    public float PotionsPerMinute { get { return potionsPerMinute; } set { potionsPerMinute = value; } }

    [Header("Factory ID")]
    [SerializeField]
    private int factoryID;

    [Header("Unlock Stars")]
    [SerializeField]
    private int starsRequired;

    [Header("Factory name")]
    [SerializeField]
    private string factoryName;

    [Header("Factory Texture")]
    [SerializeField]
    private Texture factorySprite;

    [Header("Factory size")]
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    [Header("Default Machines")]
    [SerializeField]
    private DefaultMachine[] defaultMachines;

    [Header("Inactive tiles")]
    [SerializeField]
    private Vector2[] inactiveTiles;

    [Header("Potion to make")]
    [SerializeField]
    private Potion potion;

    [Header("Stockpile limit")]
    [SerializeField]
    private uint stockpileLimit;

    [Header("Score Thresholds")]
    [SerializeField]
    private int[] scoreThresholds;

    // Non-GameObject class variables
    public Stockpile stockpile;
    public Level level;

    public bool Unlocked { get { return unlocked; } }
    public int FactoryId { get { return factoryID; } }
    public int starsToUnlock { get { return starsRequired; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public uint StockpileLimit { get { return stockpileLimit; } }
    public int[] ScoreThresholds { get { return scoreThresholds; } }
    public string FactoryName { get { return factoryName; } }
    public Texture FactorySprite { get { return factorySprite; } }
    public Potion Potion { get { return potion; } }
    public DefaultMachine[] DefaultMachines { get { return defaultMachines; } }
    public Vector2[] InactiveTiles { get { return inactiveTiles; } }

    void Start()
    {
        // Load factory stats from file
        LoadStatsFromFile();

        // Create our stockpile object
        stockpile = new Stockpile(stockpileLimit, this);

        // Load stockpile stats from file
        stockpile.LoadFromFile();
    }

    private void OnMouseDown()
    {
        // This check makes sure we didn't simply click on UI ontop of the game object
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // Set this as the currently-selected factory
            GameManager.Instance.SetFactory(this);

            // Update the canvas to open a pannel with this factories stats
            GameObject.Find("Canvas_ScreenSpace").GetComponent<OverworldCanvas>().DisplayFactory(this);
        }
    }

    public void Unlock()
    {
        unlocked = true;
    }

    /// <summary>
    /// Saves all relevant factory stats to a json file
    /// </summary>
    public void SaveStatsToFile()
    {
        // Save this to file
        SaveLoad.SaveFactoryStats(this);
    }

    /// <summary>
    /// Loads all relevent factory stats from its json file
    /// </summary>
    public void LoadStatsFromFile()
    {
        // Load from file and get a FactoryStats object
        FactoryStats fs =  SaveLoad.LoadFactoryStats(this);

        // Null check (if the file doesn't exist)
        if (fs == null)
        {
            // Then everything defaults...

            unlocked = false;
            solved = false;
            score = 0;
            ticksToSolve = 0;
            Stars = 0;
            potionsPerMinute = 0;
            Overworld.Instance.AssignFactoryStars(factoryID);

            return;
        }

        // Update our variables
        unlocked = fs.unlocked;
        solved = fs.solved;
        score = fs.score;
        ticksToSolve = fs.ticksToSolve;
        stars = fs.stars;
        potionsPerMinute = fs.potionsPerMinute;

        // Assign factory stars
        Overworld.Instance.AssignFactoryStars(FactoryId);
    }

    /// <summary>
    /// This method will save the information for all machines in a level to a file.
    /// </summary>
    /// <param name="level">The array of the level to save</param>
    /// <param name="levelwidth">The x width of the level</param>
    /// <param name="levelheight">The y height of the level</param>
    public void SaveLevelToFile(Tile[,] level, int levelwidth, int levelheight)
    {
        // Save this level to file
        SaveLoad.SaveLevel(this, level, levelwidth, levelheight);
    }

    /// <summary>
    /// Loads the level data for this factory and returns it in a LevelToFile object
    /// </summary>
    /// <returns>LevelToFile, an class type containing information about the machines in a level</returns>
    public LevelToFile LoadLevelFromFile()
    {
        return (SaveLoad.LoadLevel(this));
    }
}