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
}

public class Factory : MonoBehaviour
{
    public Stockpile stockpile;

    [Header("Visible for debugging.")]
    [SerializeField]
    private bool solved;
    [SerializeField]
    private uint score;
    [SerializeField]
    private uint stars;
    [SerializeField]
    private int ticksToSolve;
    [SerializeField]
    private bool unlocked;

    public bool Solved { get { return solved; } set { solved = value; } }
    public uint Score { get { return score; } set { score = value; } }
    public uint Stars { get { return stars; } set { stars = value; } }
    public int TicksToSolve { get { return ticksToSolve; } set { ticksToSolve = value; } }

    public uint StockpileLimit = 0;

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

    [Header("Potion to make")]
    [SerializeField]
    private Potion potion;

    public int[] ScoreThresholds = new int[3];

    // Level --
    public Level Level;

    public bool Unlocked { get { return unlocked; } }
    public int FactoryId { get { return factoryID; } }
    public int starsToUnlock { get { return starsRequired; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public string FactoryName { get { return factoryName; } }
    public Texture FactorySprite { get { return factorySprite; } }
    public Potion Potion { get { return potion; } }
    public float PotionsPerMinute
    {
        get
        {
            if (ticksToSolve > 0)
            {
                float timeToMakePotion = ticksToSolve * LevelController.Instance.TickWaitTime;
                return 60 / timeToMakePotion;
            }
            return 0;
        }
    }

    void Start()
    {
        LoadStatsFromFile();
        stockpile = new Stockpile(StockpileLimit);
        stockpile.factory = this;
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

    public void UnlockFactory()
    {
        unlocked = true;
    }

    /// <summary>
    /// Saves all relevant factory stats to a json file
    /// </summary>
    public void SaveStatsToFile()
    {
        // Save this to file
        SaveLoad.SaveFactoryStatsToFile(this);
    }

    /// <summary>
    /// Loads all relevent factory stats from its json file
    /// </summary>
    public void LoadStatsFromFile()
    {
        // Load from file and get a FactoryStats object
        FactoryStats fs =  SaveLoad.LoadFactoryStatsFromFile(this);

        // Null check (if the file doesn't exist)
        if (fs == null) { return; }

        // Update our variables
        unlocked = fs.unlocked;
        solved = fs.solved;
        score = fs.score;
        ticksToSolve = fs.ticksToSolve;
        stars = fs.stars;
        Overworld.Instance.AssignFactoryStars();
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
        SaveLoad.SaveLevelToFile(this, level, levelwidth, levelheight);
    }

    /// <summary>
    /// Loads the level data for this factory and returns it in a LevelToFile object
    /// </summary>
    /// <returns>LevelToFile, an class type containing information about the machines in a level</returns>
    public LevelToFile LoadLevelFromFile()
    {
        return (SaveLoad.LoadLevelFromFile(this));
    }


    public void SavePpmToDisk()
    {
        if(Math.Abs(PlayerPrefs.GetFloat(FactoryId + "-PPM") - PotionsPerMinute) > float.Epsilon)
            PlayerPrefs.SetFloat(FactoryId + "-PPM", PotionsPerMinute);
        PlayerPrefs.Save();

    }
}