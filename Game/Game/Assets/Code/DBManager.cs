using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class DBManager
{
    private DatabaseReference db;

    public DBManager()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://group8-game.firebaseio.com/");
        db = FirebaseDatabase.DefaultInstance.RootReference;
        //TestWrite();
    }

    void TestWrite()
    {
        db.Child("Testing").SetValueAsync("TestString");
        db.Child("Testing").SetValueAsync(82529);

        var dict = new Dictionary<string, object>();
        dict.Add("test", 20200);
        dict.Add("TestString", "test");

        db.Child("Testing").SetValueAsync(dict);
    }

    void TestRead()
    {
        
    }

    public void WriteNewUser(FirebaseUser newUser)
    {
        Debug.Log("Writing new user!");
        db.Child("users").Child(newUser.UserId).SetValueAsync(newUser.DisplayName).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Writing a new user hit a fault!");
                Debug.LogError(task.Exception.ToString());
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("Writing a new user was cancelled!");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Successfully wrote new user.");
            }
        });
    }

    public void WriteScore(uint score, int factoryID, FirebaseUser user)
    {
        db.Child("highscores").Child(factoryID.ToString()).Child(user.UserId).SetValueAsync(score);
    }

    public long? GetFactoryHighscore(int factoryID, FirebaseUser user)
    {
        var factoryHighscores = FirebaseDatabase.DefaultInstance
            .GetReference("highscores").Child(factoryID.ToString());
        long output = 0;
        factoryHighscores.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                var snapshot = task.Result;
                output = (long)snapshot.Child(user.UserId).GetValue(false);
            }
        });
        if(output != 0)
        {
            return output;
        } else
        {
            return null;
        }
    }

    //public List<int> GetFactoryHighscores(int factoryID)
    //{
    //    var factoryHighscores = FirebaseDatabase.DefaultInstance
    //        .GetReference("highscores").Child(factoryID.ToString());
    //    List<KeyValuePair<string, long>> highscores = new List<KeyValuePair<string, long>>();
    //    factoryHighscores.GetValueAsync().ContinueWith(task =>
    //    {
    //        if(task.IsFaulted)
    //        {

    //        } else if(task.IsCompleted)
    //        {
    //            var snapshot = task.Result;
    //            foreach(var user in snapshot.Children)
    //            {
    //                var userID = user.GetValue
    //                highscores.Add(factory, )
    //            }
    //        }
    //    })
    //}
}
