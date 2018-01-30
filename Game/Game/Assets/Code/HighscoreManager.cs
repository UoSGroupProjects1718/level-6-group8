using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    private DatabaseReference db;
	// Use this for initialization
	void Start () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://group8-game.firebaseio.com/");
	    db = FirebaseDatabase.DefaultInstance.RootReference;
        TestWrite();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    HighscoreManager()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void TestWrite()
    {
        db.Child("Testing").SetValueAsync("TestString");
        db.Child("Testing").SetValueAsync(82529);

        var dict = new Dictionary<string, object>();
        dict.Add("test", 20200);
        dict.Add("TestString", "BLA");

        db.Child("Testing").SetValueAsync(dict);
    }

    void TestRead()
    {
        
    }

    public void PushScore(int score)
    {
        db.Child()
    }   
}
