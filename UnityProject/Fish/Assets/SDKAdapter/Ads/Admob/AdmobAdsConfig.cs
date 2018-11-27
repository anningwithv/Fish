using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    [Serializable]
    public class AdmobAdsConfig : SDKAdapterConfig
    {
        public override string adapterClassName
        {
            get
            {
                return "Qarth.AdmobAdsAdapter";
            }
        }

        public string appIDAndroid;
        public string appIDIos;
    }
}
