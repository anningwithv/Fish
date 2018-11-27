using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ApplovinInterstitialHandler : AdFullScreenHandler, ApplovinInterstitialInstance.IInterstitialHandler
    {
        protected ApplovinInterstitialInstance m_Instance;

        public override bool isAdReady
        {
            get
            {
                if (m_Instance == null)
                {
                    return false;
                }

                return m_Instance.isAdReady;
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (m_Instance == null)
            {
                if (ApplovinInterstitialInstance.S.Bind(this))
                {
                    m_Instance = ApplovinInterstitialInstance.S;

                    return m_Instance.PreLoadAd();
                }
                return false;
            }
            else
            {
                return m_Instance.PreLoadAd();
            }
        }

        protected override bool DoShowAd()
        {
            if (m_Instance == null)
            {
                return false;
            }

            return m_Instance.ShowAd();
        }

        protected override void DoCleanAd()
        {
            if (m_Instance != null)
            {
                m_Instance.UnBind(this);
                m_Instance = null;
            }
        }

        public void OnInterstitialLoaded()
        {
            HandleOnAdLoaded();
        }

        public void OnInterstitialFailed2Load(string msg)
        {
            HandleOnAdFailedToLoad(msg);
        }

        public void OnInterstitialOpen()
        {
            HandleOnAdOpened();
        }

        public void OnInterstitialClose()
        {
            HandleOnAdClosed();
        }

        public void OnInterstitialLeftApplication()
        {
            HandleOnAdLeftApplication();
        }

        public TDAdConfig GetAdConfig()
        {
            return m_Config;
        }
    }
}
