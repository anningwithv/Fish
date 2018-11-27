using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class MoPubBannerHandler : AdBannerHandler
    {
        private readonly Dictionary<string, bool> m_AdBannerLoadedMapping = new Dictionary<string, bool>();
        private readonly Dictionary<string, bool> m_AdBannerShownMapping = new Dictionary<string, bool>();
        private bool m_HasInit = false;

        public override bool isAdReady
        {
            get
            {
                if (m_Config == null || !m_AdBannerLoadedMapping.ContainsKey(m_Config.unitID))
                {
                    return false;
                }
                return m_AdBannerLoadedMapping[m_Config.unitID];
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (string.IsNullOrEmpty(m_Config.unitID))
            {
                return false;
            }

            if (m_AdBannerLoadedMapping.Count == 0)
            {
                m_AdBannerLoadedMapping.Add(m_Config.unitID, false);
                m_AdBannerShownMapping.Add(m_Config.unitID, false);

                MoPubManager.OnAdLoadedEvent += OnAdLoadedEvent;
                MoPubManager.OnAdFailedEvent += OnAdFailedEvent;
                MoPubManager.OnAdClickedEvent += OnAdClickedEvent;
                MoPubManager.OnAdCollapsedEvent += OnAdCollapsedEvent;
            }
            
            CreateBanner();
            
            return true;
        }

        public override void SetAdConfig(TDAdConfig config)
        {
            base.SetAdConfig(config);
            MoPub.LoadBannerPluginsForAdUnits(new string[] { m_Config.unitID });
        }

        protected override bool DoShowAd()
        {
            if (m_Config.unitID == null)
            {
                return false;
            }

            m_IsShowing = true;
            MoPub.ShowBanner(m_Config.unitID,true);

            return true;
        }

        protected override bool DoRefreshAd()
        {
            if (!m_IsShowing)
            {
                return false;
            }

            m_AdState = AdState.Loading;

            m_AdBannerLoadedMapping[m_Config.unitID] = false;
            m_AdBannerShownMapping[m_Config.unitID] = false;
            CreateBanner();

            return true;
        }

        protected override void DoHideAd()
        {
            if (m_Config.unitID == null)
            {
                return;
            }

            m_IsShowing = false;
            MoPub.ShowBanner(m_Config.unitID, false);
        }
        private void OnAdLoadedEvent(string adUnitId, float height)
        {
            m_AdBannerLoadedMapping[adUnitId] = true;
            m_AdBannerShownMapping[adUnitId] = true;
            HandleOnAdLoaded();
        }

        private void OnAdFailedEvent(string adUnitId, string error)
        {
            HandleOnAdFailedToLoad(error);
        }

        private void OnAdClickedEvent(string adUnitId)
        {
            HandleOnAdClick();
        }

        private void OnAdCollapsedEvent(string adUnitId)
        {
            HandleOnAdClosed();
        }


        private void CreateBanner()
        {
            switch (m_AdInterface.adPosition)
            {
                case AdPosition.Top:
                    MoPub.CreateBanner(m_Config.unitID, MoPubBase.AdPosition.TopCenter);
                    break;

                case AdPosition.Bottom:
                    MoPub.CreateBanner(m_Config.unitID, MoPubBase.AdPosition.BottomCenter);
                    break;

                case AdPosition.TopLeft:
                    MoPub.CreateBanner(m_Config.unitID, MoPubBase.AdPosition.TopLeft);
                    break;

                case AdPosition.TopRight:
                    MoPub.CreateBanner(m_Config.unitID, MoPubBase.AdPosition.TopRight);
                    break;

                case AdPosition.BottomLeft:
                    MoPub.CreateBanner(m_Config.unitID, MoPubBase.AdPosition.BottomLeft);
                    break;

                case AdPosition.BottomRight:
                    MoPub.CreateBanner(m_Config.unitID, MoPubBase.AdPosition.BottomRight);
                    break;

                case AdPosition.Center:
                    MoPub.CreateBanner(m_Config.unitID, MoPubBase.AdPosition.Centered);
                    break;

                case AdPosition.CustomDefine: //自定义位置不支持
                    MoPub.CreateBanner(m_Config.unitID, MoPubBase.AdPosition.BottomCenter);
                    break;
                default:
                    MoPub.CreateBanner(m_Config.unitID, MoPubBase.AdPosition.BottomCenter);
                    break;
            }
        }



    }
}
