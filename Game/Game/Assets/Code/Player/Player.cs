using System.Collections;
using System.Collections.Generic;
using Assets.Code.Player;
// using NUnit.Framework.Constraints; Disabled for android
using UnityEngine;
using UnityEngine.SocialPlatforms;

// This class contains all player data to be serialized to json
public class PlayerStats
{
    public uint PrimaryMoney;
    public uint PremiumMoney;
    public string name;
}

public class Player
{
    public Player()
    {
        Init();
    }

    // Private members
    private uint primaryMoney;
    private uint premiumMoney;
    private string playerName = "Group 8";
    private readonly PlayerAchievements playerAchievements = new PlayerAchievements();

    // Public properties
    public uint StarCount
    {
        get {
            uint sc = 0;
            foreach (TownSection section in Overworld.Instance.TownSections)
            {
                foreach (Factory fac in section.Factories)
                {
                    sc += fac.Stars;
                }
            }
            return sc;
        }
    }
    public uint PrimaryMoney { get { return primaryMoney; } }
    public uint PremiumMoney { get { return premiumMoney; } }
    public string PlayerName { get { return playerName; } set { playerName = value; } }

    public void Init()
    {
        playerAchievements.Init();

        Load();
    }

    public void AddPrimaryMoney(uint i)
    {
        primaryMoney += i;
    }

    public void RemovePrimaryMoney(uint i)
    {
        primaryMoney -= i;
    }

    public void AddPremiumMoney(uint i)
    {
        premiumMoney += i;
    }

    public void RemovePremiumMoney(uint i)
    {
        premiumMoney -= i;
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
            primaryMoney = ps.PrimaryMoney;
            premiumMoney = ps.PremiumMoney;
            playerName = ps.name;

            Debug.Log("Player stats loaded in from file.");
        }
        else
        {
            Debug.Log("No PlayerStats file found, setting players stats to default...");
            primaryMoney = 0;
            premiumMoney = 0;
        }
    }
}
