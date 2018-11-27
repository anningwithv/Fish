using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qarth
{
    [Serializable]
	public class OutAdsConfig : SDKAdapterConfig
    {
        public override string adapterClassName
        {
            get { return "Qarth.OutAdsAdapter"; }
        }
    }
}

