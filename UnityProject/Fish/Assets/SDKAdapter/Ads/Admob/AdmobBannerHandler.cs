using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using GoogleMobileAds.Api;

namespace Qarth
{
    public class AdmobBannerHandler : AdBannerHandler, IBannerHandler
    {
        private BannerView m_BannerView;

        public override bool isAdReady
        {
            get
            {
                return m_AdState != AdState.Failed;
            }
        }

        public Vector2 GetBannerSizeInPixel()
        {
            if (m_BannerView == null)
            {
                return Vector2.zero;
            }

            return new Vector2(m_BannerView.GetWidthInPixels(), m_BannerView.GetHeightInPixels());
        }

        protected override bool DoPreLoadAd()
        {
            if (string.IsNullOrEmpty(m_Config.unitID))
            {
                return false;
            }

            if (m_BannerView == null)
            {
                if (m_AdInterface.adPosition == AdPosition.CustomDefine)
                {
                    Vector2Int p = m_AdInterface.adCustomGrid;
                    m_BannerView = new BannerView(m_Config.unitID, ConvertAdSize(m_AdInterface.adSize), p.x, p.y);
                }
                else
                {
                    m_BannerView = new BannerView(m_Config.unitID, ConvertAdSize(m_AdInterface.adSize), ConvertAdPosition(m_AdInterface.adPosition));
                }

                m_BannerView.OnAdLoaded += HandleOnAdLoaded;
                m_BannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
                m_BannerView.OnAdOpening += HandleOnAdOpened;
                m_BannerView.OnAdClosed += HandleOnAdClosed;
                m_BannerView.OnAdLeavingApplication += HandleOnAdLeftApplication;
            }

            return true;
        }

        protected override bool DoShowAd()
        {
            if (m_BannerView == null)
            {
                return false;
            }

            m_IsShowing = true;
            m_BannerView.Show();

            return true;
        }

        protected override bool DoRefreshAd()
        {
            if (!m_IsShowing)
            {
                return false;
            }

            m_AdState = AdState.Loading;

            m_BannerView.LoadAd(AdmobAdsAdapter.BuildRequest(m_Config));

            return true;
        }

        public override void SyncAdPosition()
        {
            if (m_BannerView == null)
            {
                return;
            }

            if (m_AdInterface.adPosition == AdPosition.CustomDefine)
            {
                Vector2Int pos = m_AdInterface.adCustomGrid;
                m_BannerView.SetPosition(pos.x, pos.y);
            }
            else
            {
                m_BannerView.SetPosition(ConvertAdPosition(m_AdInterface.adPosition));
            }
        }

        public void DestryAD()
        {
            if (m_BannerView == null)
            {
                return;
            }

            m_IsShowing = false;
            m_BannerView.Destroy();
            m_BannerView = null;
        }

        protected override void DoHideAd()
        {
            if (m_BannerView == null)
            {
                return;
            }

            m_IsShowing = false;
            m_BannerView.Hide();
        }

        public override string ToString()
        {
            return "#AdmobBanner:" + m_Config.id;
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
            
        }

        protected static GoogleMobileAds.Api.AdSize ConvertAdSize(AdSize size)
        {
            return new GoogleMobileAds.Api.AdSize(size.width, size.height);
        }

        protected static GoogleMobileAds.Api.AdPosition ConvertAdPosition(AdPosition position)
        {
            return (GoogleMobileAds.Api.AdPosition)((int)position);
        }
    }
}
