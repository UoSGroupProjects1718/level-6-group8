using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreScreen : MonoBehaviour
{
    [SerializeField]
    Image potionImage;

    [SerializeField]
    Text factoryNameText;

    [SerializeField]
    Text statsText; // This is where efficiency, reward, ticks, etc. is displayed

    [SerializeField]
    Text scoreText;


    public void SetScore(int score, int ticks)
    {
        // Set factory name
        factoryNameText.text = LevelController.Instance.LevelFactory.FactoryName;

        // Set the Image to the sprite of the Item we made to solve the level
        potionImage.sprite = LevelController.Instance.LevelFactory.Target.ItemSprite;

        // Set the stats text
        statsText.text = string.Format("Efficiency: {0}", ticks);

        // Set the score text
        scoreText.text = string.Format("Final score: {0}", score.ToString());
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
