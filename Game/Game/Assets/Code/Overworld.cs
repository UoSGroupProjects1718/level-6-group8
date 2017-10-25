using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overworld : MonoBehaviour
{
    //Player player;
    private Factory currentlySelectedFactory;

    public Factory CurrentlySelectedFactory { get { return currentlySelectedFactory; } }

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

    public void SetCurrentFactory(Factory factory)
    {
        currentlySelectedFactory = factory;
    }

    public void PurchaseFactory()
    {
        if (int.MaxValue /* player.level*/ >= currentlySelectedFactory.LevelToUnlock)
        {
            //player.money -= currentlySelectedFactory.Cost;

            currentlySelectedFactory.UnlockFactory();
            GameObject.Find("Canvas").GetComponent<OverworldCanvas>().DisplayFactory(currentlySelectedFactory);
        }
        else
        {
            // Display "insufficient funds"
        } 
    }
}
