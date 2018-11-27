using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public interface ISDKAdapter
    {
        int GetPriorityScore();
        bool InitWithConfig(SDKConfig config, SDKAdapterConfig adapterConfig);
    }
}
