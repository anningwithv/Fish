//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//namespace Qarth
//{
//    public class VungleAdsAdapter : AbstractAdsAdapter
//    {
//        protected VungleAdsConfig m_Config;

//        private string m_Appid;

//        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
//        {
//            m_Config = adapterConfig as VungleAdsConfig;

//#if UNITY_ANDROID
//            m_Appid = m_Config.appIDAndroid;

//#elif UNITY_IPHONE
//            m_Appid = m_Config.appIDIos;
//#else
//            m_Appid = "unexpected_platform";
//#endif


//            return true;
//        }

//        public override string adPlatform
//        {
//            get
//            {
//                return "vungle";
//            }
//        }

//        public override void InitWithData()
//        {
//            var datas = TDAdConfigTable.GetAdDataByPlatform("vungle");

//            if (datas.Count <= 0)
//            {
//                return;
//            }

//            string[] placements = new string[datas.Count];

//            Vungle.onInitializeEvent += OnInitialzeEvent;

//            Vungle.init(m_Appid, placements);
//        }

//        private void OnInitialzeEvent()
//        {
//            Log.i("[VungleAdsAdapter]: Vungle Init");
//        }

//        public override AdHandler CreateInterstitialHandler()
//        {
//            return new VungleInterstitialHandler();
//        }

//    }
//}