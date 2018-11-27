using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using System.Runtime.InteropServices;

#if UNITY_IPHONE || UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;
#endif

namespace Qarth
{
    public class IOSSocialAdapter : AbstractSDKAdapter, ISocialAdapter
    {
        private bool m_GameCenterState = false;

        public bool supportShare2Social
        {
            get
            {
                return true;
            }
        }

        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            if (Social.localUser == null)
            {
                return false;
            }

            /*
            if (!Social.localUser.authenticated)
            {
                return true;
            }
            */
            Social.localUser.Authenticate(OnAuthenticatedEvent);
            if (Social.localUser.authenticated)
            {
                m_GameCenterState = true;
            }
            /*
#if UNITY_IPHONE
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(
                NotificationType.Alert |
                NotificationType.Badge |
                NotificationType.Sound);
#endif
            */
            return true;
        }

        public string GetMarketDetailPageURL()
        {
            return string.Format("itms-apps://itunes.apple.com/app/id{0}", SDKConfig.S.iosAppID);
        }

#if UNITY_IPHONE

        [DllImport("__Internal")]
        private static extern void ShareText2SocialPlatform(string title, string msg, string url);
        [DllImport("__Internal")]
		private static extern void OpenAppStoreRatepage(string appID);
        [DllImport("__Internal")]
        private static extern void ShareImage2SocialPlatform(string title, string imagePath);
#endif

        public void ShareTextWithURL(string title, string msg, string url)
        {
#if UNITY_IPHONE
            ShareText2SocialPlatform(title, msg, url);
#endif
        }

        public void ShareImage(string title, string path)
        {
#if UNITY_IPHONE
            ShareImage2SocialPlatform(title,path);
#endif
        }

        public void RecommandApp(string AppId)
        {

        }


        public void OpenMarketRatePage()
        {
#if UNITY_IPHONE
            OpenAppStoreRatepage(SDKConfig.S.iosAppID);
#endif
        }

        public void OpenMarketDownloadPage(string identifyer)
        {
#if UNITY_IPHONE
            OpenAppStoreRatepage(identifyer);
#endif
        }

        public void ShowLeaderboardUI()
        {
            if (!m_GameCenterState)
            {
				Log.i ("GameCenter Not Auth Now.");
                return;
            }

            Social.ShowLeaderboardUI();
        }

        public void ReportScore(string leaderboard, long score)
        {
            if (!m_GameCenterState)
            {
                return;
            }

            if (string.IsNullOrEmpty(leaderboard))
            {
                Log.e("Invalid Leaderboard.");
                return;
            }

            var data = TDSocialAdapterTable.GetData(leaderboard);
            if (data == null)
            {
                Log.w("Not Find Leaderboard Data:" + leaderboard);
                return;
            }

            if (string.IsNullOrEmpty(data.param1))
            {
                Log.w("Invalid GameCenter leaderboard Config");
                return;
            }

            Social.ReportScore(score, data.param1, OnReportScoreEvent);
        }

        public void ShowAchievmentsUI()
        {
            if (!m_GameCenterState)
            {
                return;
            }

            Social.ShowAchievementsUI();
        }

        public void ReportAchievementsUI(string achievementID, double progress)
        {
            if (!m_GameCenterState)
            {
                return;
            }

            Social.ReportProgress(achievementID, progress, OnReportAchievementsEvent);
        }

        private void OnReportScoreEvent(bool state)
        {
            Log.i("[GameCenter]OnReportScoreEvent:" + state);
        }

        private void OnReportAchievementsEvent(bool state)
        {
            Log.i("[GameCenter]OnReportAchievementsEvent:" + state);
        }

        private void OnAuthenticatedEvent(bool state)
        {
			m_GameCenterState = Social.localUser.authenticated;
			Log.i("[GameCenter]OnAuthenticatedEvent:" + state + " Auth:" + m_GameCenterState);
        }

        public void NotificationMessage(string title, string message, System.DateTime newDate) 
        {
#if UNITY_IPHONE
            //推送时间需要大于当前时间
            if (newDate > System.DateTime.Now)
            {
                UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
                localNotification.fireDate = newDate;
                localNotification.alertBody = message;
                localNotification.applicationIconBadgeNumber = 1;
                localNotification.hasAction = true;
                localNotification.alertAction = title;
                localNotification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
                UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
            }
#endif

        }

        public void CleanNotification()
        {
#if UNITY_IPHONE
            UnityEngine.iOS.LocalNotification l = new UnityEngine.iOS.LocalNotification();
            l.applicationIconBadgeNumber = -1;
            UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(l);
            UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
            UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
#endif
        }
    }
}
