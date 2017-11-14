using UnityEngine;
using UnityEngine.UI;

public class FactoryStatsUI : MonoBehaviour
{
    public Text HighscoreText;

    public void SetHighscoreText(uint score)
    {
        HighscoreText.text = "Highscore: " + score;
    }
}
