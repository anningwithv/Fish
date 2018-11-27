using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class SocialMgr : TSingleton<SocialMgr>, ISocialAdapter
    {
        private const string RATE_RECORD_KEY = "record_rate_1202";
        private const string APP_RUNTIME_RECORD_KEY = "record_runtime_12143";
        private const string SHARE_TIME_RECORD_KEY = "share_key100";

        private static ISocialAdapter m_Adapter;
        private long m_StartRunningSecond = 0;

        public bool supportShare2Social
        {
            get
            {
                return m_Adapter.supportShare2Social;
            }
        }

        public void Init()
        {
#if UNITY_ANDROID
            m_Adapter = new AndroidSocialAdapter();
#elif UNITY_IOS
			m_Adapter = new IOSSocialAdapter();
#else
            m_Adapter = new DefauleSocialAdapter();
#endif
            m_Adapter.InitWithConfig(SDKConfig.S, null);

            m_StartRunningSecond = Helper.GetCurrentTimeSecond();

            InitEventListener();
            Log.i("Init[SocialMgr]");
        }

        private void InitEventListener()
        {
            EventSystem.S.Register(EngineEventID.OnApplicationFocusChange, OnApplicationFocusChange);
        }

        private void OnApplicationFocusChange(int key, params object[] args)
        {
            bool state = (bool)args[0];
            if (state)
            {
                m_StartRunningSecond = Helper.GetCurrentTimeSecond();
            }
            else
            {
                PlayerPrefs.SetFloat(APP_RUNTIME_RECORD_KEY, GetGameTotalPlayingTime());
                m_StartRunningSecond = Helper.GetCurrentTimeSecond();
                PlayerPrefs.Save();
            }
        }

        public float GetGameTotalPlayingTime()
        {
            long pass = Helper.GetCurrentTimeSecond() - m_StartRunningSecond;
            float alreadyRunSecond = PlayerPrefs.GetFloat(APP_RUNTIME_RECORD_KEY, 0);
            return pass + alreadyRunSecond;
        }

        public bool NeedShowRatePanel()
        {
            float value = PlayerPrefs.GetFloat(RATE_RECORD_KEY, 1);
            if (value < 0)
            {
                return false;
            }

            float second = GetGameTotalPlayingTime() - value;
            if (second > 1200)
            {
                return true;
            }

            return false;
        }

        public void RecordShowRatePanel()
        {
            PlayerPrefs.SetFloat(RATE_RECORD_KEY, -1);
            PlayerPrefs.Save();
            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.OPEN_MARKET);
        }

        public void RecordOpenRatePanel()
        {
            PlayerPrefs.SetFloat(RATE_RECORD_KEY, GetGameTotalPlayingTime());
            PlayerPrefs.Save();
        }

        public string GetRandomLanguageConfig(string prefix)
        {
            string key = string.Format("{0}{1}", prefix, I18Mgr.S.langugePrefix);
            var data = TDSocialAdapterTable.GetData(key + "_Count");
            if (data == null)
            {
                key = prefix;
                data = TDSocialAdapterTable.GetData(key + "_Count");
            }

            if (data == null)
            {
                Log.e("Not Find RandomLanguageConfig:" + prefix);
                return null;
            }

            int count = Helper.String2Int(data.param1);
            int index = RandomHelper.Range(0, count);
            key = string.Format("{0}_{1}", key, index);

            data = TDSocialAdapterTable.GetData(key);

            if (data == null)
            {
                Log.e("Invalid Count Config For RandomLanguageConfig:" + prefix);
                return null;
            }

            return data.param1;
        }

        public void ShowLeaderboardUI()
        {
            m_Adapter.ShowLeaderboardUI();
        }

        public void ReportScore(string leaderboard, long score)
        {
            m_Adapter.ReportScore(leaderboard, score);
        }

        public void ShowAchievmentsUI()
        {
            m_Adapter.ShowAchievmentsUI();
        }

        public void ReportAchievementsUI(string achievementID, double progress)
        {
            m_Adapter.ReportAchievementsUI(achievementID, progress);
        }

        public int GetShareTime()
        {
            return PlayerPrefs.GetInt(SHARE_TIME_RECORD_KEY, 0);
        }

        public void ShareTextWithURL(string title, string msg, string url)
        {
            m_Adapter.ShareTextWithURL(title, msg, url);

            int shareTime = PlayerPrefs.GetInt(SHARE_TIME_RECORD_KEY, 0);
            PlayerPrefs.SetInt(SHARE_TIME_RECORD_KEY, shareTime + 1);

            EventSystem.S.Send(EngineEventID.OnShare2Social);
            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.SHARE_POSTCARD);
        }

        public void ShareImage(string title, string path)
        {
            m_Adapter.ShareImage(title, path);

            Timer.S.Post2Really(OnShareImageFinishTick, 5);

            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.SHARE_POSTCARD);
        }

        protected void OnShareImageFinishTick(int count)
        {
            EventSystem.S.Send(SDKEventID.OnShareImageFinish);
        }

        public void OpenMarketRatePage()
        {
            m_Adapter.OpenMarketRatePage();
            RecordShowRatePanel();
        }

        public void OpenMarketDownloadPage(string identifyer)
        {
            m_Adapter.OpenMarketDownloadPage(identifyer);
            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.CROSS_EXT, identifyer);
        }

        public string GetMarketDetailPageURL()
        {
            return m_Adapter.GetMarketDetailPageURL();
        }

        public int GetPriorityScore()
        {
            throw new NotImplementedException();
        }

        public bool InitWithConfig(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            throw new NotImplementedException();
        }

        public void RecommandApp(string AppId)
        {
            m_Adapter.RecommandApp(AppId);
        }

        public void NotificationMessage(string title,string message, System.DateTime newDate)
        {

            m_Adapter.NotificationMessage(title, message, newDate);
        }

        public void CleanNotification() 
        {
            m_Adapter.CleanNotification();
        }
    }
}
