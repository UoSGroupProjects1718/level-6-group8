using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is a singleton class, it will be alive for the entire duration
/// of the game. It will remember data that needs to be carried across
/// scenes.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance = null;

    private Factory currentFactory;

    [Header("Ingredients")]
    [SerializeField]
    private Ingredient[] ingredients;

    [Header("Potions")]
    [SerializeField]
    private Potion[] potions;

    [Header("Compound")]
    [SerializeField]
    private Compound compound;

    [Header("Waste")]
    [SerializeField]
    private Waste waste;

    [Header("Burnt ingredient")]
    [SerializeField]
    private Ingredient burntIngredient;

    /* The player class object to hold all of the players currency, stats, achievements, etc */
    private Player player;

    public static GameManager Instance { get { return instance; } }
    public Factory CurrentFactory { get { return currentFactory; } }
    public Waste Waste { get { return waste; } }
    public Compound Compound { get { return compound; } }
    public Player Player { get { return player; } }
    public Ingredient BurntIngredient { get { return burntIngredient; } }
    public Item[] Ingredients { get { return ingredients; } }
    public CraftableItem[] Potions { get { return potions; } }

    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;

            // Initialize everything that is required at the opening of the game...
            StartCoroutine(WaitInit());
        }

        // If instance already exists and it's not this:
        else if (instance != this)
        {
            // then destroy this.
            // This enforces our singleton meaning that there can only be one instance of GameController
            Destroy(gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    private IEnumerator WaitInit()
    {
        /*
            We wait here for .2 seconds just to give enough time 
            for everything else to load, such as the Overworld 
            Singleton instance.
        */
        yield return new WaitForSeconds(0.2f);
        Init();
    }

    /// <summary>
    /// This method is called once and only once at the very start of the game 
    /// when the singleton gets created
    /// </summary>
    private void Init()
    {
        // Load our player
        /*
            The Player constructor calls an InitPlayer() method which takes care of 
            loading the player stats in from file and loading the achievements, etc. 
        */
        player = new Player();

        // At the start of the game, load factory stats from file
        LoadAllFactoryStatsFromFile();

        // Calculate offline income...
        CalculateOfflineIncome();

        // By default, factory 0 will be unlocked
        GetFacoryByID(0).UnlockFactory();
    }

    void Start()
    {
        // At the start of the game, load factory stats from file
        LoadAllFactoryStatsFromFile();
    }

    /// <summary>
    /// This function is called once and only once at the beginning of the application
    /// It calculates how much income you earned whilst you were offline
    /// </summary>
    private void CalculateOfflineIncome()
    {
        /* Calculate offline income here... */
        var timespan = GetTimespanSinceLastClose();

        // If the app hasn't been opened before it returns MinValue
        if (timespan == TimeSpan.MinValue)
        {
            return;
        }

        foreach (Factory factory in Overworld.Instance.Factories)
        {
            var ppm = factory.PotionsPerMinute;

            // You can do the rest
            var potionsGainedWhileOffline = Math.Floor(ppm * timespan.TotalMinutes);

            // Access the factories stockpile this like
            // Increment the number of potions the factory has by potionsGainedWhileOffline
            Debug.Log(string.Format("Trying to add {0} potions to {1}.", potionsGainedWhileOffline, factory.FactoryName));
            factory.stockpile.AddOrIncrement(factory.Potion, (uint)potionsGainedWhileOffline);
        }
    }

    public TimeSpan GetTimespanSinceLastClose()
    {
        // What time is it now?
        System.DateTime timeAppOpen = System.DateTime.Now;

        // What time was it when the application was closed?
        System.DateTime timeAppClose = SaveLoad.GetAppCloseTime();

        // If the app hasn't been opened before, return
        if (timeAppClose == DateTime.MinValue)
        {
            return TimeSpan.MinValue;
        }

        // Debug
        Debug.Log(string.Format("You last closed the app on: {0}", timeAppClose));
        Debug.Log(string.Format("You have now opened the app on: {0}", timeAppOpen));

        // How long was it closed for?
        System.TimeSpan difference = timeAppOpen - timeAppClose;
        Debug.Log(string.Format("The app was closed for (h:m:s): {0}", difference));

        return difference;
    }

    private IEnumerator WaitAndLoadAllFactoryStatsFromFile()
    {
        /*  We wait for 0.25 seconds here because this will be called
            when changing scenes. This gives time to wait for the scene
            to finish loading before we start loading from file.*/
        yield return new WaitForSeconds(0.25f);
        LoadAllFactoryStatsFromFile();
    }

    private void LoadAllFactoryStatsFromFile()
    {
        foreach (var fac in Overworld.Instance.Factories)
        {
            fac.GetComponent<Factory>().LoadStatsFromFile();
        }
    }


    public void SetFactory(Factory factory)
    {
        currentFactory = factory;
    }

    public Factory GetFacoryByID(int id)
    {
        foreach (var factory in Overworld.Instance.Factories)
        {
            if (factory.FactoryId == id) { return factory; }
        }
        return null;
    }

    /// <summary>
    /// Changes back to the Overworld scene
    /// </summary>
    public void ReturnToOverworld()
    {
        SceneManager.LoadScene(0);
        StartCoroutine(WaitAndLoadAllFactoryStatsFromFile());
    }

    /// <summary>
    /// Calls the LoadLevelCouroutine with the given factory argument
    /// </summary>
    /// <param name="factory">The factory to load</param>
    public void LoadLevel(Factory factory)
    {
        StartCoroutine(LoadLevelCoroutine(factory));
    }

    /// <summary>
    /// Changes to the Level scene, loads the given factory
    /// </summary>
    /// <param name="factory">The factory to load</param>
    IEnumerator LoadLevelCoroutine(Factory factory)
    {
        // Load the scene
        SceneManager.LoadScene(1);

        // Wait a second for everything to load
        yield return new WaitForSeconds(0.25f);

        // Find the level controller and load the factory
        GameObject.Find("LevelController").GetComponent<LevelController>().LoadLevelFromFactory(factory);

        // Call the level controllers OnLevelLoad function
        GameObject.Find("LevelController").GetComponent<LevelController>().OnLevelLoad();
    }

    /// <summary>
    /// All behaviour that needs to be done upon exiting the app...
    /// </summary>
    private void OnApplicationQuit()
    {
        Debug.Log("Saving player stats to file...");
        player.Save();

        Debug.Log("Saving current DateTime to file...");
        SaveLoad.SaveAppCloseTime();
    }
}
