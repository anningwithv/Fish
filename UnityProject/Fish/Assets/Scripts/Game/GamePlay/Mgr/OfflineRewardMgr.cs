using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qarth;

namespace GameWish.Game
{
    public class OfflineRewardMgr : TSingleton<OfflineRewardMgr>
    {
        private int m_GameplayTimer;
        private bool m_IsWatchingAD = false;

        private int GetShowTimeRewardMinTime
        {
            get { return 4; }
        }

        public void Init()
        {
            RegisterEvent();

            CheckTimeReward();
        }

        private void RegisterEvent()
        {
            EventSystem.S.Register(EngineEventID.OnApplicationPauseChange, OnApplicationPauseChange);
            EventSystem.S.Register(EventID.OnWatchingAD, HandleEvent);
        }

        protected void OnApplicationPauseChange(int key, params object[] args)
        {
            bool pause = (bool)args[0];
            if (!pause)
            {
                Log.i("Game Unpause.");
                if (m_IsWatchingAD)
                {
                    Log.w("Is watching ad, OfflineEarnPanel should not prop");
                    return;
                }

                CheckTimeReward();
            }
        }

        public void CheckTimeReward()
        {
            string lastTimeStr = GameData.LastPlayTimeString;
            if (!string.IsNullOrEmpty(lastTimeStr))
            {
                DateTime dtStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var timeStr = UnityExtensions.GetTimeStamp();
                if (!string.IsNullOrEmpty(timeStr))
                {
                    GameData.LastPlayTimeString = timeStr;
                    long longTimeLast /*= long.Parse(timeStr)*/;
                    long.TryParse(lastTimeStr, out longTimeLast);
                    var dtNow = DateTime.UtcNow;
                    var dtLast = dtStart.AddMilliseconds(longTimeLast);
                    var adds = (int)(dtNow - dtLast).TotalSeconds;

                    if (adds > GetShowTimeRewardMinTime)
                    {
                        //if(GuideMgr.S.IsGuideFinish(2))
                        //UIMgr.S.OpenPanel(UIID.PopEarnPanel, adds);
                    }
                }
            }

            StartTimeRecord();
        }

        private void StartTimeRecord()
        {
            OnGameTimeRecord(0);
            m_GameplayTimer = Timer.S.Post2Really(OnGameTimeRecord, 60, -1);
        }

        //每隔一段时间请求一下服务器时间存档作为最后游玩时间(ms时间戳)
        private void OnGameTimeRecord(int count)
        {
            Log.i("OnGameTimeRecord : " + count);

            GameDataMgr.S.Save();

            var timeStr = UnityExtensions.GetTimeStamp();
            if (/*m_GetTimeReward && */!string.IsNullOrEmpty(timeStr))
            {
                GameData.LastPlayTimeString = timeStr;
            }
        }

        private void HandleEvent(int eventId, params object[] param)
        {
            if (eventId.Equals((int)EventID.OnWatchingAD))
            {
                if (param == null || param.Length != 1)
                {
                    Log.e("Handle msg OnWatchingAD, but param pattern wrong!");
                    return;
                }

                m_IsWatchingAD = (bool)param[0];
            }
        }
    }
}
