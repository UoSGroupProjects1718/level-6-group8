using System.Runtime.InteropServices;
using System.Text;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreUI : MonoBehaviour
{
    public ScrollRect highscoreList;
	public Text userHighscoreText;
    private int _factoryId;

	void Start ()
	{
	    _factoryId = GameManager.Instance.CurrentFactory.FactoryId;
	    if (AuthServices.isSignedIn)
	    {
		    UpdateHighscores(_factoryId, new DBManager());
	    }
	}

	private void OnEnable()
	{
		if (!AuthServices.isSignedIn) return;
		UpdateHighscores(_factoryId, new DBManager());
	}

	private void SetUserHighscore(int factoryId, DBManager dbm)
	{
		var score = dbm.GetScore(factoryId, FirebaseAuth.DefaultInstance.CurrentUser) ?? 0;
		userHighscoreText.text = score.ToString();
	}

	private void UpdateHighscores(int factoryId, DBManager dbm)
	{
		SetUserHighscore(factoryId, dbm);
		var scores = dbm.GetTopFactoryHighscores(factoryId);
		var sb = new StringBuilder();
		if (scores.Count > 0)
		{
			foreach (var scorePair in scores)
			{
				var userId = scorePair.Key;
				var score = scorePair.Value;
				sb.AppendLine(string.Format("{0}\t|\t{1}", dbm.GetDisplayNameFromHasedId(userId), score));
			}
		}
		else
		{
			sb.AppendLine("No highscores for this factory yet.");
		}
		
		highscoreList.content.GetChild(0).GetComponent<Text>().text = sb.ToString();
	}
}
