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
    Ingredient[] ingredients;

    [Header("Craftable items")]
    [SerializeField]
    CraftableItem[] craftableItems;

    [Header("Waste")]
    [SerializeField]
    Waste waste;

    public static GameManager Instance { get { return instance; } }
    public Factory CurrentFactory { get { return currentFactory; } }
    public Waste Waste { get { return waste; } }
    public Item[] Ingredients { get { return ingredients; } }
    public CraftableItem[] CraftableItems { get { return craftableItems; } }

    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
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

    void Start()
    {
        // At the start of the game, load factory stats from file
        StartCoroutine(LoadAllFactoryStatsFromFile());
    }

    private IEnumerator LoadAllFactoryStatsFromFile()
    {
        /*  We wait for 0.25 seconds here because this will be called
            when changing scenes. This gives time to wait for the scene
            to finish loading. */
        yield return new WaitForSeconds(0.25f);

        foreach (var fac in Overworld.Instance.Factories)
        {
            fac.GetComponent<Factory>().LoadStatsFromFile();
        }
    }


    public void SetFactory(Factory factory)
    {
        currentFactory = factory;
    }

    /// <summary>
    /// Changes back to the Overworld scene
    /// </summary>
    public void ReturnToOverworld()
    {
        SceneManager.LoadScene(0);
        StartCoroutine(LoadAllFactoryStatsFromFile());
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
    }
}
