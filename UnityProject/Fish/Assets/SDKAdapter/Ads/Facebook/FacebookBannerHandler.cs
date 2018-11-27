using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AudienceNetwork;

namespace Qarth
{
    public class FacebookBannerHandler : AdBannerHandler
    {
        private AdView m_BannerView;
        private float screenDensity
        {
            get
            {
                return Screen.dpi / 160;
            }
        }

        public override bool isAdReady
        {
            get
            {
                return m_AdState != AdState.Failed;
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (string.IsNullOrEmpty(m_Config.unitID))
            {
                return false;
            }

            if (m_BannerView == null)
            {
                m_BannerView = new AdView(m_Config.unitID, ConvertAdSize(m_AdInterface.adSize));

                m_BannerView.Register(UIMgr.S.uiRoot.gameObject);

                m_BannerView.AdViewDidLoad += HandleOnAdLoaded;
                m_BannerView.AdViewDidFailWithError += HandleOnAdFailedToLoad;
                m_BannerView.AdViewWillLogImpression += HandleOnAdOpened;

                //m_BannerView.DisableAutoRefresh();
            }

            return true;
        }

        protected AudienceNetwork.AdSize ConvertAdSize(AdSize size)
        {
            if (size.height <= 50)
            {
                return AudienceNetwork.AdSize.BANNER_HEIGHT_50;
            }

            if (size.height <= 90)
            {
                return AudienceNetwork.AdSize.BANNER_HEIGHT_90;
            }

            return AudienceNetwork.AdSize.RECTANGLE_HEIGHT_250;
        }

        protected int CovertAdSizeHeight(AdSize size)
        {
            if (size.height <= 50)
            {
                return 50;
            }

            if (size.height <= 90)
            {
                return 90;
            }

            return 250;
        }

        protected int ConvertAdPosition(AdPosition position, Vector2Int grid, int height)
        {
            if (position == AdPosition.CustomDefine)
            {
                return grid.y - height;
            }
            else
            {
                float y = 0;
                switch (position)
                {
                    case AdPosition.Bottom:
                    case AdPosition.BottomLeft:
                    case AdPosition.BottomRight:
                        y = Screen.height / screenDensity - height;
                        break;
                    case AdPosition.Top:
                    case AdPosition.TopLeft:
                    case AdPosition.TopRight:
                        return 0;
                    case AdPosition.Center:
                        y = Screen.height / screenDensity / 2 - height;
                        break;
                    default:
                        y = Screen.height / screenDensity - height;
                        break;
                }

                return (int)y;
            }
        }

        protected override bool DoShowAd()
        {
            if (m_BannerView == null)
            {
                return false;
            }

            m_IsShowing = true;
            int y = ConvertAdPosition(m_AdInterface.adPosition, m_AdInterface.adCustomGrid, CovertAdSizeHeight(m_AdInterface.adSize));
            m_BannerView.Show(y);
            return true;
        }

        protected override bool DoRefreshAd()
        {
            if (!m_IsShowing)
            {
                return false;
            }

            m_AdState = AdState.Loading;

            m_BannerView.LoadAd();

            return true;
        }

        public void DestryAD()
        {
            if (m_BannerView == null)
            {
                return;
            }

            m_IsShowing = false;
            m_BannerView.Dispose();
            m_BannerView = null;
        }

        protected override void DoHideAd()
        {
            if (m_BannerView == null)
            {
                return;
            }

            m_IsShowing = false;
            DestryAD();
        }

        public override string ToString()
        {
            return "#FacebookBanner:" + m_Config.id;
        }
    }
}

