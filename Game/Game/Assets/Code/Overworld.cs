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
    public Sprite Sprite_FilledStar;
    public Sprite Sprite_EmptyStar;
    public Sprite Sprite_Lock;

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
        foreach (var townSection in townSections)
        {
            foreach (var factory in townSection.Factories)
            {
                var canvas = factory.gameObject.transform.Find("Canvas").gameObject;
                var text = new GameObject("FactoryID");

                RectTransform trans = text.AddComponent<RectTransform>();

                Text textComponent = text.AddComponent<Text>();
                textComponent.text = factory.FactoryId.ToString();
                textComponent.fontSize = 24;
                textComponent.color = Color.white;     
                
                Instantiate(text, canvas.transform);
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

    public void UnlockTownSections()
    {
        foreach (TownSection section in townSections)
        {
            // Check if the section is unlocked
            if (section.ID <= GameManager.Instance.Player.MapSectionsUnlocked)
            {
                section.Unlock(true);
            } else
            {
                section.Unlock(false);
            }
        }
    }
}
