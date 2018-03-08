using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FactoryOverworld : MonoBehaviour {

    public Factory self;

    [SerializeField]
    private Sprite Sprite_FilledStar;
    [SerializeField]
    private Sprite Sprite_EmptyStar;
    [SerializeField]
    private Sprite Sprite_Lock;

    [SerializeField]
    private Image[] stars;

    private Color clearYellow = new Color32(255,255,0, 25);
    private Color defaultYellow = new Color32(255, 255, 0, 255);

    void Start()
    {
        foreach(Transform child in gameObject.transform)
        {
            if (child.name.Contains("IDText"))
            {
                child.GetComponent<Text>().text = self.FactoryId.ToString();
            }
        }
    }
    

    public void setUI()
    {
        
        if (self.Unlocked)
        {
            setStars();
        } else
        {
            setLocked();
        }
    }
    
    private void setStars()
    {
        int filled = calculateFilledStars();
        for(int i = 0; i < stars.Length; i++)
        {
            if (filled > i)
                stars[i].sprite = Sprite_FilledStar;
            else
                stars[i].sprite = Sprite_EmptyStar;

            stars[i].color = defaultYellow;
        }
    }

    private int calculateFilledStars()
    {
        int i = 0;
        foreach (int scoreThreshold in self.ScoreThresholds)
        {
            if(self.Score > scoreThreshold)
                i++;
            else
                return i;
        }
        return i;
    }

    private void setLocked()
    {
        stars[0].sprite = Sprite_EmptyStar;
        stars[0].color = clearYellow;
        stars[1].sprite = Sprite_Lock;
        stars[2].sprite = Sprite_EmptyStar;
        stars[2].color = clearYellow;
    }
}
