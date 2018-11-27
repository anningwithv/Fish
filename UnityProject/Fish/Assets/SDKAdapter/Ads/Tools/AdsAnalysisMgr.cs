using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Qarth
{
    public class AdsAnalysisDefine
    {
        //PlayerPrefs
        public static string PLAY_MINTIME = "Play_MinTime";
        public static string REWARD_SHOW_COUNT = "Reward_Show_Count";
        public static string INTER_SHOW_COUNT = "Inter_Show_Count";
        public static string GAME_PROCESS = "Game_Process";
        public static string INTER_INTERVAL = "Inter_Interval";
        public static string USER_GRADE = "User_Grade";

        //RemoteConfig
        public static string TIME_WEIGHT = "playtime_weight";
        public static string REWARD_WEIGHT = "reward_weight";
        public static string INTER_WEIGHT = "inter_weight";
        public static string PROCESS_WEIGHT = "gameprocess_weight";
        public static string SHOW_INTER_TIME = "show_inter_time";
        public static string SEC_INTER_TIME = "secinter_time";
    }
	public class AdsAnalysisMgr : TSingleton<AdsAnalysisMgr>
	{
	    public static readonly int defaultInterVal = 300;
	    public static readonly int userGradeCount = 10;

        private int m_PlayMinTime;
	    private int m_RewardShowCount;
	    private int m_InterShowCount;
	    private int m_GameProcess;
	    private int m_ShowInter_Interval;

        private float m_TimeWeight;
	    private float m_RewardWeight;
	    private float m_InterWeight;
	    private float m_ProcessWeight;
	    private int m_SecInterTime;

	    private List<int> m_ShowInterTime;
	    private bool m_InterIsReady;
	    private int m_IntTimerCheck;
	    private int[] m_UserGradeValue = new int[userGradeCount];
	    private int m_UserGradeIndex;

	    public override void OnSingletonInit()
	    {
	        m_PlayMinTime = PlayerPrefs.GetInt(AdsAnalysisDefine.PLAY_MINTIME, 0);
	        m_RewardShowCount = PlayerPrefs.GetInt(AdsAnalysisDefine.REWARD_SHOW_COUNT, 0);
	        m_InterShowCount = PlayerPrefs.GetInt(AdsAnalysisDefine.INTER_SHOW_COUNT, 0);
	        m_GameProcess = PlayerPrefs.GetInt(AdsAnalysisDefine.GAME_PROCESS, 0);
	        m_ShowInter_Interval = PlayerPrefs.GetInt(AdsAnalysisDefine.INTER_INTERVAL, defaultInterVal);
	        m_UserGradeIndex = PlayerPrefs.GetInt(AdsAnalysisDefine.USER_GRADE, 0);
            m_InterIsReady = false;

            ParseWeightData();
	        InitUserGrade();

            Timer.S.Post2Really(StartPassTimeCount, 60, -1);
	        m_IntTimerCheck = StartIntervalTimer();
	    }

	    public void AddRewardShowCount()
	    {
	        m_RewardShowCount++;
            PlayerPrefs.SetInt(AdsAnalysisDefine.REWARD_SHOW_COUNT, m_RewardShowCount);
	        CalInterval();
	    }

	    public void AddInterShowCount()
	    {
	        m_InterShowCount++;
            PlayerPrefs.SetInt(AdsAnalysisDefine.INTER_SHOW_COUNT, m_InterShowCount);
	        CalInterval();
	        m_IntTimerCheck = StartIntervalTimer();
	    }

	    public void SetGameProcess()
	    {
	        m_GameProcess++;
            PlayerPrefs.SetInt(AdsAnalysisDefine.GAME_PROCESS, m_GameProcess);
	        CalInterval();
        }

        void StartPassTimeCount(int min)
	    {
	        m_PlayMinTime += min;
            PlayerPrefs.SetInt(AdsAnalysisDefine.PLAY_MINTIME, m_PlayMinTime);
	        CalInterval();
        }

	    int StartIntervalTimer()
	    {
	        return Timer.S.Post2Really(StartIntervalCheck, 1, -1);
	    }

	    void StartIntervalCheck(int second)
	    {
            m_InterIsReady = false;

            if (second > m_ShowInter_Interval)
	        {
	            m_InterIsReady = true;
	            Timer.S.Cancel(m_IntTimerCheck);
	            m_IntTimerCheck = -1;
	        }
	    }


        void ParseWeightData()
	    {
	        m_TimeWeight = TDRemoteConfigTable.QueryFloat(AdsAnalysisDefine.TIME_WEIGHT);
	        m_RewardWeight = TDRemoteConfigTable.QueryFloat(AdsAnalysisDefine.REWARD_WEIGHT);
	        m_InterWeight = TDRemoteConfigTable.QueryFloat(AdsAnalysisDefine.INTER_WEIGHT);
	        m_ProcessWeight = TDRemoteConfigTable.QueryFloat(AdsAnalysisDefine.PROCESS_WEIGHT);

	        m_ShowInterTime = TDRemoteConfigTable.QueryIntList(AdsAnalysisDefine.SHOW_INTER_TIME);
	        m_SecInterTime = TDRemoteConfigTable.QueryInt(AdsAnalysisDefine.SEC_INTER_TIME, -1);
	    }

	    void CalInterval()
	    {
	        float totalWeight = m_TimeWeight * m_PlayMinTime + m_RewardWeight * m_RewardShowCount + m_InterWeight * m_InterShowCount + m_ProcessWeight * m_GameProcess;
	        if (m_ShowInterTime != null && m_ShowInterTime.Count > 1)
	        {
	            m_ShowInter_Interval = m_ShowInterTime[0] - (int)totalWeight * m_ShowInterTime[1];
	            if (m_ShowInter_Interval < 0)
	            {
	                m_ShowInter_Interval = 0;
	            }
	            PlayerPrefs.SetInt(AdsAnalysisDefine.INTER_INTERVAL, m_ShowInter_Interval);
            }
	        SetUserGrade();
	    }

	    void InitUserGrade()
	    {
	        if (m_ShowInterTime != null)
	        {
                int grade = m_ShowInterTime[0] / m_UserGradeValue.Length;

                for (int i = 0; i < m_UserGradeValue.Length; i++)
                {
                    m_UserGradeValue[i] = grade * i;
                }
	        }
	        SetUserGrade();
        }

	    void SetUserGrade()
	    {
	        for (int i = 0; i < m_UserGradeValue.Length; i++)
	        {
	            if (m_ShowInter_Interval <= m_UserGradeValue[i])
	            {
	                m_UserGradeIndex = m_UserGradeValue.Length - i;
                    break;
	            }
	        }

	        if (m_UserGradeIndex != PlayerPrefs.GetInt(AdsAnalysisDefine.USER_GRADE,0))
	        {
	            PlayerPrefs.SetInt(AdsAnalysisDefine.USER_GRADE, m_UserGradeIndex);
                DataAnalysisMgr.S.CustomEventWithDate(AdsAnalysisDefine.USER_GRADE, String.Format("{0}{1}", AdsAnalysisDefine.USER_GRADE, m_UserGradeIndex));
	        }
            Debug.Log("------------------------gradeIndex:"+ m_UserGradeIndex);
	    }

        public bool IsInterAvailable()
	    {
	        return m_InterIsReady;
	    }

	    public bool IsSecInterAvaliable()
	    {
	        if (m_SecInterTime >= m_ShowInter_Interval)
	        {
	            int rangeNum = RandomHelper.Range(0, m_UserGradeValue.Length);
	            if (rangeNum <= m_UserGradeIndex)
	            {
	                return true;
	            }
            }

	        return false;
	    }
    }
}

