using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using Polymer;

namespace Qarth
{
    public class PolyAdsInterstitialHandler : AdFullScreenHandler
    {
        protected bool m_HasInit = false;

        public override bool isAdReady
        {
            get
            {
                if (!m_HasInit)
                {
                    return false;
                }

                return UPSDK.isInterstitialReady(m_Config.unitID);
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (!m_HasInit)
            {
                m_HasInit = true;

                UPSDK.UPInterstitialDidShowCallback = HandleOnAdOpen;
                UPSDK.UPInterstitialDidClickCallback = HandleOnAdClick;
                UPSDK.UPInterstitialDidCloseCallback = HandleOnAdClose;
                UPSDK.setIntersitialLoadCallback(m_Config.unitID, HandleOnAdLoadSuccess, HandleOnAdLoadFailed);
            }

            return true;
        }

        protected override bool DoShowAd()
        {
            UPSDK.showInterstitialAd(m_Config.unitID);
            return true;
        }

        public override string ToString()
        {
            return "#PolyAdsInterstitialVideo:" + m_Config.id;
        }

        ///////////////////////////
        protected void HandleOnAdLoadSuccess(string placeID, string msg)
        {
            HandleOnAdLoaded();
        }

        protected void HandleOnAdLoadFailed(string placeID, string msg)
        {
            HandleOnAdFailedToLoad(msg);
        }

        protected void HandleOnAdOpen(string placeId, string msg)
        {
            HandleOnAdOpened();
        }

        protected void HandleOnAdClick(string placeId, string msg)
        {
            HandleOnAdClick();
        }

        protected void HandleOnAdClose(string placeId, string msg)
        {
            HandleOnAdClosed();
        }

        protected void HandleOnAdRewarded(string placeId, string msg)
        {
            HandleOnAdRewarded();
        }

        protected void HandleOnAdAbandon(string placeId, string msg)
        {
            
        }
    }
}
