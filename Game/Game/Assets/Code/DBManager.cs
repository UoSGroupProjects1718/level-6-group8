using System;
using System.Collections.Generic;
using System.Linq;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class DBManager
{
    private readonly DatabaseReference _db;

    public DBManager()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://group8-game.firebaseio.com/");
        _db = FirebaseDatabase.DefaultInstance.RootReference;
        //TestWrite();
    }

    // Writes a new user to the database with format <user_id> : <user_name>
    public void WriteNewUser(FirebaseUser newUser)
    {
        Debug.Log("Writing new user!");
        _db.Child("users").Child(newUser.UserId.GetHashCode().ToString()).SetValueAsync(newUser.DisplayName).ContinueWith(task =>
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

    // Writes a new score for a specific user
    // to the database under the 'highscores' node
    // with format <user_id> : <score>
    public void WriteScore(uint score, int factoryID, FirebaseUser user)
    {
        _db.Child("highscores")
            .Child(factoryID.ToString())
            .Child(user.UserId.GetHashCode().ToString()).SetValueAsync((long)score)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Writing score failed!");
                }
                else if (task.IsCanceled)
                {
                    Debug.Log("Writing score was cancelled.");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Wrote new score!");
                }
            });
    }

    // Writes a new score for a specific user
    // to the database under the 'highscores' node
    // with format <user_id> : <score>
    public uint? GetScore(int factoryID, FirebaseUser user)
    {
        uint? output = null;
        _db.Child("highscores")
            .Child(factoryID.ToString())
            .Child(user.UserId.GetHashCode().ToString()).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Getting score failed!");
                }
                else if (task.IsCanceled)
                {
                    Debug.Log("Getting score was cancelled.");
                }
                else if (task.IsCompleted)
                {
                    var snapshot = task.Result;
                    output = (uint) snapshot.GetValue(false);
                }
            });
        return output;
    }

//    [CanBeNull]
//    public List<KeyValuePair<string, long>> GetNearbyScores(int factoryID, int numberOfScores)
//    {
//        List<KeyValuePair<string, long>> scores = new List<KeyValuePair<string, long>>();
//        var userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
//        var factoryHighscores = FirebaseDatabase.DefaultInstance
//            .GetReference("highscores").Child(factoryID.ToString());
//        factoryHighscores.GetValueAsync().ContinueWith(task =>
//        {
//            if (task.IsFaulted)
//            {
//            }
//            else if (task.IsCompleted)
//            {
//                scores.AddRange(task.Result.Children.Select(child =>
//                    new KeyValuePair<string, long>(child.Key, (long) child.Value)));
//            }
//        });
//        var index = scores.FindIndex(score => score.Key == userID);
//        var numberOfBelowScores = Math.Min(0, Math.Min(index - 1, Math.Ceiling((double) numberOfScores / 2 - 1)));
//        var numberOfAboveScores = numberOfScores - numberOfBelowScores - 1;
//
//        scores = scores.GetRange(index - (int) numberOfBelowScores, (int) numberOfAboveScores + 1);
//
//
//        return scores;
//    }

    public List<KeyValuePair<string, uint>> GetFactoryHighscores(int factoryID)
    {
        var output = new List<KeyValuePair<string, uint>>();
        _db.Child("highscores")
            .Child(factoryID.ToString()).GetValueAsync()
            .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("Getting scores failed!");
                    }
                    else if (task.IsCanceled)
                    {
                        Debug.Log("Getting scores was cancelled.");
                    }
                    else if (task.IsCompleted)
                    {
                        var snapshot = task.Result;
                        output.AddRange(snapshot.Children.Select(scoreSnapshot =>
                            new KeyValuePair<string, uint>(
                                scoreSnapshot.Key,
                                (uint) scoreSnapshot.Value
                            )
                        ));
                    }
                }
            );
        return output;
    }

    public List<KeyValuePair<string, uint>> GetTopFactoryHighscores(int factoryID, int amount = 3)
    {
        var output = new List<KeyValuePair<string, uint>>();
        _db.Child("highscores")
            .Child(factoryID.ToString())
            .OrderByValue()
            .LimitToFirst(amount)
            .GetValueAsync()
            .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("Getting scores failed!");
                    }
                    else if (task.IsCanceled)
                    {
                        Debug.Log("Getting scores was cancelled.");
                    }
                    else if (task.IsCompleted)
                    {
                        var snapshot = task.Result;
                        output.AddRange(snapshot.Children.Select(scoreSnapshot =>
                            new KeyValuePair<string, uint>(
                                scoreSnapshot.Key,
                                (uint) scoreSnapshot.Value
                            )
                        ));
                    }
                }
            );
        return output;
    }

    public string GetDisplayNameFromHasedId(string hashedId)
    {
        var name = string.Empty;
        _db.Child("users")
            .Child(hashedId).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Getting user display name failed!");
                }
                else if (task.IsCanceled)
                {
                    Debug.Log("Getting user display name was cancelled.");
                }
                else if (task.IsCompleted)
                {
                    name = (string)task.Result.Value;
                }
            });
        return name;
    }
}