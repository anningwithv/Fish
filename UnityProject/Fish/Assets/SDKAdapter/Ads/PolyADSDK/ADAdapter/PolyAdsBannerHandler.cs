using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using Polymer;

namespace Qarth
{
    public class PolyAdsBannerHandler : AdBannerHandler
    {
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

            return false;
        }

        protected override bool DoShowAd()
        {
            m_IsShowing = true;
            switch (m_AdInterface.adPosition)
            {
                case AdPosition.Bottom:
                case AdPosition.BottomLeft:
                case AdPosition.BottomRight:
                    UPSDK.showBannerAdAtBottom(m_Config.unitID);
                    break;
                case AdPosition.Top:
                case AdPosition.TopLeft:
                case AdPosition.TopRight:
                    UPSDK.showBannerAdAtTop(m_Config.unitID);
                    break;
            }
            return true;
        }

        protected override bool DoRefreshAd()
        {
            if (!m_IsShowing)
            {
                return false;
            }

            return true;
        }

        public void DestryAD()
        {
            if (!m_IsShowing)
            {
                return;
            }

            m_IsShowing = false;
            UPSDK.removeBannerAdAt(m_Config.unitID);
        }

        protected override void DoHideAd()
        {
            if (!m_IsShowing)
            {
                return;
            }

            m_IsShowing = false;
            DestryAD();
        }

        public override string ToString()
        {
            return "#PolyAdsBanner:" + m_Config.id;
        }
    }
}
