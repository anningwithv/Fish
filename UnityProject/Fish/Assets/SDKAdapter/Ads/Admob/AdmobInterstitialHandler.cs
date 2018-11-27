using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using GoogleMobileAds.Api;

namespace Qarth
{
    public class AdmobInterstitialHandler : AdFullScreenHandler
    {
        private InterstitialAd m_InterstitialAd;

        public override bool isAdReady
        {
            get
            {
                if (m_InterstitialAd == null)
                {
                    return false;
                }

                return m_InterstitialAd.IsLoaded();
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (string.IsNullOrEmpty(m_Config.unitID))
            {
                return false;
            }

#if UNITY_IOS
            if (m_InterstitialAd != null)
            {
                return false;
            }
#endif

            if (m_InterstitialAd == null)
            {
                m_InterstitialAd = new InterstitialAd(m_Config.unitID);

                m_InterstitialAd.OnAdLoaded += HandleOnAdLoaded;
                m_InterstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
                m_InterstitialAd.OnAdOpening += HandleOnAdOpened;
                m_InterstitialAd.OnAdClosed += HandleOnAdClosed;
                m_InterstitialAd.OnAdLeavingApplication += HandleOnAdLeftApplication;
            }

            if (m_InterstitialAd.IsLoaded())
            {
                return false;
            }

            m_InterstitialAd.LoadAd(AdmobAdsAdapter.BuildRequest(m_Config));
            //DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.INTERSTITIAL_STATE, ADLabelDefine.LOADED_REQUEST);
            return true;
        }

        protected override bool DoShowAd()
        {
            m_InterstitialAd.Show();
            //DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.INTERSTITIAL_STATE, ADLabelDefine.SHOW);
            return true;
        }

        protected override void DoCleanAd()
        {
            if (m_InterstitialAd == null)
            {
                return;
            }

#if UNITY_IOS
            m_InterstitialAd.Destroy();
            m_InterstitialAd = null;            
#endif
        }

        public override string ToString()
        {
            return "#AdmobInterstitial:" + m_Config.id;
        }

        ///////////////////////////

        protected void HandleOnAdLoaded(object sender, EventArgs args)
        {
            HandleOnAdLoaded();
        }

        protected void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            HandleOnAdFailedToLoad(args.Message);
        }

        //统计点击
        protected void HandleOnAdOpened(object sender, EventArgs args)
        {
            HandleOnAdOpened();
        }

        //恢复操作
        protected void HandleOnAdClosed(object sender, EventArgs args)
        {
            HandleOnAdClosed();
        }

        protected void HandleOnAdLeftApplication(object sender, EventArgs args)
        {
            HandleOnAdLeftApplication();
        }
    }
}
