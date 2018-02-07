using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownHall : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField]
    Button sectionButton;
    [SerializeField]
    Text sectionButtonText;
    [SerializeField]
    Text sectionPrice;
    [SerializeField]
    Image primaryCost;

    public void UnloadMapSection()
    {
        // Check the player has enough money
        if (GameManager.Instance.Player.PrimaryMoney >= Overworld.Instance.TownSections[GameManager.Instance.Player.MapSectionsUnlocked].Cost)
        {
            // Remove money
            GameManager.Instance.Player.RemovePrimaryMoney(Overworld.Instance.TownSections[GameManager.Instance.Player.MapSectionsUnlocked].Cost);

            // Unlock the factory
            Overworld.Instance.TownSections[GameManager.Instance.Player.MapSectionsUnlocked].Unlock(true);

            Debug.Log("New map section unlocked!");

            GameManager.Instance.Player.UnlockNextMapSection();

            // Set the UI for the next section
            setNextSection();
        }
    }

    private void setNextSection()
    {
        // Checks if new sections can be unlocked
        if((int)GameManager.Instance.Player.MapSectionsUnlocked == Overworld.Instance.TownSections.Length)
        {
            sectionButton.interactable = false;
            sectionPrice.text = "";
            sectionButtonText.text = "More Coming Soon";
            primaryCost.enabled = false;
        }
        else
        {
            TownSection section = Overworld.Instance.TownSections[GameManager.Instance.Player.MapSectionsUnlocked];
            sectionPrice.text = section.Cost.ToString();
        }
    }

    private void OnMouseDown()
    {
        // This check makes sure we didn't simply click on UI ontop of the game object
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            setNextSection();

            // Update the canvas to open a pannel with this factories stats
            GameObject.Find("Canvas_ScreenSpace").GetComponent<OverworldCanvas>().DisplayTownHall();
        }
    }
}
