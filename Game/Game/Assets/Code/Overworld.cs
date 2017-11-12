using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for handling behaviour that 
/// occurs within the Overworld scene.
/// </summary>
public class Overworld : MonoBehaviour
{
    //Player player;

    [SerializeField]
    Factory[] factories;

    /// <summary>
    /// Spawn the player object if it isnt found in the game. Load it's stats
    /// </summary>
	void Start()
    {
        /*
        if (!GameObject.Find("Player")
        {
            player = Instantiate(Player);
            player.LoadStatsFromFile();
        }
        */
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
        GameManager.instance.LoadLevel(GameManager.instance.CurrentFactory);
    }

    /// <summary>
    /// Unlocks a factory if all requirements are met.
    /// </summary>
    public void PurchaseFactory()
    {
        if (int.MaxValue /* player.level*/ >= GameManager.instance.CurrentFactory.LevelToUnlock)
        {
            GameManager.instance.CurrentFactory.UnlockFactory();
            GameObject.Find("Canvas_ScreenSpace").GetComponent<OverworldCanvas>().DisplayFactory(GameManager.instance.CurrentFactory);
        }
        else
        {
            // Display "insufficient level"
        } 
    }
}
