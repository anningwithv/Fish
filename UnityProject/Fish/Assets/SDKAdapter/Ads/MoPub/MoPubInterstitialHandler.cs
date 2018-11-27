using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using MoPubInternal;


namespace Qarth
{
    public class MoPubInterstitialHandler : AdFullScreenHandler
    {
        private bool m_IsInitUnitId = false;
        public override bool isAdReady
        {
            get
            {
                if (!m_IsInitUnitId)
                {
                    return false;
                }
                return MoPub.IsInterstialReady(m_Config.unitID);
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (string.IsNullOrEmpty(m_Config.unitID))
            {
                return false;
            }
            MoPub.RequestInterstitialAd(m_Config.unitID);

            if (!m_IsInitUnitId)
            {
                m_IsInitUnitId = true;
            }
            return true;
        }

        public override void SetAdConfig(TDAdConfig config)
        {
            base.SetAdConfig(config);
            MoPub.LoadInterstitialPluginsForAdUnits(new string[] { m_Config.unitID });

            MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
            MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
            MoPubManager.OnInterstitialShownEvent += OnInterstitialShownEvent;
            MoPubManager.OnInterstitialClickedEvent += OnInterstitialClickedEvent;
            MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;
            MoPubManager.OnInterstitialExpiredEvent += OnInterstitialExpiredEvent;
        }

        protected override bool DoShowAd()
        {
            if (!isAdReady)
            {
                return false;
            }

            MoPub.ShowInterstitialAd(m_Config.unitID);
            return true;
        }

        protected override void DoCleanAd()
        {
            if (m_Config.unitID == null)
            {
                return;
            }
            MoPub.DestroyInterstitialAd(m_Config.unitID);
        }

        public override string ToString()
        {
            return "#MopubInterstitial:" + m_Config.id;
        }
        private void OnInterstitialLoadedEvent(string adUnitId)
        {
            ThreadMgr.S.mainThread.PostAction(ProcessAdLoadedAction);
        }

        private void OnInterstitialFailedEvent(string adUnitId, string error)
        {
            HandleOnAdFailedToLoad(error);
        }

        private void OnInterstitialShownEvent(string adUnitId)
        {
            HandleOnAdOpened();
        }

        private void OnInterstitialClickedEvent(string adUnitId)
        {
            HandleOnAdClick();
        }

        private void OnInterstitialDismissedEvent(string adUnitId)
        {
            HandleOnAdClosed();
        }


        private void OnInterstitialExpiredEvent(string adUnitId)
        {
            HandleOnAdClosed();
        }
    }
}
