using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using AudienceNetwork;

namespace Qarth
{
    public class FacebookRewardVideoAdHandler : AdFullScreenHandler
    {
        private RewardedVideoAd m_RewardVideoAd;

        public override bool isAdReady
        {
            get
            {
                if (m_RewardVideoAd == null)
                {
                    return false;
                }

                return m_RewardVideoAd.IsValid();
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (m_RewardVideoAd != null && m_RewardVideoAd.IsValid())
            {
                return false;
            }

            if (m_RewardVideoAd == null)
            {
                m_RewardVideoAd = new RewardedVideoAd(m_Config.unitID);
                m_RewardVideoAd.Register(UIMgr.S.uiRoot.gameObject);
                m_RewardVideoAd.RewardedVideoAdDidLoad = HandleOnAdLoaded;
                m_RewardVideoAd.RewardedVideoAdDidFailWithError += HandleOnAdFailedToLoad;
                m_RewardVideoAd.RewardedVideoAdWillLogImpression += HandleOnAdLogImpression;
                m_RewardVideoAd.RewardedVideoAdDidClick += HandleOnAdClick;
                m_RewardVideoAd.RewardedVideoAdDidClose += HandleOnFBAdClose;
                m_RewardVideoAd.RewardedVideoAdComplete += HandleOnAdComplate;
                m_RewardVideoAd.RewardedVideoAdDidSucceed += HandleOnAdSuccess;
            }

            m_RewardVideoAd.LoadAd();

            return true;
        }

        protected override bool DoShowAd()
        {
            m_RewardVideoAd.Show();
            return true;
        }

        protected override void DoCleanAd()
        {
            if (m_RewardVideoAd == null)
            {
                return;
            }

//#if UNITY_IOS
            m_RewardVideoAd.Dispose();
            m_RewardVideoAd = null;            
//#endif
        }

        public override string ToString()
        {
            return "#FacebookRewardVideo:" + m_Config.id;
        }

        ///////////////////////////
        protected void HandleOnFBAdClose()
        {
            HandleOnAdClosed();
        }

        protected void HandleOnAdComplate()
        {
            m_RewardVideoAd.RewardedVideoAdDidFailWithError -= HandleOnAdFailedToLoad;
            HandleOnAdRewarded();
        }

        protected void HandleOnAdLogImpression()
        {
            HandleOnAdOpened();
        }

        //统计点击
        protected void HandleOnAdSuccess()
        {
        }
    }
}
