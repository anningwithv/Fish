using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using Polymer;

namespace Qarth
{
    public class PolyAdsRewardedVideoHandler : AdFullScreenHandler
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

                return UPSDK.isRewardReady();
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (!m_HasInit)
            {
                m_HasInit = true;

                UPSDK.UPRewardDidOpenCallback = HandleOnAdOpen;
                UPSDK.UPRewardDidClickCallback = HandleOnAdClick;
                UPSDK.UPRewardDidCloseCallback = HandleOnAdClose;
                UPSDK.UPRewardDidGivenCallback = HandleOnAdRewarded;
                UPSDK.UPRewardDidAbandonCallback = HandleOnAdAbandon;
                UPSDK.setRewardVideoLoadCallback(HandleOnAdLoadSuccess, HandleOnAdLoadFailed);
            }

            return true;
        }

        protected override bool DoShowAd()
        {
            UPSDK.showRewardAd(m_Config.unitID);
            return true;
        }

        public override string ToString()
        {
            return "#PolyAdsRewardVideo:" + m_Config.id;
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
