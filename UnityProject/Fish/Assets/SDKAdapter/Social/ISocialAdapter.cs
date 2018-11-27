using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public interface ISocialAdapter : ISDKAdapter
    {
        void ShowLeaderboardUI();
        void ReportScore(string leaderboard, long score);
        void ShowAchievmentsUI();
        void ReportAchievementsUI(string achievementID, double progress);
        void ShareTextWithURL(string title, string msg, string url);
        void OpenMarketRatePage();
        void OpenMarketDownloadPage(string identifyer);
        void ShareImage(string title, string path);
        void RecommandApp(string AppId);
        string GetMarketDetailPageURL();

        bool supportShare2Social
        {
            get;
        }

        void NotificationMessage(string title,string message, System.DateTime newDate);

        void CleanNotification();
    }
}
