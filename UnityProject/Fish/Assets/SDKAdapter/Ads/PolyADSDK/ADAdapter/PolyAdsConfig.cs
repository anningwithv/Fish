using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    [Serializable]
    public class PolyAdsConfig : SDKAdapterConfig
    {
        public bool isLibEnable = false;

        public override string adapterClassName
        {
            get
            {
                return "Qarth.PolyAdsAdapter";
            }
        }
    }
}
