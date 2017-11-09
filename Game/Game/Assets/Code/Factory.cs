using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Json
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// This class is used to serialize and deserialize factory data to/from json
/// </summary>
class FactoryStats
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

    [Header("Factory sprite")]
    [SerializeField]
    private Sprite factorySprite;

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
    public Sprite FactorySprite { get { return factorySprite; } }
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
            GameObject.Find("Canvas").GetComponent<OverworldCanvas>().DisplayFactory(this);
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
        // Write all of the information we want to store into a FactoryStats object
        FactoryStats fs = new FactoryStats();
        fs.unlocked = IsUnlocked;
        fs.completed = Completed;
        fs.efficiency = FactoryEfficiency;

        // Get the json string
        string json = JsonConvert.SerializeObject(fs);

        // Get the current directory
        var currentDir = Directory.GetCurrentDirectory();
        StringBuilder sb = new StringBuilder(currentDir);

        // Add on the file name
        sb.Append(string.Format("\\{0}.json", FactoryName));

        // Debug.Log(sb.ToString());

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
        // Create a new LevelToFile object
        LevelToFile ltf = new LevelToFile();

        // Loop through our level
        for (int y = 0; y < levelheight; y++)
        {
            for (int x = 0; x < levelwidth; x++)
            {

                // Query if the current tile has a machine child
                if (level[y, x].GetComponent<Tile>().Machine != null)
                {
                    // If it does, grab this machine
                    Machine machine = level[y, x].GetComponent<Tile>().Machine;

                    // Query its type
                    switch (machine.Type)
                    {

                        // If its a conveyer, mixer or output
                        case MachineType.conveyer:
                        case MachineType.mixer:
                        case MachineType.output:

                            // Create a MachineToFile object to hold all relevent information
                            MachineToFile mtf = new MachineToFile();
                            mtf.x = machine.Parent.X;
                            mtf.y = machine.Parent.Y;   
                            mtf.dir = machine.GetDirection;
                            mtf.type = machine.Type.ToString();

                            // Add this to our LevelToFiles list of machines
                            ltf.machines.Add(mtf);
                            break;

                        // If its an input:
                        case MachineType.input:

                            // Create an inputToFile object to hold all relevent information
                            InputToFile itf = new InputToFile();
                            itf.x = machine.Parent.X;
                            itf.y = machine.Parent.Y;
                            itf.dir = machine.GetDirection;
                            itf.type = machine.Type.ToString();

                            if (machine.GetComponent<Inputter>().ItemToOutput != null)
                            {
                                itf.ingredient = machine.GetComponent<Inputter>().ItemToOutput.DisplayName;
                            }
                            else
                            {
                                itf.ingredient = "";
                            }
                            
                            // Add this to our LevelToFiles list of machines
                            ltf.inputs.Add(itf);
                            break;
                    }
                }
            }
        }

        // Once we have looped through our level it's time to save all of our gathered data to file...

        // Get the json string of our LevelToFile object
        string json = JsonConvert.SerializeObject(ltf);

        // Get the current directory
        var currentDir = Directory.GetCurrentDirectory();
        StringBuilder sb = new StringBuilder(currentDir);

        // Add on the file name
        sb.Append(string.Format("\\Level_{0}.json", FactoryName));

        // Save to file
        System.IO.File.WriteAllText(sb.ToString(), json);
    }

    /// <summary>
    /// Loads the level data for this factory and returns it in a LevelToFile object
    /// </summary>
    /// <returns>LevelToFile, an class type containing information about the machines in a level</returns>
    public LevelToFile LoadLevelFromFile()
    {
        // Get the current directory
        var currentDir = Directory.GetCurrentDirectory();
        StringBuilder sb = new StringBuilder(currentDir);

        // Add on the file name
        sb.Append(string.Format("\\Level_{0}.json", FactoryName));

        // Check the file exists
        if (!File.Exists(sb.ToString()))
        {
            return null;
        }

        // Read from file
        string json = System.IO.File.ReadAllText(sb.ToString());

        // Deserialize the object
        LevelToFile ltf = JsonConvert.DeserializeObject<LevelToFile>(json);

        // Update our variables
        return ltf;
    }
}
