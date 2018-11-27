using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameWish.Game;
using Qarth;
using UnityEngine.Advertisements;

namespace Qarth
{
    public class UnityAdsAdapter : AbstractAdsAdapter
    {
        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            string gameID = GetAdsGameID(config);
            if (string.IsNullOrEmpty(gameID))
            {
                Log.w("Invalid Unity Ads Config.");
                return false;
            }
            if (Advertisement.isSupported)
            {
                Advertisement.Initialize(gameID);
                return true;
            }
            return false;
        }

        private string GetAdsGameID(SDKConfig config)
        {            
#if UNITY_ANDROID
            return config.adsConfig.unityConfig.UnityGameID_Android;
#elif UNITY_IPHONE
            return config.adsConfig.unityConfig.UnityGameID_iOS;
#else
            return null;
#endif
        }

        public override string adPlatform
        {
            get
            {
                return "unity";
            }
        }

        public override AdHandler CreateRewardVideoHandler()
        {
            return new UnityRewardVideoHandler();
        }

        public override AdHandler CreateInterstitialHandler()
        {
            return new UnityInterstitialHandler();
        }
    }
}
