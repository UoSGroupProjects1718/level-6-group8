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
    public static GameManager instance = null;

    private Factory currentFactory;

    public Factory CurrentFactory { get { return currentFactory; } }

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
