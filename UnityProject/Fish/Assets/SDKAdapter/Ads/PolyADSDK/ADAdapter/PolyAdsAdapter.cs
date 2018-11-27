using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using Polymer;

namespace Qarth
{
    public class PolyAdsAdapter : AbstractAdsAdapter
    {
        private PolyAdsConfig m_Config;
        public static bool isSDKInitFinish;

        public override string adPlatform
        {
            get
            {
                return "upltv";
            }
        }

        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            m_Config = adapterConfig as PolyAdsConfig;

            if (!m_Config.isLibEnable)
            {
                return false;
            }

            UPSDK.UPSDKInitFinishedCallback = OnSDKInitFinish;

            UPSDK.initPolyAdSDK(0);

            return true;
        }

        protected void OnSDKInitFinish(bool result, string msg)
        {
            isSDKInitFinish = result;
        }

        public override AdHandler CreateRewardVideoHandler()
        {
            return new PolyAdsRewardedVideoHandler();
        }

        public override AdHandler CreateBannerHandler()
        {
            return new PolyAdsBannerHandler();
        }
    }
}
