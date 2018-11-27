using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class IronSourceRewardVideoHandler : AdFullScreenHandler
    {
        protected bool m_HasInit = false;

        public override bool isAdReady
        {
            get
            {
                return IronSource.Agent.isRewardedVideoAvailable();
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (!m_HasInit)
            {
                m_HasInit = true;

                IronSourceEvents.onRewardedVideoAdOpenedEvent += HandleOnAdOpened;
                IronSourceEvents.onRewardedVideoAdClosedEvent += HandleOnAdClosed;
                IronSourceEvents.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
                IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += OnRewardedVideoAvailabilityChangedEvent;
            }

            return false;
        }

        public override bool ShowAd()
        {
            if (!isAdReady)
            {
                return false;
            }

            if (DoShowAd())
            {
                m_AdState = AdState.Showing;
            }

            return m_AdState == AdState.Showing;
        }

        protected void OnRewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            HandleOnAdRewarded();
        }

        protected void OnRewardedVideoAvailabilityChangedEvent(bool state)
        {
            if (state)
            {
                HandleOnAdLoaded();
            }
        }
    }
}
