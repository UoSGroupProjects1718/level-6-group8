using System.Collections;
using System.Collections.Generic;
using Assets.Code.Player;
// using NUnit.Framework.Constraints; Disabled for android
using UnityEngine;
using UnityEngine.SocialPlatforms;

// This class contains all player data to be serialized to json
public class PlayerStats
{
    public uint Stars;
    public uint MapSectionsUnlocked;
    public string name;
}

public class Player
{
    public Player()
    {
        Init();
    }

    private uint stars;
    private uint mapSectionsUnlocked;
    private string playerName = "Group 8";
    private readonly PlayerAchievements playerAchievements = new PlayerAchievements();

    // Public properties
//    public uint StarCount
//    {
//        get {
//            uint sc = 0;
//            foreach (TownSection section in Overworld.Instance.TownSections)
//            {
//                foreach (Factory fac in section.Factories)
//                {
//                    sc += fac.Stars;
//                }
//            }
//            return sc;
//        }
//    }
    public uint Stars
    {
        get { return stars; }
    }
    public uint MapSectionsUnlocked { get { return mapSectionsUnlocked; } }
    public string PlayerName { get { return playerName; } set { playerName = value; } }

    public void Init()
    {
        playerAchievements.Init();
        Load();
    }
    
    public void AddStars(uint numberOfStarsToRemove)
    {
        stars += numberOfStarsToRemove;
    }

    public void RemoveStars(uint numberOfStarsToRemove)
    {
        stars -= numberOfStarsToRemove;
    }

    public void UnlockNextMapSection()
    {
        mapSectionsUnlocked++;
    }

    public void Save()
    {
        SaveLoad.SavePlayerStats(this);
    }

    public void Load()
    {
        // Load our playerstats from the json
        PlayerStats ps = SaveLoad.LoadPlayerStats();

        // If it's not null (a save file exists)
        if (ps != null)
        {
            // Set our stats...
            stars = ps.Stars;
            playerName = ps.name;
            mapSectionsUnlocked = ps.MapSectionsUnlocked;

            Debug.Log("Player stats loaded in from file.");
        }
        else
        {
            Debug.Log("No PlayerStats file found, setting players stats to default...");
            stars = 0;
            mapSectionsUnlocked = 0;
        }

        // Update UI
        Overworld.Instance.OverworldCanvas.UpdatePlayerStats(stars);
    }
}
