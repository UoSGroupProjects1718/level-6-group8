using System.Collections;
using System.Collections.Generic;
// using NUnit.Framework.Constraints; Disabled for Android build
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for handling behaviour that 
/// occurs within the Overworld scene.
/// </summary>
public class Overworld : MonoBehaviour
{
    private static Overworld instance = null;

    [Header("Town sections")]
    [SerializeField]
    private TownSection[] townSections;

    public static Overworld Instance { get { return instance; } }
    public TownSection[] TownSections { get { return townSections; } }

    [Header("Misc")]
    public Sprite FilledStar;
    public Sprite EmptyStar;

    private OverworldCanvas oc;
    public OverworldCanvas OverworldCanvas { get { return oc; } }

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

        oc = GameObject.FindGameObjectWithTag("OverworldCanvas").GetComponent<OverworldCanvas>();
    }

    public void AssignFactoryStars(int theFactoryID)
    {
        foreach (TownSection section in townSections)
        {
            foreach (Factory factory in section.Factories)
            {
                if (factory.FactoryId == theFactoryID)
                {
                    uint starCounter = 0;
                    foreach (int scoreThreshold in factory.ScoreThresholds)
                    {
                        if (factory.Score > scoreThreshold)
                        {
                            starCounter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    factory.Stars = starCounter;

                    for (int i = 0; i < factory.transform.Find("Canvas").childCount; i++)
                    {
                        if (starCounter == 0) break;
                        factory.transform.Find("Canvas").GetChild(i).gameObject.GetComponent<Image>().sprite = FilledStar;
                    }

                    return;
                }
            }
        }
    }

    /// <summary>
    /// Saves all factories stats and then calls the GameManager to change 
    /// to the level scene and loads the game managers current factory
    /// </summary>
    public void LoadFactory()
    {
        // Save all factory data to file
        foreach (TownSection section in townSections)
        {
            foreach (Factory factory in section.Factories)
            {
                factory.SaveStatsToFile();
            }
        }

        // Check that the factory is unlocked
        if (GameManager.Instance.CurrentFactory.Unlocked)
        {
            // Call game manager to handle loading the level
            GameManager.Instance.LoadLevel(GameManager.Instance.CurrentFactory);
        }
    }

    /// <summary>
    /// Unlocks a factory if all requirements are met.
    /// </summary>
    public void PurchaseFactory()
    {
        if (int.MaxValue /* player.level*/ >= GameManager.Instance.CurrentFactory.starsToUnlock)
        {
            // Unlock the factory
            GameManager.Instance.CurrentFactory.Unlock();

            // Save the factory to file
            GameManager.Instance.CurrentFactory.SaveStatsToFile();

            GameObject.Find("Canvas_ScreenSpace").GetComponent<OverworldCanvas>().DisplayFactory(GameManager.Instance.CurrentFactory);
        }
        else
        {
            // Display "insufficient level"
        }
    }

    public void UnlockTownSections()
    {
        foreach (TownSection section in townSections)
        {
            // If this section is unlocked
            if (section.ID <= GameManager.Instance.Player.MapSectionsUnlocked)
            {
                section.Unlock();
            }
            // Else if its locked
            else
            {
                section.DisableLights();
            }
        }
    }
}
