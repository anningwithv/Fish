using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ApplovinAdsAdapter : AbstractAdsAdapter
    {
        private static AdmobAdsConfig m_Config;

        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
#if !UNITY_EDITOR
            AppLovin.InitializeSdk();
            //AppLovin.SetTestAdsEnabled(m_Config.isDebugMode);
            AppLovin.SetUnityAdListener("ApplovinEventCenter");
#endif
            return true;
        }

        public override string adPlatform
        {
            get
            {
                return "applovin";
            }
        }

#if !UNITY_EDITOR
        public override AdHandler CreateBannerHandler()
        {
            AdHandler handler = new ApplovinBannerHandler();
            return handler;
        }

        public override AdHandler CreateInterstitialHandler()
        {
            AdHandler handler = new ApplovinInterstitialHandler();
            return handler;
        }

        public override AdHandler CreateRewardVideoHandler()
        {
            AdHandler handler = new ApplovinRewardVideoHandler();
            return handler;
        }
#endif
    }
}
