using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Code.Player
{
    //TODO: Finish this using Social API https://docs.unity3d.com/Manual/net-SocialAPI.html
    class PlayerAchievements
    {
        private List<IAchievement> achievements;
        private List<IAchievementDescription> achievementDescriptions;

        public PlayerAchievements(List<IAchievement> achievements = null,
            List<IAchievementDescription> achievementDescriptions = null)
        {
            this.achievements = achievements;
            this.achievementDescriptions = achievementDescriptions;
        }

        public void Init()
        {
            achievements = new List<IAchievement>();
            achievementDescriptions = new List<IAchievementDescription>();
            Social.localUser.Authenticate(processAuthentication);
            LoadAchievementDescriptions();
        }

        private void processAuthentication(bool success)
        {
            if (success)
            {
                Debug.Log("Successfully authenticated!");
                LoadPlayerAchievements();
            }
        }

        private void LoadAchievementDescriptions()
        {
            Social.LoadAchievementDescriptions(loadedAchievementDescriptions =>
            {
                if (loadedAchievementDescriptions.Length > 0)
                {
                    achievementDescriptions.AddRange(loadedAchievementDescriptions);
                }
                else
                {
                    Debug.Log("No achievement data set up.");
                }
            });
        }

        public void LogAchievementDescriptions()
        {
            Debug.Log("Printing achievement Descriptions");
            foreach (var achievementDescription in achievementDescriptions) 
            {
                Debug.Log(achievementDescription.achievedDescription);
            }
        }

        private void LoadPlayerAchievements()
        {
            Social.LoadAchievements(loadedAchievements =>
            {
                if (loadedAchievements.Length > 0)
                    achievements.AddRange(loadedAchievements);
                else
                    Debug.Log("Player has no achievements.");
            });
        }

        public void LogPlayerAchievementInfo()
        {
            Debug.Log("Printing player achievement info");

            if (achievements.Count > 0)
            {
                foreach (var achievement in achievements)
                {
                    Debug.Log(achievement);
                }
            }
        }
    }
}
