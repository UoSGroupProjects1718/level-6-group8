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

            Debug.Log(GetHighscores(_factoryId));
	        highscoreList.content.GetComponent<Text>().text =
                FirebaseAuth.DefaultInstance.CurrentUser.DisplayName + ": " + GetHighscores(_factoryId);
            
	    }
	} 

    private long? GetHighscores(int factoryID)
    {
        return dbm.GetFactoryHighscore(factoryID, FirebaseAuth.DefaultInstance.CurrentUser);
    }

    // Update is called once per frame
	void Update () {
		
	}
}
