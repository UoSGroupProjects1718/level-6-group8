using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TownHall : MonoBehaviour
{
    public void UnloadMapSection(int i)
    {
        // Return if its already unlocked
        if (Overworld.Instance.TownSections[i].Unlocked)
        {
            return;
        }

        // Check the player has enough money
        if (GameManager.Instance.Player.PrimaryMoney >= Overworld.Instance.TownSections[i].Cost)
        {
            // Remove money
            GameManager.Instance.Player.RemovePrimaryMoney(Overworld.Instance.TownSections[i].Cost);

            // Unlock the factory
            Overworld.Instance.TownSections[i].Unlock();

            Debug.Log("New map section unlocked!");
        }
    }

    private void OnMouseDown()
    {
        // This check makes sure we didn't simply click on UI ontop of the game object
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // Update the canvas to open a pannel with this factories stats
            GameObject.Find("Canvas_ScreenSpace").GetComponent<OverworldCanvas>().DisplayTownHall();
        }
    }
}
