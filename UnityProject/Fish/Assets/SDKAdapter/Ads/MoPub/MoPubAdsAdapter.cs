using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class MoPubAdsAdapter : AbstractAdsAdapter
    {
        private MoPubAdsConfig m_Config;

        public override string adPlatform
        {
            get
            {
                return "mopub";
            }
        }

        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            m_Config = adapterConfig as MoPubAdsConfig;
#if UNITY_ANDROID
            string appId = m_Config.appIDAndroid;
#elif UNITY_IPHONE
            string appId = m_Config.appIDIos;
#else
            string appId = "unexpected_platform";
#endif

            string adUnit = m_Config.anyAdUnit;

            MoPub.InitializeSdk(new MoPubBase.SdkConfiguration
            {
                AdUnitId = adUnit,
                AdvancedBidders = new MoPub.AdvancedBidder[] {
                    MoPub.AdvancedBidder.AppLovin,
                    MoPub.AdvancedBidder.Unity,
                    MoPub.AdvancedBidder.AdMob,
                    MoPub.AdvancedBidder.Facebook
                },
                NetworksToInit = new MoPub.RewardedNetwork[] {
                    MoPub.RewardedNetwork.Unity,
                    MoPub.RewardedNetwork.Facebook,
                    MoPub.RewardedNetwork.AppLovin,
                    MoPub.RewardedNetwork.AdMob
                },
                MediationSettings = new MoPubBase.MediationSetting[] {
                }
            });
            
            MoPub.ReportApplicationOpen(appId);

            return true;
        }

        public override AdHandler CreateBannerHandler()
        {
            AdHandler handler = new MoPubBannerHandler();
            return handler;
        }

        public override AdHandler CreateInterstitialHandler()
        {
            AdHandler handler = new MoPubInterstitialHandler();
            return handler;
        }

        public override AdHandler CreateRewardVideoHandler()
        {
            AdHandler handler = new MoPubRewardVideoHandler();
            return handler;
        }
    }
}


