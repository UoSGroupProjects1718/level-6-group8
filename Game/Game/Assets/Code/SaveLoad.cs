using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Json
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// A static utilities class which handles all of the saving 
/// and loading to and from json files
/// </summary>
public static class SaveLoad
{
    private static string GetSaveDirectory()
    {
#if UNITY_EDITOR
        // Get the current directory
        StringBuilder currentDir = new StringBuilder(Directory.GetCurrentDirectory());
        currentDir.Append("\\SaveData");
        return currentDir.ToString();

#elif UNITY_ANDROID
        return Application.persistentDataPath;
#endif

    }

    /// <summary>
    /// Saves a factories stats to json
    /// </summary>
    /// <param name="factory"></param>
    public static void SaveFactoryStats(Factory factory)
    {
        // Write all of the informationwe want to store into a FactoryStats object
        FactoryStats fs = new FactoryStats();
        fs.unlocked = factory.Unlocked;
        fs.solved = factory.Solved;
        fs.score = factory.Score;
        fs.ticksToSolve = factory.TicksToSolve;
        fs.stars = factory.Stars;
        fs.potionsPerMinute = factory.PotionsPerMinute;

        // Get the json string
        string json = JsonConvert.SerializeObject(fs);

        // Get dir
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());

        // Append the file name and file extension
        saveDir.Append(string.Format("\\Data_{0}.json", factory.FactoryId));

        // Save to file
        File.WriteAllText(saveDir.ToString(), json);
    }

    /// <summary>
    /// Loads and returns a FactoryStats object containing all the 
    /// stats of a factory
    /// </summary>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static FactoryStats LoadFactoryStats(Factory factory)
    {
        // Get dir
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());
        saveDir.Append(string.Format("\\Data_{0}.json", factory.FactoryId));

        // Check to see if the file exists
        if (!File.Exists(saveDir.ToString()))
        {
            return null;
        }

        // Read from file
        string json = File.ReadAllText(saveDir.ToString());

        // Deserialize the object
        FactoryStats fs = JsonConvert.DeserializeObject<FactoryStats>(json);

        // Return
        return fs;
    }

    /// <summary>
    /// Saves all information about machines in a level to a file
    /// </summary>
    /// <param name="factory">The factory to save the level of</param>
    /// <param name="level">The Tile[,] array of the level</param>
    /// <param name="levelwidth">The width of the level array</param>
    /// <param name="levelheight">The height of the level array</param>
    public static void SaveLevel(Factory factory, Tile[,] level, int levelwidth, int levelheight)
    {
        // Create a new LevelToFile object
        LevelToFile ltf = new LevelToFile();

        /*
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
        */

        // Loop through our level
        for (int x = 0; x < levelwidth; x++)
        {
            for (int y = 0; y < levelheight; y++)
            {

                // Query if the current tile has a machine child
                if (level[x, y].GetComponent<Tile>().Machine != null)
                {
                    // If it does, grab this machine
                    Machine machine = level[x, y].GetComponent<Tile>().Machine;

                    // Query it's type
                    switch (machine.Type)
                    {
                        // If it's a "normal" type:
                        case MachineType.conveyer:
                        case MachineType.mixer:
                        case MachineType.output:
                        case MachineType.pestlemortar:
                        case MachineType.brewer:

                            // Create a MachineToFile object to hold all relevent information
                            MachineToFile mtf = new MachineToFile();
                            mtf.x = machine.Parent.X;
                            mtf.y = machine.Parent.Y;
                            mtf.dir = machine.GetDirection;
                            mtf.type = machine.Type.ToString();

                            // Add this to our LevelToFiles list of machines
                            ltf.machines.Add(mtf);
                            break;

                        // If it's an input:
                        case MachineType.input:

                            // Create an InputToFile object to hold all relevent information
                            InputToFile itf = new InputToFile();
                            itf.x = machine.Parent.X;
                            itf.y = machine.Parent.Y;
                            itf.dir = machine.GetDirection;
                            itf.type = machine.Type.ToString();

                            // Save ingredient as string
                            if (machine.GetComponent<Inputter>().ItemToOutput != null)
                            {
                                itf.ingredient = machine.GetComponent<Inputter>().ItemToOutput.DisplayName;
                            }
                            else
                            {
                                itf.ingredient = "";
                            }

                            // Add this to our LevelToFiles list of inputs
                            ltf.inputs.Add(itf);
                            break;
                    }
                }
            } //end x loop
        } //end y loop

        // Once we have looped through our level it's time to save all of our gathered data to file...

        // Get the json string of our LevelToFile object
        string json = JsonConvert.SerializeObject(ltf);

        // Get the savedir
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());
        // Append filename and file extension
        saveDir.Append(string.Format("\\Level_{0}.json", factory.FactoryId));

        // Save to file
        File.WriteAllText(saveDir.ToString(), json);
    }

    /// <summary>
    /// Loads and returns a LevelToFile object containing information about 
    /// all machines within a level
    /// </summary>
    /// <param name="">The factory to load the level of</param>
    public static LevelToFile LoadLevel(Factory factory)
    {
        // Get the current directory
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());
        // Append filename and file extension
        saveDir.Append(string.Format("\\Level_{0}.json", factory.FactoryId));

        // Check that this file exists
        if (!File.Exists(saveDir.ToString()))
        {
            return null;
        }

        // Read from file
        string json = File.ReadAllText(saveDir.ToString());

        // Deserialize the object
        LevelToFile ltf = JsonConvert.DeserializeObject<LevelToFile>(json);

        // Return
        return ltf;
    }

    /// <summary>
    /// Saves a Stockpiles stats to file
    /// </summary>
    /// <param name="stockpile"></param>
    public static void SaveStockpile(Stockpile stockpile)
    {
        // Make a new StockpileStats object
        StockpileStats ss = new StockpileStats();

        // Copy our stockpile data over
        ss.items = stockpile.Items;
        ss.Value = stockpile.Value;

        // Serialize to json
        string json = JsonConvert.SerializeObject(ss);

        // Get the current directory
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());
        // Append filename and file extension
        saveDir.Append(string.Format("\\Stockpile_{0}.json", stockpile.Factory.FactoryId));

        // Save to file 
        File.WriteAllText(saveDir.ToString(), json);
    }

    public static StockpileStats LoadStockpile(Stockpile stockpile)
    {
        // Get the current directory
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());
        // Append filename and file extension
        saveDir.Append(string.Format("\\Stockpile_{0}.json", stockpile.Factory.FactoryId));

        // Check that this file exists
        if (!File.Exists(saveDir.ToString()))
        {
            return null;
        }

        // Read from file
        string json = File.ReadAllText(saveDir.ToString());

        // Deserialize the object
        StockpileStats ss = JsonConvert.DeserializeObject<StockpileStats>(json);

        // Return
        return ss;
    }

    /// <summary>
    /// Saves a Player objects stats to json file
    /// </summary>
    /// <param name="player"></param>
    public static void SavePlayerStats(Player player)
    {
        // Make a new PlayerStats object
        PlayerStats ps = new PlayerStats();

        // Copy our players data over
        ps.PrimaryMoney = player.PrimaryMoney;
        ps.PremiumMoney = player.PremiumMoney;
        ps.name = player.PlayerName;
        ps.MapSectionsUnlocked = player.MapSectionsUnlocked;

        // Serialize the playerstats to json
        string json = JsonConvert.SerializeObject(ps);

        // Get the saveDir
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());
        // Append filename and file extension
        saveDir.Append("\\Player.json");

        // Save to file 
        File.WriteAllText(saveDir.ToString(), json);
    }

    /// <summary>
    /// Loads PlayerStats from json, returns PlayerStats object with appropriate 
    /// data if file exists, otherwise returns null
    /// </summary>
    /// <returns></returns>
    public static PlayerStats LoadPlayerStats()
    {
        // Get the current directory
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());
        // Append filename and file extension
        saveDir.Append("\\Player.json");

        // Check that this file exists
        if (!File.Exists(saveDir.ToString()))
        {
            Debug.Log("Unable to find player stats file to load from.");
            return null;
        }

        // Read from file
        string json = File.ReadAllText(saveDir.ToString());

        // Deserialize the object
        PlayerStats ps = JsonConvert.DeserializeObject<PlayerStats>(json);

        // Return
        return ps;
    }

    public static void SaveAppCloseTime()
    {
        DateTime now = DateTime.Now;

        string json = JsonConvert.SerializeObject(now);

        // Get the saveDir
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());
        // Append filename and file extension
        saveDir.Append("\\AppCloseDateTime.json");

        // Save to file 
        File.WriteAllText(saveDir.ToString(), json);
    }

    public static DateTime GetAppCloseTime()
    {
        // Get the saveDir
        StringBuilder saveDir = new StringBuilder(GetSaveDirectory());
        // Append filename and file extension
        saveDir.Append("\\AppCloseDateTime.json");

        if (!File.Exists(saveDir.ToString()))
        {
            return default(DateTime);
        }

        // Read from file
        string json = File.ReadAllText(saveDir.ToString());

        DateTime closingTime = JsonConvert.DeserializeObject<DateTime>(json);

        return closingTime; 
    }
}
