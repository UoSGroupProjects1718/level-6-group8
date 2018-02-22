using System;
using System.Collections.Generic;
using System.Linq;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using JetBrains.Annotations;
using NUnit.Framework.Constraints;
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

    [CanBeNull]
    public List<KeyValuePair<string, long>> GetNearbyScores(int factoryID, int numberOfScores)
    {
        List<KeyValuePair<string, long>> scores = new List<KeyValuePair<string, long>>();
        var userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var factoryHighscores = FirebaseDatabase.DefaultInstance
            .GetReference("highscores").Child(factoryID.ToString());
        factoryHighscores.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                scores.AddRange(task.Result.Children.Select(child => 
                    new KeyValuePair<string, long>(child.Key, (long) child.Value)));
            }
        });
        var index = scores.FindIndex(score => score.Key == userID);
        var numberOfBelowScores = Math.Min(0,Math.Min(index - 1, Math.Ceiling((double)numberOfScores / 2 - 1)));
        var numberOfAboveScores = numberOfScores - numberOfBelowScores - 1;

        scores = scores.GetRange(index - (int)numberOfBelowScores, (int)numberOfAboveScores + 1);


        return scores;

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
