using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;

namespace Qarth
{
    public class AdmobRewardVideoInstance : TSingleton<AdmobRewardVideoInstance>
    {
        public interface IRewardVideoHandler
        {
            void OnRewardVideoLoaded();
            void OnRewardVideoFailed2Load(string msg);
            void OnRewardVideoOpen();
            void OnRewardVideoClose();
            void OnRewardLeftApplication();
            void OnRewardVideoRewarded();

            TDAdConfig GetAdConfig();
        }

        private RewardBasedVideoAd m_RewardBasedVideoAd;
        private IRewardVideoHandler m_RewardVideoHandler;

        public bool isAdReady
        {
            get
            {
                if (m_RewardBasedVideoAd == null)
                {
                    return false;
                }

#if UNITY_EDITOR
                return true;
#endif
                return m_RewardBasedVideoAd.IsLoaded();
            }
        }

        public bool Bind(IRewardVideoHandler handler)
        {
            if (m_RewardVideoHandler != null)
            {
                return false;
            }

            m_RewardVideoHandler = handler;

            return true;
        }

        public void UnBind(IRewardVideoHandler handler)
        {
            if (m_RewardVideoHandler == handler)
            {
                m_RewardVideoHandler = null;
            }
        }

        public bool ShowAd()
        {
            m_RewardBasedVideoAd.Show();
            return true;
        }

        public bool PreLoadAd()
        {
            if (m_RewardVideoHandler == null)
            {
                return false;
            }

            if (m_RewardBasedVideoAd == null)
            {
                m_RewardBasedVideoAd = RewardBasedVideoAd.Instance;

                m_RewardBasedVideoAd.OnAdLoaded += HandleOnAdLoaded;
                m_RewardBasedVideoAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
                m_RewardBasedVideoAd.OnAdOpening += HandleOnAdOpened;
                m_RewardBasedVideoAd.OnAdClosed += HandleOnAdClosed;
                m_RewardBasedVideoAd.OnAdLeavingApplication += HandleOnAdLeftApplication;
                m_RewardBasedVideoAd.OnAdStarted += HandleOnAdStarted;
                m_RewardBasedVideoAd.OnAdRewarded += HandleOnAdRewarded;
            }

            if (m_RewardBasedVideoAd.IsLoaded())
            {
                return false;
            }

            var adConfig = m_RewardVideoHandler.GetAdConfig();
            m_RewardBasedVideoAd.LoadAd(AdmobAdsAdapter.BuildRequest(adConfig), adConfig.unitID);

            return true;
        }

        protected void HandleOnAdLoaded(object sender, EventArgs args)
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoLoaded();
        }

        protected void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoFailed2Load(args.Message);
        }

        //统计点击
        protected void HandleOnAdOpened(object sender, EventArgs args)
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoOpen();
        }

        //恢复操作
        protected void HandleOnAdClosed(object sender, EventArgs args)
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoClose();
        }

        protected void HandleOnAdLeftApplication(object sender, EventArgs args)
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardLeftApplication();
        }

        protected void HandleOnAdStarted(object sender, EventArgs args)
        {

        }

        protected void HandleOnAdRewarded(object sender, Reward args)
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoRewarded();
        }
    }
}
