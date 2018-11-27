using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Qarth
{
    [Serializable]
    public class UnityAdsConfig : SDKAdapterConfig
    {
        public UnityAdsConfig() : base()
        {
            isEnable = false;
        }

        public string UnityGameID_Android;
        public string UnityGameID_iOS;

        public override string adapterClassName
        {
            get
            {
                return "Qarth.UnityAdsAdapter";
            }
        }
    }
}