using System.Collections;
using System.Collections.Generic;
using Assets.Code.Player;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SocialPlatforms;

// This class contains all player data to be serialized to json
public class PlayerStats
{
    public uint money;
    public string name;
}

public class Player
{
    public Player()
    {
        Init();
    }

    // Private members
    private uint money;
    private string playerName = "Group 8";
    private readonly PlayerAchievements playerAchievements = new PlayerAchievements();

    // Public properties
    public uint StarCount
    {
        get {
            uint sc = 0;
            foreach (Factory fac in Overworld.Instance.Factories)
            {
                sc += fac.Stars;
            }
            return sc;
        }
    }
    public uint Money { get { return money; } }
    public string PlayerName { get { return playerName; } set { playerName = value; } }

    
	// Called on Awake by the 
    public void Init()
    {
        playerAchievements.Init();
        playerAchievements.LogAchievementDescriptions();
        playerAchievements.LogPlayerAchievementInfo();

        Load();
    }

    public void AddMoney(uint i)
    {
        money += i;
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
            money = ps.money;
            playerName = ps.name;

            Debug.Log("Player stats loaded in from file.");
        }
        else
        {
            Debug.Log("No PlayerStats file found, setting players stats to default...");
            money = 0;
        }
    }
}
