using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AbstractSDKAdapter : ISDKAdapter
    {
        protected SDKAdapterConfig m_AdapterConfig;

        public int GetPriorityScore()
        {
            if (m_AdapterConfig == null)
            {
                return 0;
            }

            return m_AdapterConfig.priority;
        }

        public bool InitWithConfig(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            m_AdapterConfig = adapterConfig;
            return DoAdapterInit(config, adapterConfig);
        }

        protected virtual bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            return true;
        }
    }
}
