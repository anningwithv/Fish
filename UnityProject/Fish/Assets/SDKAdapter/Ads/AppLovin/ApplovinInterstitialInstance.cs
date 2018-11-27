using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ApplovinInterstitialInstance : TSingleton<ApplovinInterstitialInstance>
    {
        public interface IInterstitialHandler
        {
            void OnInterstitialLoaded();
            void OnInterstitialFailed2Load(string msg);
            void OnInterstitialOpen();
            void OnInterstitialClose();
            void OnInterstitialLeftApplication();

            TDAdConfig GetAdConfig();
        }

        private IInterstitialHandler m_InterstitialHandler;
        protected bool m_HasInit = false;

        public bool isAdReady
        {
            get
            {
                if (m_InterstitialHandler == null)
                {
                    return false;
                }

                return AppLovin.HasPreloadedInterstitial(m_InterstitialHandler.GetAdConfig().unitID);
            }
        }

        public bool Bind(IInterstitialHandler handler)
        {
            if (m_InterstitialHandler != null)
            {
                return false;
            }

            m_InterstitialHandler = handler;

            return true;
        }

        public void UnBind(IInterstitialHandler handler)
        {
            if (m_InterstitialHandler == handler)
            {
                m_InterstitialHandler = null;
            }
        }

        public bool ShowAd()
        {
            if (m_InterstitialHandler == null)
            {
                return false;
            }

            AppLovin.ShowInterstitialForZoneId(m_InterstitialHandler.GetAdConfig().unitID);
            return true;
        }

        public bool PreLoadAd()
        {
            if (m_InterstitialHandler == null)
            {
                return false;
            }

            if (!m_HasInit)
            {
                m_HasInit = true;
                ApplovinEventCenter.S.on_InterLoaded += HandleOnAdLoaded;
                ApplovinEventCenter.S.on_InterClose += HandleOnAdClosed;
                ApplovinEventCenter.S.on_InterLoadFailed += HandleOnAdFailedToLoad;
            }

            AppLovin.PreloadInterstitial(m_InterstitialHandler.GetAdConfig().unitID);
            return true;
        }

        protected void HandleOnAdLoaded()
        {
            if (m_InterstitialHandler == null)
            {
                return;
            }

            m_InterstitialHandler.OnInterstitialLoaded();
        }

        protected void HandleOnAdFailedToLoad()
        {
            if (m_InterstitialHandler == null)
            {
                return;
            }

            m_InterstitialHandler.OnInterstitialFailed2Load("");
        }

        //统计点击
        protected void HandleOnAdOpened()
        {
            if (m_InterstitialHandler == null)
            {
                return;
            }

            m_InterstitialHandler.OnInterstitialOpen();
        }

        //恢复操作
        protected void HandleOnAdClosed()
        {
            if (m_InterstitialHandler == null)
            {
                return;
            }

            m_InterstitialHandler.OnInterstitialClose();
        }

        protected void HandleOnAdLeftApplication()
        {
            if (m_InterstitialHandler == null)
            {
                return;
            }

            m_InterstitialHandler.OnInterstitialLeftApplication();
        }

        protected void HandleOnAdStarted()
        {

        }
    }
}