using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AdEventLogger
    {
        public bool isEnable
        {
            get;
            set;
        }

        public string adPlacement
        {
            get;
            set;
        }

        public string adInterface
        {
            get;
            set;
        }

        private int m_AdType;
        private string m_StateEventKey;
        private string m_IPUEventKey;
        private string m_ImpressionKey;

        public AdEventLogger(int adType)
        {
            m_AdType = adType;
            switch (m_AdType)
            {
                case AdType.Interstitial:
                    m_StateEventKey = DataAnalysisDefine.INTERSTITIAL_STATE;
                    m_IPUEventKey = DataAnalysisDefine.INTERSTITIAL_IPU;
                    m_ImpressionKey = DataAnalysisDefine.IMPRESSION_INTERSTITIAL;
                    break;
                case AdType.RewardedVideo:
                    m_StateEventKey = DataAnalysisDefine.REWARD_VIDEO_STATE;
                    m_IPUEventKey = DataAnalysisDefine.REWARD_VIDEO_IPU;
                    m_ImpressionKey = DataAnalysisDefine.IMPRESSION_VIDEO;
                    break;
                default:
                    break;
            }
        }

        public void Log(string label)
        {
            if (!isEnable)
            {
                return;
            }

            DataAnalysisMgr.S.CustomEvent(m_StateEventKey, label);

            /* 
            if (!string.IsNullOrEmpty(adPlacement))
            {
                DataAnalysisMgr.S.CustomEvent(adPlacement, label);
            }
            */
        }

        public void LogIPU()
        {
            if (!isEnable)
            {
                return;
            }

            DataAnalysisMgr.S.CustomEventWithDate(m_IPUEventKey, "IPU");
            AdAnalysisMgr.S.RecordAdReward(adInterface);
            DataAnalysisMgr.S.CustomEvent(m_ImpressionKey);
        }

        public void LogIPUInit()
        {
            if (!isEnable)
            {
                return;
            }

            DataAnalysisMgr.S.CustomEventDailySingle(m_IPUEventKey, "INIT");
        }
    }
}
