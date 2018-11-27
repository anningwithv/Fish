using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class MoPubRewardVideoHandler : AdFullScreenHandler
    {
        private bool m_IsUnitIdInit = false;
        public override bool isAdReady
        {
            get
            {
                if (!m_IsUnitIdInit)
                {
                    return false;
                }
                return MoPub.HasRewardedVideo(m_Config.unitID);
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (string.IsNullOrEmpty(m_Config.unitID))
            {
                return false;
            }
            
            MoPub.RequestRewardedVideo(m_Config.unitID);

            if (!m_IsUnitIdInit)
            {
                m_IsUnitIdInit = true;
            }
            return true;
        }

        public override void SetAdConfig(TDAdConfig config)
        {
            base.SetAdConfig(config);
            MoPub.LoadRewardedVideoPluginsForAdUnits(new string[] { m_Config.unitID });

            MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
            MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
            MoPubManager.OnRewardedVideoExpiredEvent += OnRewardedVideoExpiredEvent;
            MoPubManager.OnRewardedVideoShownEvent += OnRewardedVideoShownEvent;
            MoPubManager.OnRewardedVideoClickedEvent += OnRewardedVideoClickedEvent;
            MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
            MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
            MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
            MoPubManager.OnRewardedVideoLeavingApplicationEvent += OnRewardedVideoLeavingApplicationEvent;

        }

        protected override bool DoShowAd()
        {
            MoPub.ShowRewardedVideo(m_Config.unitID);
            return true;
        }

        protected override void DoCleanAd()
        {
            if (m_Config.unitID == null)
            {
                return;
            }
        }

        public override string ToString()
        {
            return "#MopubRewardVideo:" + m_Config.id;
        }

        private void OnRewardedVideoLoadedEvent(string adUnitId)
        {
            HandleOnAdLoaded();
        }

        private void OnRewardedVideoFailedEvent(string adUnitId, string error)
        {
            HandleOnAdFailedToLoad(error);
        }

        private void OnRewardedVideoExpiredEvent(string adUnitId)
        {
            HandleOnAdClosed();
        }
        private void OnRewardedVideoShownEvent(string adUnitId)
        {
            HandleOnAdOpened();
        }

        private void OnRewardedVideoClickedEvent(string adUnitId)
        {
            HandleOnAdClick();
        }

        private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
        {
            HandleOnAdClosed();
        }

        private void OnRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
        {
            HandleOnAdRewarded();
        }

        private void OnRewardedVideoClosedEvent(string adUnitId)
        {
            HandleOnAdClosed();
        }

        private void OnRewardedVideoLeavingApplicationEvent(string adUnitId)
        {
            HandleOnAdClosed();
            HandleOnAdLeftApplication();
        }
    }
}
