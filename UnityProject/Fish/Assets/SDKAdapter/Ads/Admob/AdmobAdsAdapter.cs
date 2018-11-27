using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using GoogleMobileAds.Api;

namespace Qarth
{
    public class AdmobAdsAdapter : AbstractAdsAdapter
    {
        private static AdmobAdsConfig m_Config;

        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            //AudienceNetwork.AdSettings.AddTestDevice("6275f42bceb568babc15c283a4b9de8b");
            
            m_Config = adapterConfig as AdmobAdsConfig;

#if UNITY_ANDROID
            string appId = m_Config.appIDAndroid;
#elif UNITY_IPHONE
            string appId = m_Config.appIDIos;
#else
            string appId = "unexpected_platform";
#endif
            MobileAds.Initialize(appId);
            return true;
        }

        public override string adPlatform
        {
            get
            {
                return "admob";
            }
        }

        public override AdHandler CreateBannerHandler()
        {
            AdHandler handler = new AdmobBannerHandler();
            return handler;
        }

        public override AdHandler CreateInterstitialHandler()
        {
            AdHandler handler = new AdmobInterstitialHandler();
            return handler;
        }

        public override AdHandler CreateRewardVideoHandler()
        {
            AdHandler handler = new AdmobRewardedVideoHandler();
            return handler;
        }

        public static AdRequest BuildRequest(TDAdConfig data)
        {
            var builder = new AdRequest.Builder();
            if (m_Config.isDebugMode)
            {
                builder.AddTestDevice("54A21F94407E31BD8A20879613096F8B");
            }

            if (!string.IsNullOrEmpty(data.keyword))
            {
                builder.AddKeyword(data.keyword);
            }
            builder.SetGender((Gender)data.gender);
            if (data.isBirthDayConfiged)
            {
                builder.SetBirthday(data.GetBirthDayTime());
            }

            if (data.forFamilies)
            {
                builder.AddExtra("is_designed_for_families", "true");
            }

            if (data.forChild)
            {
                builder.TagForChildDirectedTreatment(true);
            }

            return builder.Build();
        }
    }
}
