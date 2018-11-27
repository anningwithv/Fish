using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AdsMgr : TSingleton<AdsMgr>
    {
        private const string KEY_NO_ADS = "key_noads";

        private bool                m_IsNoAds;

        private bool m_IsInit = false;
        private bool m_IsDataInit = false;

        private Dictionary<string, IAdAdapter> m_AdAdapterMap;
        private Dictionary<string, AdInterface> m_AdInterfaceMap;
        private Dictionary<string, AdPlacement> m_AdPlacementMap;
        private Dictionary<string, AdHandler> m_AdHandlerMap;
        private Dictionary<string, string> m_AfPlatformName;

        public bool isNoAdsMode
        {
            get { return m_IsNoAds; }
            set
            {
                if (m_IsNoAds == value)
                {
                    return;
                }

                m_IsNoAds = value;
                PlayerPrefs.SetInt(KEY_NO_ADS, m_IsNoAds ? 1 : 0);

                if (m_IsNoAds)
                {
                    HideAllAd();
                }

                EventSystem.S.Send(SDKEventID.OnNoAdModeChange);
            }
        }

        public void Init()
        {
            if (m_IsInit)
            {
                return;
            }

            m_IsInit = true;
            m_IsNoAds = PlayerPrefs.GetInt(KEY_NO_ADS, 0) > 0;

            InitSupportedAdapter(SDKConfig.S);

            Log.i("Init[AdsMgr]");
        }

        public void InitAllAdData()
        {
            if (m_IsDataInit)
            {
                return;
            }

            m_IsDataInit = true;
            //不要调换顺序
            InitRemoteConfig();
            InitAdapterData();
            InitAdInterfaceMap();
            InitAdPlacementMap();
        }

        public void ReInitAllAdData()
        {
            //不要调换顺序
            InitRemoteConfig();

            foreach (var item in m_AdInterfaceMap.Values)
            {
                item.RebuildAdHandlerList();
            }
        }

        public void PreloadAllAd()
        {
            foreach (var item in m_AdInterfaceMap.Values)
            {
                if (item.adType == AdType.Interstitial || item.adType == AdType.RewardedVideo)
                {
                    item.PreLoadAd();
                }
            }
        }

        protected void InitRemoteConfig()
        {
            int maxLoadingCount = TDRemoteConfigTable.QueryInt("ad_max_loading_count");
            if (maxLoadingCount > 0)
            {
                AdInterface.MAX_LOADING_AD_COUNT = maxLoadingCount;
            }

            int maxLoadedCount = TDRemoteConfigTable.QueryInt("ad_max_loaded_count");
            if (maxLoadedCount > 0)
            {
                AdInterface.MAX_LOADED_AD_COUNT = maxLoadedCount;
            }

            float fulladLoadOffset = TDRemoteConfigTable.QueryFloat("ad_fullscreen_load_offset");
            if (fulladLoadOffset > 1)
            {
                AdInterface.FULLSCREEN_AD_LOAD_OFFSET = fulladLoadOffset;
            }
        }

        protected void InitAdapterData()
        {
            foreach(var item in m_AdAdapterMap.Values)
            {
                item.InitWithData();
            }
        }

        protected void InitAdPlacementMap()
        {
            m_AdPlacementMap = new Dictionary<string, AdPlacement>();

            var datas = TDAdPlacementTable.dataList;

            for (int i = 0; i < datas.Count; ++i)
            {
                AdPlacement unit = new AdPlacement(datas[i]);
                m_AdPlacementMap.Add(datas[i].id, unit);
                unit.CheckAdInterfaceValid();
            }
        }

        protected void InitAdInterfaceMap()
        {
            m_AdInterfaceMap = new Dictionary<string, AdInterface>();
            m_AdHandlerMap = new Dictionary<string, AdHandler>();

            var datas = TDAdConfigTable.dataList;
            for (int i = 0; i < datas.Count; ++i)
            {
                var data = datas[i];

                AdInterface adInterface = null;

                if (m_AdInterfaceMap.TryGetValue(data.adInterface, out adInterface))
                {
                    continue;
                }

                switch (data.adType)
                {
                    case AdType.Banner:
                        adInterface = new AdBannerInterface();
                        break;
                    case AdType.Interstitial:
                        adInterface = new AdFullScreenInterface();
                        break;
                    case AdType.RewardedVideo:
                        adInterface = new AdFullScreenInterface();
                        break;
                    case AdType.NativeAD:
                        break;
                    default:
                        break;
                }

                if (adInterface != null)
                {
                    adInterface.InitAdInterface(data.adInterface, data.adType);
                    m_AdInterfaceMap.Add(data.adInterface, adInterface);
                }
            }
        }

        public NativeAdHandler GetNativeAdHandler(string id)
        {
            return null;
        }

        public AdInterface GetAdInterface(string interfaceName)
        {
            AdInterface adInterface = null;
            if (m_AdInterfaceMap.TryGetValue(interfaceName, out adInterface))
            {
                return adInterface;
            }

            return null;
        }

        public AdPlacement GetAdPlacement(string adName)
        {
            AdPlacement result;
            if (m_AdPlacementMap.TryGetValue(adName, out result))
            {
                return result;
            }

            return null;
        }

        public AdHandler CreateAdHandler(TDAdConfig data)
        {
            if (string.IsNullOrEmpty(data.unitID))
            {
                return null;
            }

            if (data.ecpm <= -100000)
            {
                return null;
            }

            if (m_AdHandlerMap.ContainsKey(data.id))
            {
                return null;
            }

            IAdAdapter adAdapter = GetAdAdapter(data.adPlatform);
            if (adAdapter == null)
            {
                Log.e("Not Find AdAdapter For Ad:" + data.id);
                return null;
            }

            AdHandler result = null;

            switch (data.adType)
            {
                case AdType.Banner:
                    result = adAdapter.CreateBannerHandler();
                    break;
                case AdType.Interstitial:
                    result = adAdapter.CreateInterstitialHandler();
                    break;
                case AdType.NativeAD:
                    result = adAdapter.CreateNativeAdHandler();
                    break;
                case AdType.RewardedVideo:
                    result = adAdapter.CreateRewardVideoHandler();
                    break;
                default:
                    break;
            }

            if (result != null)
            {
                result.SetAdConfig(data);
            }

            m_AdHandlerMap.Add(data.id, result);

            return result;
        }

        public IAdAdapter GetAdAdapter(string platformName)
        {
            if (m_AdInterfaceMap == null)
            {
                return null;
            }

            IAdAdapter adapter;
            m_AdAdapterMap.TryGetValue(platformName, out adapter);

            return adapter;
        }

        public AdInterface GetAdInterfaceByPlacementID(string placementID, int index)
        {
            AdPlacement placement = GetAdPlacement(placementID);

            if (placement == null)
            {
                Log.e("Not Find Placement:" + placementID);
                return null;
            }

            if (string.IsNullOrEmpty(placement.data.adInterface0))
            {
                Log.e("Invalid AdInterface For Ad:" + placementID);
                return null;
            }

            AdInterface ad = null;

            if (index == 0)
            {
                ad = GetAdInterface(placement.data.adInterface0);
            }
            else
            {
                ad = GetAdInterface(placement.data.adInterface1);
            }

            if (ad == null)
            {
                Log.w("Not Find AdInterface For Ad:" + placementID);
                return null;
            }

            return ad;
        }

        public void ShowBannerAd(string placementID, AdSize size, AdPosition position, int x = 0, int y = 0, bool force = false)
        {
            if (m_IsNoAds && !force)
            {
                return;
            }

#if UNITY_IOS || UNITY_IPHONE
            if (size.height == AdSize.SmartBanner.height)
            {
                size = AdSize.Banner;
            }
#endif
            AdInterface ad = GetAdInterfaceByPlacementID(placementID, 0);

            if (ad == null)
            {
                return;
            }

            ad.adSize = size;
            ad.adPosition = position;
            ad.adCustomGrid = new Vector2Int(x, y);

            ad.ShowAd("");
        }

        public void SetBannerPosition(string placementID, AdPosition position, int x, int y)
        {
            AdInterface ad = GetAdInterfaceByPlacementID(placementID, 0);

            if (ad == null)
            {
                return;
            }

            ad.adPosition = position;
            ad.adCustomGrid = new Vector2Int(x, y);

            ad.SyncAdPosition();
        }

        public void HideBannerAd(string placementID)
        {
            AdInterface ad = GetAdInterfaceByPlacementID(placementID, 0);

            if (ad == null)
            {
                return;
            }

            ad.HideAd();
        }

        public void HideAllAd()
        {
            foreach (var item in m_AdInterfaceMap.Values)
            {
                item.HideAd();
            }
        }

        public void ShowTestView()
        {
            UIMgr.S.OpenPanel(SDKUI.AdTestPanel);
        }

        ///////////////////////
        private void InitSupportedAdapter(SDKConfig config)
        {
            m_AdAdapterMap = new Dictionary<string, IAdAdapter>();

            if (!config.adsConfig.isEnable)
            {
                Log.w("Ads System Is Not Enable.");
                return;
            }

            //RegisterAdapter(config, config.adsConfig.webeyeAdsConfig);
            RegisterAdapter(config, config.adsConfig.admobConfig);
            RegisterAdapter(config, config.adsConfig.facebookConfig);
            RegisterAdapter(config, config.adsConfig.applovinAdsConfig);
            RegisterAdapter(config, config.adsConfig.ironSourceConfig);
            RegisterAdapter(config, config.adsConfig.polyAdsConfig);
            //RegisterAdapter(config, config.adsConfig.outAdsConfig);
            if (!RegisterAdapter(config, config.adsConfig.moPubAdsConfig))
            {
                RegisterAdapter(config, config.adsConfig.unityConfig);
            }
#if !UNITY_EDITOR
            
            //RegisterAdapter(config,config.adsConfig.moPubAdsConfig);
#endif

            InitAfPlatformName();
        }

        private bool RegisterAdapter(SDKConfig sdkConfig, SDKAdapterConfig adapterConfig)
        {
            if (!adapterConfig.isEnable)
            {
                return false;
            }

            Type type = Type.GetType(adapterConfig.adapterClassName);
            if (type == null)
            {
                Log.w("Not Support Ads:" + adapterConfig.adapterClassName);
                return false;
            }

            var method = type.GetMethod("GetInstance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            IAdAdapter adsAdapter = null;

            if (method != null)
            {
                adsAdapter = method.Invoke(null, null) as IAdAdapter;
            }
            else
            {
                adsAdapter = type.Assembly.CreateInstance(adapterConfig.adapterClassName) as IAdAdapter;
            }

            if (adsAdapter == null)
            {
                Log.e("AdAdapter Create Failed:" + adapterConfig.adapterClassName);
                return false;
            }

            if (adsAdapter.InitWithConfig(sdkConfig, adapterConfig))
            {
                m_AdAdapterMap.Add(adsAdapter.adPlatform, adsAdapter);

                Log.i("Success Register AdAdapter:" + adsAdapter.adPlatform);
            }
            else
            {
                Log.w("Failed Register AdAdapter:" + adsAdapter.adPlatform);
            }

            return true;
        }

        private void InitAfPlatformName()
        {
            m_AfPlatformName = new Dictionary<string, string>();
            foreach (var key in m_AdAdapterMap.Keys)
            {
                m_AfPlatformName.Add(key,SwitchSdkName(key));
            }
        }

        private string SwitchSdkName(string name)
        {
            var sdkName = name.ToLower();
            switch (sdkName)
            {
                case "facebook":
                    sdkName = "fb";
                    break;
                case "webeye":
                    sdkName = "wemob";
                    break;
            }

            return sdkName;
        }
        public string GetAfPlatformName(string key)
        {
            string name = "";
            m_AfPlatformName.TryGetValue(key, out name);
            return name;
        }
    }
}
