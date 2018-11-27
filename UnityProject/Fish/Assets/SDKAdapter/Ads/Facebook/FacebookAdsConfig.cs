using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    [Serializable]
    public class FacebookAdsConfig : SDKAdapterConfig
    {
        public override string adapterClassName
        {
            get
            {
                return "Qarth.FacebookAdsAdapter";
            }
        }
    }
}
