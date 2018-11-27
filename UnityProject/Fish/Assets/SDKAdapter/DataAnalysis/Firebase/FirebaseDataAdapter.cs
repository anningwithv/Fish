using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Analytics;
using System;

namespace Qarth
{
    public class FirebaseDataAdapter : IDataAnalysisAdapter
    {

        private bool m_Init = false;
        public bool InitWithConfig(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            SDKMgr.S.RegisterFilebaseDepInitCB(() =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);    

                FirebaseAnalytics.SetUserProperty(
                FirebaseAnalytics.UserPropertySignUpMethod,
                    "Unity");
    // Set the user ID.
                FirebaseAnalytics.SetUserId("my_user");
    // Set default session duration values.
                FirebaseAnalytics.SetMinimumSessionDuration(new TimeSpan(0, 0, 10));
                FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
                m_Init = true;

            });
            return true;
        }

        public void CustomEvent(string eventID, string label = null)
        {
            if(!m_Init)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(eventID, "description", label == null ? "" : label);
        }

        public void CustomEventDuration(string eventID, long duration)
        {
        }

        public void CustomEventMapSend(string eventID)
        {

        }

        public void CustomEventMapValue(string key, string value)
        {

        }

        public int GetPriorityScore()
        {
            return 0;
        }

        public void LevelBegin(string levelID)
        {
            if(!m_Init)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(DataAnalysisDefine.EVENTID_STARTLEVEL, "levelID", levelID);
        }

        public void LevelComplate(string levelID)
        {
           if(!m_Init)
            {
                return;
            }

            FirebaseAnalytics.LogEvent(DataAnalysisDefine.EVENTID_OVERLEVEL, "levelID", levelID);
        }

        public void LevelFailed(string levelID, string reason)
        {
           if(!m_Init)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(DataAnalysisDefine.EVENTID_DYING,
                new Parameter("levelID", levelID),
                new Parameter("reason", reason));
        }

        public void OnApplicationQuit()
        {
        }

        public void Pay(double cash, double coin)
        {
           if(!m_Init)
            {
                return;
            }
            FirebaseAnalytics.LogEvent("purchase",
                new Parameter(FirebaseAnalytics.EventEcommercePurchase, cash),
                new Parameter("itemNumber", coin));
        }

        public void SetUserLevel(int level)
        {
            if(!m_Init)
            {
                return;
            }

            FirebaseAnalytics.LogEvent("LevelUp", new Parameter(FirebaseAnalytics.EventLevelUp, level));
        }
        public void CustomEventDic(string eventId, Dictionary<string, string> dic)
        {

        }
    }
}
