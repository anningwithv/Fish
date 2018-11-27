using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using AudienceNetwork;

namespace Qarth
{
    public class FacebookInterstitialHandler : AdFullScreenHandler
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

                return m_InterstitialAd.IsValid();
            }
        }

        protected override bool DoPreLoadAd()
        {
#if UNITY_IOS
            if (m_InterstitialAd != null)
            {
                return false;
            }
#endif

            if (m_InterstitialAd == null)
            {
                m_InterstitialAd = new InterstitialAd(m_Config.unitID);
                m_InterstitialAd.Register(UIMgr.S.uiRoot.gameObject);
                m_InterstitialAd.InterstitialAdDidLoad = HandleOnAdLoaded;
                m_InterstitialAd.InterstitialAdDidFailWithError += HandleOnAdFailedToLoad;
                m_InterstitialAd.InterstitialAdWillLogImpression += HandleOnAdOpened;
                m_InterstitialAd.InterstitialAdDidClick += HandleOnAdClick;
                m_InterstitialAd.InterstitialAdDidClose += HandleOnAdClosed;
            }

            if (m_InterstitialAd.IsValid())
            {
                return false;
            }

            m_InterstitialAd.LoadAd();

            return true;
        }

        protected override bool DoShowAd()
        {
            if (!isAdReady)
            {
                return false;
            }

            m_InterstitialAd.Show();
            return true;
        }

        protected override void DoCleanAd()
        {
            if (m_InterstitialAd == null)
            {
                return;
            }

//#if UNITY_IOS
            m_InterstitialAd.Dispose();
            m_InterstitialAd = null;            
//#endif
        }

        public override string ToString()
        {
            return "#FacebookInterstitial:" + m_Config.id;
        }
    }
}
