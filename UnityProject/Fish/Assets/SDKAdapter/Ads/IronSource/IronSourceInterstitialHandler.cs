using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class IronSourceInterstitialHandler : AdFullScreenHandler
    {
        protected bool m_HasInit = false;

        public override bool isAdReady
        {
            get
            {
                return IronSource.Agent.isInterstitialReady();
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (!m_HasInit)
            {
                m_HasInit = true;

                IronSourceEvents.onInterstitialAdOpenedEvent += HandleOnAdOpened;
                IronSourceEvents.onInterstitialAdClosedEvent += HandleOnAdClosed;
                IronSourceEvents.onInterstitialAdLoadFailedEvent += OnInterstitialAdLoadFailedEvent;
                IronSourceEvents.onInterstitialAdReadyEvent += HandleOnAdLoaded;
            }

            if (isAdReady)
            {
                return false;
            }

            IronSource.Agent.loadInterstitial();

            return true;
        }

        protected override bool DoShowAd()
        {
            IronSource.Agent.showInterstitial();

            return true;
        }

        protected void OnInterstitialAdLoadFailedEvent(IronSourceError error)
        {
            HandleOnAdFailedToLoad(error.getDescription());
        }
    }
}
