using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    
    [Serializable]
    public class SDKAdapterConfig
    {
        public bool isEnable = true;
        public bool isDebugMode = false;
        public int priority = 1; // 越低优先级越高
        public virtual string adapterClassName
        {
            get { return ""; }
        }
    }
    
}
