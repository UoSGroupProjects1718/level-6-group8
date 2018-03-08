using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

	public Image[] StarBoxes;
	public Slider ScoreSlider;
	public Sprite emptyStar, filledStar;
	
	[Range(0, 5000)]
	public float SliderAnimationTimeMs;


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

	public void SetSliderToScore(uint score)
	{
		var sliderWidth = ScoreSlider.gameObject.GetComponent<RectTransform>().rect.width;
		FillStarsBasedOnScore(score);
		AnimateSliderToScore(score, 1000);
	}
	

	public void SetupStarBoxes()
	{
		var sliderWidth = ScoreSlider.gameObject.GetComponent<RectTransform>().rect.width;
		var threshholds = LevelController.Instance.LevelFactory.ScoreThresholds;
		for (var i = 0; i < StarBoxes.Length; i++)
		{
			var starbox = StarBoxes[i];
			var xPos = Mathf.Lerp(0, sliderWidth, (float)threshholds[i] / threshholds[threshholds.Length - 1]);
			starbox.rectTransform.anchoredPosition = new Vector2(xPos, starbox.rectTransform.anchoredPosition.y);
		}
	}

	private void FillStarsBasedOnScore(uint score)
	{
		var threshholds = LevelController.Instance.LevelFactory.ScoreThresholds;
		var starsToFill = threshholds.Count(threshhold => score >= threshhold);

		for (var i = 0; i < starsToFill; i++)
		{
			StarBoxes[i].transform.Find("Star").GetComponent<Image>().sprite = filledStar;
		}
	}

	private void AnimateSliderToScore(uint score, uint timeInMs)
	{
		var thresholds = LevelController.Instance.LevelFactory.ScoreThresholds;
		var desiredSliderValue = (float) score / thresholds[thresholds.Length - 1];
		DOTween.To(() => ScoreSlider.value, x => ScoreSlider.value = x, desiredSliderValue, (float)timeInMs / 1000);
	}
}
