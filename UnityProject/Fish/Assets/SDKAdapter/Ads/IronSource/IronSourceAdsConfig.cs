using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    [Serializable]
    public class IronSourceAdsConfig : SDKAdapterConfig
    {
        public override string adapterClassName
        {
            get
            {
                return "Qarth.IronSourceAdsAdapter";
            }
        }

        public string appKey;
    }
}
