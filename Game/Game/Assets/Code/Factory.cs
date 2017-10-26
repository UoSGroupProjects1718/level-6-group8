using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Json
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class FactoryStats
{
    public bool unlocked;
    public float efficiency;
}

public class Factory : MonoBehaviour
{
    [SerializeField]
    private bool unlocked;
    private float efficiency;

    [Header("Unlock level")]
    [SerializeField]
    private int levelToUnlock;

    [Header("Factory name")]
    [SerializeField]
    private string factoryName;

    [Header("Factory sprite")]
    [SerializeField]
    private Sprite factorySprite;

    [Header("Factory size")]
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    public bool IsUnlocked { get { return unlocked; } }
    public int LevelToUnlock { get { return levelToUnlock; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public float FactoryEfficiency { get { return efficiency; } }
    public string FactoryName { get { return factoryName; } }
    public Sprite FactorySprite { get { return factorySprite; } }

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
            GameObject.Find("Canvas").GetComponent<OverworldCanvas>().DisplayFactory(this);
        }
    }

    public void UnlockFactory()
    {
        unlocked = true;
    }

    public void CalculateEfficiency()
    {

    }

    public void LoadInternalFactory()
    {

    }

    /// <summary>
    /// Saves all relevant factory stats to a json file
    /// </summary>
    public void SaveStatsToFile()
    {
        // Write all of the information we want to store into a FactoryStats object
        FactoryStats fs = new FactoryStats();
        fs.unlocked = IsUnlocked;
        fs.efficiency = FactoryEfficiency;

        // Get the json string
        string json = JsonConvert.SerializeObject(fs);

        // Get the current directory
        var currentDir = Directory.GetCurrentDirectory();
        StringBuilder sb = new StringBuilder(currentDir);

        // Add on the file name
        sb.Append(string.Format("\\{0}.json", FactoryName));

        Debug.Log(sb.ToString());

        // Save to file
        System.IO.File.WriteAllText(sb.ToString(), json);
    }

    /// <summary>
    /// Loads all relevent factory stats from its json file
    /// </summary>
    public void LoadStatsFromFile()
    {
        // Get the current directory
        var currentDir = Directory.GetCurrentDirectory();
        StringBuilder sb = new StringBuilder(currentDir);

        // Add on the file name
        sb.Append(string.Format("\\{0}.json", FactoryName));

        // Check the file exists
        if (!File.Exists(sb.ToString()))
        {
            return;
        }

        // Read from file
        string json = System.IO.File.ReadAllText(sb.ToString());

        // Deserialize the object
        FactoryStats fs = JsonConvert.DeserializeObject<FactoryStats>(json);

        // Update our variables
        unlocked = fs.unlocked;
        efficiency = fs.efficiency;
    }

    public void SaveLevelToFile()
    {

    }
}
