using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for handling behaviour that 
/// occurs within the Overworld scene.
/// </summary>
public class Overworld : MonoBehaviour
{
    private static Overworld instance = null;

    [Header("Factories")]
    [SerializeField]
    Factory[] factories;

    public static Overworld Instance { get { return instance; } }
    public Factory[] Factories { get { return factories; } }

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Saves all factories stats and then calls the GameManager to change 
    /// to the level scene and loads the game managers current factory
    /// </summary>
    public void LoadFactory()
    {
        // Save all factory data to file
        foreach (Factory factory in factories)
        {
            factory.SaveStatsToFile();
        }

        // Call game manager to handle loading the level
        GameManager.Instance.LoadLevel(GameManager.Instance.CurrentFactory);
    }

    /// <summary>
    /// Unlocks a factory if all requirements are met.
    /// </summary>
    public void PurchaseFactory()
    {
        if (int.MaxValue /* player.level*/ >= GameManager.Instance.CurrentFactory.starsToUnlock)
        {
            // Unlock the factory
            GameManager.Instance.CurrentFactory.UnlockFactory();

            // Save the factory to file
            GameManager.Instance.CurrentFactory.SaveStatsToFile();

            GameObject.Find("Canvas_ScreenSpace").GetComponent<OverworldCanvas>().DisplayFactory(GameManager.Instance.CurrentFactory);
        }
        else
        {
            // Display "insufficient level"
        } 
    }
}
