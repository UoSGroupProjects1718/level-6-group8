using System.Collections;
using System.Collections.Generic;
using Assets.Code.Player;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Player : MonoBehaviour
{
    // Need a singleton so that money remains after scene changes and no duplicate objects are created
    // due to missing references
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private string playerName = "Test";
    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    private float money;
    public float Money
    {
        get { return money; }
        set { money = value < 0 ? 0 : value; }
    }

    private int moneyPerSecond = 5;
    public int MoneyPerSecond
    {
        get { return moneyPerSecond; }
        set { moneyPerSecond = value; }
    }

    private List<Factory> lockedFactories;
    public List<Factory> LockedFactories
    {
        get { return lockedFactories; }
        set { lockedFactories = value; }
    }

    private List<Factory> unlockedFactories;
    public List<Factory> UnlockedFactories
    {
        get { return unlockedFactories; }
        set { unlockedFactories = value; }
    }

    private readonly PlayerAchievements playerAchievements = new PlayerAchievements();

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(GainPassiveMoney());
        playerAchievements.Init();
        playerAchievements.LogAchievementDescriptions();
        playerAchievements.LogPlayerAchievementInfo();
    }

    // Update is called once per frame
    void Update () {
	}

    private IEnumerator GainPassiveMoney()
    {
        while (true)
        {
            // Debug.Log(string.Format("Player money: {0}", money));
            money += moneyPerSecond;
            yield return new WaitForSeconds(1);
        }
    }
}
