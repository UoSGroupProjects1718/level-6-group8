using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class is used to serialize and deserialize factory data to/from json
/// </summary>
public class FactoryStats
{
    public bool unlocked;
    public bool completed;
    public float efficiency;
}

public class Factory : MonoBehaviour
{
    [SerializeField]
    private bool unlocked;
    [SerializeField]
    private bool completed;
    private float efficiency;

    [Header("Unlock level")]
    [SerializeField]
    private int levelToUnlock;

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

    public bool Completed { get { return completed; } }
    public bool IsUnlocked { get { return unlocked; } }
    public int LevelToUnlock { get { return levelToUnlock; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public float FactoryEfficiency { get { return efficiency; } }
    public string FactoryName { get { return factoryName; } }
    public Texture FactorySprite { get { return factorySprite; } }
    public Potion Potion { get { return potion; } }

    void Start()
    {
        LoadStatsFromFile();
    }

    private void OnMouseDown()
    {
        // This check makes sure we didn't simply click on UI ontop of the game object
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // Set this as the currently-selected factory
            GameManager.instance.SetFactory(this);

            // Update the canvas to open a pannel with this factories stats
            GameObject.Find("Canvas_ScreenSpace").GetComponent<OverworldCanvas>().DisplayFactory(this);
        }
    }

    public void UnlockFactory()
    {
        unlocked = true;
    }

    public void SetAsComplete()
    {
        completed = true;
    }

    public void CalculateEfficiency()
    {

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
        completed = fs.completed;
        efficiency = fs.efficiency;
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
}