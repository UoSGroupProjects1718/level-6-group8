using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownHall : MonoBehaviour
{
    private int currentSectionIndex = 0;

    [SerializeField]
    Button sectionButton;
    [SerializeField]
    Text sectionButtonText;
    [SerializeField]
    Text sectionPrice;
    [SerializeField]
    Image primaryCost;

    public void Start()
    {
        // @Johny TODO: Store/find saved players unlocked areas
        //currentSection = GameManager.Instance.Player.
        for(int i = 0; i < Overworld.Instance.TownSections.Length; i++)
        {
            if (Overworld.Instance.TownSections[i].Unlocked)
            {
                currentSectionIndex++;
            } else
            {
                break;
            }
        }

        setNextSection();
    }

    public void UnloadMapSection()
    {
        // Check the player has enough money
        if (GameManager.Instance.Player.PrimaryMoney >= Overworld.Instance.TownSections[currentSectionIndex].Cost)
        {
            // Remove money
            GameManager.Instance.Player.RemovePrimaryMoney(Overworld.Instance.TownSections[currentSectionIndex].Cost);

            // Unlock the factory
            Overworld.Instance.TownSections[currentSectionIndex].Unlock();

            Debug.Log("New map section unlocked!");

            currentSectionIndex++;
            // Set the UI for the next section
            setNextSection();
        }


    }

    private void setNextSection()
    {
        // Checks if new sections can be unlocked
        if(currentSectionIndex == Overworld.Instance.TownSections.Length)
        {
            sectionButton.interactable = false;
            sectionPrice.text = "";
            sectionButtonText.text = "More Coming Soon";
            primaryCost.enabled = false;
        } else
        {
            TownSection section = Overworld.Instance.TownSections[currentSectionIndex];
            sectionPrice.text = section.Cost.ToString();
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
