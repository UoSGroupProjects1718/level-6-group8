using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

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

    public Sprite FilledStar;
    public Sprite EmptyStar;

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

    void Start()
    {
       
    }

    public void AssignFactoryStars()
    {
        foreach (var factory in factories)
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
            
            //foreach (GameObject star in factory.transform.Find("Canvas").ch)
            //{
            //    if (starCounter == 0) break;
            //    star.gameObject.GetComponent<Image>().sprite = FilledStar;
            //}

            for (int i = 0; i < factory.transform.Find("Canvas").childCount; i++)
            {
                if (starCounter == 0) break;
                factory.transform.Find("Canvas").GetChild(i).gameObject.GetComponent<Image>().sprite = FilledStar;
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
