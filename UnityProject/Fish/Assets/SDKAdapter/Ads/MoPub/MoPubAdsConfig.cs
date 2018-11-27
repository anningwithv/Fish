using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    [Serializable]
    public class MoPubAdsConfig : SDKAdapterConfig
    {
        public override string adapterClassName
        {
            get
            {
                return "Qarth.MoPubAdsAdapter";
            }
        }
        public float bannerRefreshDuration = 30;
        public string anyAdUnit;
        public string appIDAndroid;
        public string appIDIos;
    }
}
