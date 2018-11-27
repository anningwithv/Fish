using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class DefauleSocialAdapter : AbstractSDKAdapter, ISocialAdapter
    {
        public bool supportShare2Social
        {
            get
            {
                return false;
            }
        }

        public bool InitWithConfig(SDKConfig config)
        {
            return true;
        }

        public void ReportAchievementsUI(string achievementID, double progress)
        {
        }

        public void ReportScore(string leaderboard, long score)
        {
        }

        public void ShowAchievmentsUI()
        {
        }

        public void ShowLeaderboardUI()
        {
        }

        public void ShareTextWithURL(string title, string msg, string url)
        {

        }

        public void OpenMarketRatePage()
        {
            SocialMgr.S.RecordShowRatePanel();
        }

        public void OpenMarketDownloadPage(string identifyer)
        {
            SocialMgr.S.RecordShowRatePanel();
        }

        public string GetMarketDetailPageURL()
        {
            return "";
        }

        public void ShareImage(string title, string path)
        {
            
        }
        public void RecommandApp(string AppId)
        {

        }

        public void NotificationMessage(string title, string message, System.DateTime newDate) 
        {
            
        }

        public void CleanNotification()
        {
            
        }
    }
}
