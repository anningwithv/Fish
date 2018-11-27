using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameWish.Game;
using Qarth;

namespace Qarth
{
    [TMonoSingletonAttribute("[SDK]/DataAnalysisMgr")]
    public class DataAnalysisMgr : TMonoSingleton<DataAnalysisMgr>, IDataAnalysisAdapter, IPanelEventLogger
    {
        private const string KEY_APP_INSTALL_TIME = "key_install1027";
        private List<IDataAnalysisAdapter> m_Adapters;
        private Dictionary<string, bool> m_SingleEventMap = new Dictionary<string, bool>();

        //!
        private int m_AnalysisTimer = -1;
        private int[] m_AnalysisFocusMins = { 1, 3, 5, 8, 10, 12, 15, 20, 25, 30, 60, 90, 120 };

        private int[] m_AnalysisLifeFocusMins = {};

        private List<int> m_AnalysisLifeFocusMinsList = new List<int>(){1, 2, 3, 5, 10, 20, 30, 60, 120};
        private int m_AnalysisTimerIndex;

        private int m_InstallDays;
        private string m_InstallDaysString;
        private string m_InstallDaysStringMax;

        public void Init()
        {
            InitSupportedAdapter(SDKConfig.S);
            UIMgr.S.panelEventLogger = this;
            InitDataAnalysisEnv();
            Log.i("Init[DataAnalysisMgr]");

            m_AnalysisTimerIndex = 0;
            m_AnalysisTimer = Timer.S.Post2Really(OnAnalysisTimeTick, 60, -1);
        }

        public void OnApplicationQuit()
        {
            if (MonoSingleton.isApplicationQuit)
            {
                return;
            }

            CancelAnalysisTimer();
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].OnApplicationQuit();
            }
        }

        public void LevelBegin(string levelID)
        {
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].LevelBegin(levelID);
            }
        }

        public void LevelComplate(string levelID)
        {
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].LevelComplate(levelID);
            }
        }

        public void LevelFailed(string levelID, string reason)
        {
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].LevelFailed(levelID, reason);
            }
        }

        public void CustomEvent(string eventID, string label = null)
        {
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].CustomEvent(eventID, label);
            }
        }

        public void CustomEventDic(string eventID, Dictionary<string, string> dic)
        {
            for (int i = 0; i < m_Adapters.Count; i++)
            {
                m_Adapters[i].CustomEventDic(eventID, dic);
            }
        }

        public void CustomEventLifeCircleSingle(string eventID, string label = null)
        {
            if (string.IsNullOrEmpty(eventID))
            {
                return;
            }

            if (PlayerPrefs.GetInt(eventID, 0) > 0)
            {
                return;
            }

            PlayerPrefs.SetInt(eventID, 1);

            CustomEvent(eventID,label);
        }

        public void CustomEventWithDate(string eventID, string label = null, bool max = true)
        {

            if (string.IsNullOrEmpty(label))
            {
                if (max)
                {
                    label = m_InstallDaysStringMax;
                }
                else
                {
                    label = m_InstallDaysString;
                }
            }
            else
            {
                if (max)
                {
                    label = string.Format("{0}_{1}", label, m_InstallDaysStringMax);
                }
                else
                {
                    label = string.Format("{0}_{1}", label, m_InstallDaysString);
                }
            }

            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].CustomEvent(eventID, label);
            }
        }

        public void CustomEventSingleton(string eventID, string label = null)
        {
            string key = eventID;
            if (!string.IsNullOrEmpty(label))
            {
                key += label;
            }

            if (m_SingleEventMap.ContainsKey(key))
            {
                return;
            }

            m_SingleEventMap.Add(key, true);

            CustomEvent(eventID, label);
        }

        public void CustomEventDailySingle(string eventID, string label = null, bool max = true)
        {
            string key = eventID;
            if (!string.IsNullOrEmpty(label))
            {
                key += label;
            }

            int day = PlayerPrefs.GetInt(key, -1);
            if (day == m_InstallDays)
            {
                return;
            }

            PlayerPrefs.SetInt(key, m_InstallDays);
            PlayerPrefs.Save();
            CustomEventWithDate(eventID, label, max);
        }

        public void CustomEventDuration(string eventID, long duration)
        {
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].CustomEventDuration(eventID, duration);
            }
        }

        public void CustomEventMapValue(string key, string value)
        {
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].CustomEventMapValue(key, value);
            }
        }

        public void Pay(TDPurchase data)
        {
            Pay(data.price, data.itemNum);
        }

        public void CustomEventMapSend(string eventID)
        {
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].CustomEventMapSend(eventID);
            }
        }

        public int GetPriorityScore()
        {
            throw new NotImplementedException();
        }

        public bool InitWithConfig(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            throw new NotImplementedException();
        }

        private void InitSupportedAdapter(SDKConfig config)
        {
            m_Adapters = new List<IDataAnalysisAdapter>();

            if (!config.dataAnalysisConfig.isEnable)
            {
                Log.w("DataAnalysis System Is Not Enable.");
                return;
            }

            //RegisterAdapter(config, config.dataAnalysisConfig.dataeyeConfig);
            RegisterAdapter(config, config.dataAnalysisConfig.appsflyerConfig);
            RegisterAdapter(config, config.dataAnalysisConfig.umengConfig);
            RegisterAdapter(config, config.dataAnalysisConfig.facebookConfig);
#if !UNITY_EDITOR
            RegisterAdapter(config, config.dataAnalysisConfig.firebaseConfig);
#endif
        }

        private void RegisterAdapter(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            if (!adapterConfig.isEnable)
            {
                return;
            }

            Type type = Type.GetType(adapterConfig.adapterClassName);
            if (type == null)
            {
                Log.w("Not Support DataAnalysis:" + adapterConfig.adapterClassName);
                return;
            }
            IDataAnalysisAdapter adapter = type.Assembly.CreateInstance(adapterConfig.adapterClassName) as IDataAnalysisAdapter;

            if (adapter == null)
            {
                Log.e("DataAnalysis Adapter Create Failed:" + adapterConfig.adapterClassName);
                return;
            }

            if (adapter.InitWithConfig(config, adapterConfig))
            {
                m_Adapters.Add(adapter);

                Log.i("Success Register DataAnalysisAdapter:" + adapterConfig.adapterClassName);
            }
            else
            {
                Log.w("Failed Register DataAnalysisAdapter:" + adapterConfig.adapterClassName);
            }
        }

        public void Pay(double cash, double coin)
        {
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].Pay(cash, coin);
            }
        }

        public void LogPanelOpen(string name, PanelEventLogType eventType)
        {
            switch (eventType)
            {
                case PanelEventLogType.Single:
                    CustomEventSingleton(DataAnalysisDefine.SINGLE_PANEL_EVENT, string.Format("{0}_open", name));
                    break;
                case PanelEventLogType.LifeCircleSingle:
                    CustomEventLifeCircleSingle(string.Format("{0}_open", name));
                    break;
                case PanelEventLogType.Repeat:
                    CustomEvent(DataAnalysisDefine.PANEL_EVENT, string.Format("{0}_open", name));
                    break;
                case PanelEventLogType.Mix:
                    CustomEventSingleton(DataAnalysisDefine.SINGLE_PANEL_EVENT, string.Format("{0}_open", name));
                    CustomEvent(DataAnalysisDefine.PANEL_EVENT, string.Format("{0}_open", name));
                    break;
                default:
                    break;
            }
        }

        public void LogPanelClose(string name, PanelEventLogType eventType)
        {
            switch (eventType)
            {
                case PanelEventLogType.Single:
                    CustomEventSingleton(DataAnalysisDefine.SINGLE_PANEL_EVENT, string.Format("{0}_close", name));
                    break;
                case PanelEventLogType.LifeCircleSingle:
                    CustomEventLifeCircleSingle(string.Format("{0}_close", name));
                    break;
                case PanelEventLogType.Repeat:
                    CustomEvent(DataAnalysisDefine.PANEL_EVENT, string.Format("{0}_close", name));
                    break;
                case PanelEventLogType.Mix:
                    CustomEventSingleton(DataAnalysisDefine.SINGLE_PANEL_EVENT, string.Format("{0}_close", name));
                    CustomEvent(DataAnalysisDefine.PANEL_EVENT, string.Format("{0}_close", name));
                    break;
                default:
                    break;
            }
        }

        void OnAnalysisTimeTick(int tickCount)
        {
            if (tickCount == m_AnalysisFocusMins[m_AnalysisTimerIndex])
            {
                CustomEvent(DataAnalysisDefine.PLAY_TIME, tickCount.ToString());
                if (m_AnalysisTimerIndex < m_AnalysisFocusMins.Length - 1)
                    m_AnalysisTimerIndex += 1;
                else
                    CancelAnalysisTimer();
            }

            int minutes = PlayerPrefs.GetInt(DataAnalysisDefine.LOGIN_DAILY_TIMES, 0);
            minutes++;
            OnAnalysisTimeTickForEvent(minutes);
            PlayerPrefs.SetInt(DataAnalysisDefine.LOGIN_DAILY_TIMES, minutes);
            PlayerPrefs.Save();
        }

        void OnAnalysisTimeTickForEvent(int minutes)
        {            
            if(m_AnalysisLifeFocusMinsList.Contains(minutes))
            {
                string key = "life_time_minute_" + minutes;
                CustomEventLifeCircleSingle(key);
            }     
        }

        void CancelAnalysisTimer()
        {
            if (m_AnalysisTimer > 0)
            {
                Timer.S.Cancel(m_AnalysisTimer);
                m_AnalysisTimer = -1;
            }
        }


        protected void InitDataAnalysisEnv()
        {
            string installTime = PlayerPrefs.GetString(KEY_APP_INSTALL_TIME, "");
            if (string.IsNullOrEmpty(installTime))
            {
                installTime = DateTime.Now.ToLongDateString();
                PlayerPrefs.SetString(KEY_APP_INSTALL_TIME, installTime);
                PlayerPrefs.Save();
            }

            DateTime time = DateTime.Parse(installTime);

            m_InstallDays = GetPassDay(time);
            m_InstallDaysString = m_InstallDays.ToString();
            if (m_InstallDays >= 30)
            {
                m_InstallDaysStringMax = "30";
            }
            else
            {
                m_InstallDaysStringMax = m_InstallDaysString;
            }

            Timer.S.Post2Really((count) =>
            {
                CustomEventDailySingle("Retain", null, false);
                CustomEventDailySingle("Retain_" + m_InstallDaysStringMax);
                CustomEventLifeCircleSingle("FirstInstall");
                CustomEventExtension();
            }, 15);
        }

        void CustomEventExtension()
        {
            RecordLogIn5Days(5);
            RecordLogOut7Days(7);
            RecordLog5In1Day(5);
            RecordLogOnLineTimeDaily(10);
        }

        void RecordLogIn5Days(int days)
        {
            //从激活时间起days天内登陆几次
            string key = string.Format("{0}{1}", DataAnalysisDefine.LOGIN_COUNT_INDAY, days);
            int count = PlayerPrefs.GetInt(key, 0);
            if (m_InstallDays < days)
            {
                count++;
                PlayerPrefs.SetInt(key, count);
                PlayerPrefs.Save();
            }
            else
            {
                CustomEventLifeCircleSingle(key, count.ToString());
            }
        }

        void RecordLogOut7Days(int days)
        {
            string key = string.Format("{0}{1}", DataAnalysisDefine.LOGIN_OUTDAY, days);
            //从激活时间起days天后有过登录
            if (m_InstallDays > days)
            {
                CustomEventLifeCircleSingle(key);
            }
        }

        void RecordLog5In1Day(int counts)
        {
            string key = string.Format("{0}{1}", DataAnalysisDefine.LOGIN_DAILY_COUNT, counts);
            string prefs = string.Format("Log__Daily_Times{0}", counts);
            //任意一天有counts次登录行为
            int count = PlayerPrefs.GetInt(prefs, -1);
            count++;
            int day = PlayerPrefs.GetInt(key, -1);
            if (day != m_InstallDays && day != -1)
            {
                if (count >= counts)
                {
                    CustomEventWithDate(key);
                }

                count = 0;
            }

            PlayerPrefs.SetInt(prefs,count);
            PlayerPrefs.SetInt(key, m_InstallDays);
            PlayerPrefs.Save();
        }

        void RecordLogOnLineTimeDaily(int minutes)
        {
            //任意一天在线时间超过minutes分钟
            string key = string.Format("{0}{1}", DataAnalysisDefine.LOGIN_DAILY_TIMES, minutes);
            int min = PlayerPrefs.GetInt(DataAnalysisDefine.LOGIN_DAILY_TIMES,0);

            int day = PlayerPrefs.GetInt(key, -1);
            if (day != m_InstallDays && day != -1)
            {
                if (min > minutes)
                {
                    CustomEventWithDate(key);
                }
                PlayerPrefs.SetInt(DataAnalysisDefine.LOGIN_DAILY_TIMES, 0);
            }
            PlayerPrefs.SetInt(key,m_InstallDays);
            PlayerPrefs.Save();
        }

        protected int GetPassDay(DateTime time)
        {
            DateTime now = DateTime.Now;
            int days = now.DayOfYear - time.DayOfYear;
            if (now.Year != time.Year)
            {
                days += (now.Year - time.Year) * 365;
            }

            return days;
        }

        public void SetUserLevel(int level)
        {
            for (int i = 0; i < m_Adapters.Count; ++i)
            {
                m_Adapters[i].SetUserLevel(level);
            }
        }
    }
}
