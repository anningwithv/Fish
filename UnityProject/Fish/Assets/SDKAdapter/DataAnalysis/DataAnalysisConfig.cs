using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    [Serializable]
    public class DataAnalysisConfig
    {
        public bool isEnable = true;
        public AppsflyerConfig appsflyerConfig;
        public DataeyeConfig dataeyeConfig;
        public GameAnalysisConfig gameAnalysisConfig;
        public UmengConfig umengConfig;
        public FacebookDataConfig facebookConfig;
        public FirebaseDataConfig firebaseConfig;
    }

    [Serializable]
    public class DataeyeConfig : SDKAdapterConfig
    {
        public string appID;

        public override string adapterClassName
        {
            get
            {
                return "Qarth.DataeyeAdapter";
            }
        }
    }

    [Serializable]
    public class GameAnalysisConfig : SDKAdapterConfig
    {
        public override string adapterClassName
        {
            get
            {
                return "Qarth.GameAnalysisAdapter";
            }
        }
    }


    [Serializable]
    public class AppsflyerConfig : SDKAdapterConfig
    {
        public string appKey;

        public override string adapterClassName
        {
            get
            {
                return "Qarth.AppsflyerDataAdapter";
            }
        }
    }

    [Serializable]
    public class UmengConfig: SDKAdapterConfig
    {
        public string iosAppKey;
        public string androidAppKey;

        public string appChannelId;

        public override string adapterClassName
        {
            get
            {
                return "Qarth.UmengAdapter";
            }
        }
    }

    [Serializable]
    public class FacebookDataConfig : SDKAdapterConfig
    {
        public override string adapterClassName
        {
            get
            {
                return "Qarth.FacebookDataAdapter";
            }
        }
    }

    [Serializable]
    public class FirebaseDataConfig : SDKAdapterConfig
    {
        public override string adapterClassName
        {
            get
            {
                return "Qarth.FirebaseDataAdapter";
            }
        }
    }
}
