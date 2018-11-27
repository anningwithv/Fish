using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using AudienceNetwork;
using AudienceNetwork.Utility;
using UnityEngine.UI;

namespace Qarth
{
    public class FacebookAdsAdapter : AbstractAdsAdapter
    {
        protected FacebookAdsConfig m_Config;

        //protected 
        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            m_Config = adapterConfig as FacebookAdsConfig;
            if (m_Config.isDebugMode)
            {
                AdSettings.AddTestDevice("6275f42bceb568babc15c283a4b9de8b");
            }
            return true;
        }

        /*
        public INativeAdHandler GetNativeAdHandler(string name)
        {
            TDAdConfig data = TDAdConfigTable.GetData(name);
            if (data == null || string.IsNullOrEmpty(data.facebook))
            {
                return null;
            }

            if (data.adType != AdType.NativeAD)
            {
                return null;
            }

            if (m_HandlerList == null)
            {
                m_HandlerList = new List<FacebookNativeAdHandler>();
            }

            for (int i = m_HandlerList.Count - 1; i >= 0; --i)
            {
                if (m_HandlerList[i].adName.Equals(name))
                {
                    return m_HandlerList[i];
                }
            }

            GameObject p = new GameObject();

            p.transform.SetParent(NativeAdHandlerPanel.adRoot);
            p.transform.localPosition = Vector3.zero;

            FacebookNativeAdHandler handler = p.AddComponent<FacebookNativeAdHandler>();
            handler.SetAdName(name);

            m_HandlerList.Add(handler);

            return handler;
        }
        */

        public override string adPlatform
        {
            get
            {
                return "facebook";
            }
        }

#if !UNITY_EDITOR

        public override AdHandler CreateBannerHandler()
        {
            AdHandler handler = new FacebookBannerHandler();
            return handler;
        }

        public override AdHandler CreateInterstitialHandler()
        {
            AdHandler handler = new FacebookInterstitialHandler();
            return handler;
        }

        public override AdHandler CreateRewardVideoHandler()
        {
            AdHandler handler = new FacebookRewardVideoAdHandler();
            return handler;
        }
#endif
    }
}
