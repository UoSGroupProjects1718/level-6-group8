using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreUI : MonoBehaviour
{
    public ScrollRect highscoreList;
    private DBManager dbm;
    private int _factoryId;

	void Start ()
	{
	    _factoryId = GameManager.Instance.CurrentFactory.FactoryId;
	    if (AuthServices.isSignedIn)
	    {
		    UpdateHighscores(_factoryId);
	    }
	}

	private void OnEnable()
	{
		if (!AuthServices.isSignedIn) return;
		UpdateHighscores(_factoryId);
	}

	private void UpdateHighscores(int factoryId)
	{
		var scores = dbm.GetTopFactoryHighscores(factoryId);
		var sb = new StringBuilder();
		sb.Append("Highscores\n\n");
		if (scores.Count > 0)
		{
			foreach (var scorePair in scores)
			{
				var userId = scorePair.Key;
				var score = scorePair.Value;
				sb.AppendLine(string.Format("{0}\t|\t{1}", userId, score));
			}
		}
		else
		{
			sb.AppendLine("No highscores for this factory yet.");
		}
		
		highscoreList.content.GetComponent<Text>().text = sb.ToString();
	}
}
