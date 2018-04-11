using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryPanel : MonoBehaviour {

    [Header("Objects")]
    [SerializeField]
    GameObject dull;

    [Header("UI Elements")]
    [SerializeField]
    RawImage factorySprite;
    [SerializeField]
    Text factoryName;
    [SerializeField]
    Text score;
    [SerializeField]
    Image[] factoryStars;
    [SerializeField]
    Text[] thresholds;

    public Image HighscorePanel; 

    [Header("Asset Reference")]
    [SerializeField]
    private Sprite StarFilled;
    [SerializeField]
    private Sprite StarEmpty;

    /// <summary>
    /// Displays the factory pannel.
    /// If the factory is unlocked, displays factory stats and a button to enter.
    /// Otherwise, displays level to unlock and an "Unlock" button.
    /// </summary>
    /// <param name="factory"></param>
    public void UpdateUI(Factory factory)
    {
        // Disable dragging whilst the menu is open
        DisableCameraMoveScript();

        // Check which pannel to open
        if (factory.Unlocked)
        {
            toggleDim(true);

            factoryName.text = factory.FactoryName;
            score.text = ""+factory.Score;
            //factorySprite.texture = factory.FactorySprite;

            for (int i = 0; i < 3; i++)
            {
                thresholds[i].text = "" + factory.ScoreThresholds[i];
            }

            if (factory.Solved)
            {
                fillfactoryStars(factory.Stars);
            }
            else
            {
                fillfactoryStars(0);
            }
        }
    }

    public void toggleDim(bool active)
    {
        dull.SetActive(active);
    }

    private void fillfactoryStars(uint stars)
    {
        for (int i = 0; i < factoryStars.Length; i++)
        {
            if (i < stars)
            {
                factoryStars[i].sprite = StarFilled;
            }
            else
            {
                factoryStars[i].sprite = StarEmpty;
            }
        }
    }

    public void ToggleHighscorePanel()
    {
        HighscorePanel.gameObject.SetActive(!HighscorePanel.gameObject.activeSelf);
    }

    /// <summary>
    /// Disables the drag script on the camera
    /// </summary>
    private void DisableCameraMoveScript()
    {
        Camera.main.GetComponent<OrthoCameraDrag>().enabled = false;
    }
}