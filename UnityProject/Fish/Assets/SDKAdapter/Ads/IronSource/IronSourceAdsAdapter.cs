using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class IronSourceAdsAdapter : AbstractAdsAdapter
    {
        private IronSourceAdsConfig m_Config;
        public override string adPlatform
        {
            get
            {
                return "ironsource";
            }
        }

        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            m_Config = adapterConfig as IronSourceAdsConfig;

            Log.i(IronSourceEvents.S.ToString());

            IronSource.Agent.init(m_Config.appKey, IronSourceAdUnits.REWARDED_VIDEO);
            IronSource.Agent.init(m_Config.appKey, IronSourceAdUnits.INTERSTITIAL);
            IronSource.Agent.init(m_Config.appKey, IronSourceAdUnits.BANNER);

            IronSource.Agent.init(m_Config.appKey);

            EventSystem.S.Register(EngineEventID.OnApplicationPauseChange, OnApplicationPauseChange);

            return true;
        }

        public override AdHandler CreateRewardVideoHandler()
        {
            return new IronSourceRewardVideoHandler();
        }

        public override AdHandler CreateInterstitialHandler()
        {
            return new IronSourceInterstitialHandler();
        }

        protected void OnApplicationPauseChange(int key, params object[] args)
        {
            IronSource.Agent.onApplicationPause((bool)args[0]);
        }
    }
}
