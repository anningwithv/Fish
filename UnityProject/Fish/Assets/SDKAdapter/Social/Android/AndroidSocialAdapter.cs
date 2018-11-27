using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AndroidSocialAdapter : AbstractSDKAdapter, ISocialAdapter
    {
        protected AndroidJavaObject m_ActivityObject;
        protected AndroidJavaClass m_ShareClass;
        protected AndroidJavaClass m_recommendClass;

        public AndroidJavaObject activityObject
        {
            get
            {
                if (m_ActivityObject == null)
                {
                    AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    m_ActivityObject = jc.GetStatic<AndroidJavaObject>("currentActivity");
                }

                return m_ActivityObject;
            }
        }

        public AndroidJavaClass shareClass
        {
            get
            {
                if (m_ShareClass == null)
                {
                    m_ShareClass = new AndroidJavaClass("com.vega.share.AndroidShare");
                }

                return m_ShareClass;
            }
        }

        public AndroidJavaClass recommendClass
        {
            get
            {
                if (m_recommendClass == null)
                {
                    m_recommendClass = new AndroidJavaClass("com.vega.share.MarketHelper");
                }
                return m_recommendClass;
            }
        }

        public bool supportShare2Social
        {
            get
            {
                return true;
            }
        }

        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            return true;
        }

        public void OpenMarketRatePage()
        {
            Application.OpenURL(GetMarketDetailPageURL());
        }

        public void OpenMarketDownloadPage(string identifyer)
        {
            Application.OpenURL(string.Format("market://details?id={0}", identifyer));
        }

        public string GetMarketDetailPageURL()
        {
            return string.Format("market://details?id={0}", Application.identifier);
        }

        public void ReportAchievementsUI(string achievementID, double progress)
        {
            
        }

        public void ReportScore(string leaderboard, long score)
        {
            
        }

        public void ShareTextWithURL(string title, string msg, string url)
        {




        }

        public void ShowAchievmentsUI()
        {
            
        }

        public void ShowLeaderboardUI()
        {
            
        }

        public void ShareImage(string title, string path)
        {
#if !UNITY_EDITOR
            var share = shareClass;
            if (share == null)
            {
                return;
            }
            share.CallStatic("sharePicture", activityObject, title, path);
#endif
        }

        public void RecommandApp(string AppId)
        {
            var recommand = recommendClass;
            if (recommand == null)
            {
                return;
            }            
            recommand.CallStatic("openMarket", activityObject, AppId);
        }

        public void NotificationMessage(string title, string message, System.DateTime newDate)
        {
            
        }

        public void CleanNotification() 
        {
            
        }
    }
}
