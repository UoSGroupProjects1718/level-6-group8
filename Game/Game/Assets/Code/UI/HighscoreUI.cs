using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class HighscoreUI : MonoBehaviour
{
    public ScrollRect highscoreList;
    private DBManager dbm;
    private int _factoryId;

	// Use this for initialization
	void Start ()
	{
	    _factoryId = GameManager.Instance.CurrentFactory.FactoryId;
	    if (AuthServices.isSignedIn)
	    {
	        dbm = new DBManager();

//            Debug.Log(GetHighscores(_factoryId));
//	        highscoreList.content.GetComponent<Text>().text =
//                FirebaseAuth.DefaultInstance.CurrentUser.DisplayName + ": " + GetHighscores(_factoryId);
		    updateHighscores(_factoryId);
            
	    }
	} 

    private long? GetHighscores(int factoryID)
    {
        return dbm.GetFactoryHighscore(factoryID, FirebaseAuth.DefaultInstance.CurrentUser);
    }

	private void updateHighscores(int factoryID)
	{
		var text = highscoreList.content.GetComponent<Text>().text;
		foreach (var score in dbm.GetNearbyScores(factoryID, 6))
		{
			text = "\n\n\n";
			if (FirebaseAuth.DefaultInstance.CurrentUser.UserId == score.Key)
			{
				text += string.Format("\t{0} | {1} \r\n", FirebaseAuth.DefaultInstance.CurrentUser.DisplayName, score.Value);
			}
			else
			{
				text += string.Format("\t{0} | {1} \r\n", score.Key, score.Value);
			}
		}
	}

    // Update is called once per frame
	void Update () {
		
	}
}
