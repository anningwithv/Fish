using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class BuglyMgr : TSingleton<BuglyMgr>
    {
        public void Init()
        {
            if (!SDKConfig.S.buglyConfig.isEnable)
            {
                return;
            }

            BuglyAgent.ConfigDebugMode(SDKConfig.S.buglyConfig.isDebugMode);
            BuglyAgent.InitWithAppId(GetBuglyID(SDKConfig.S.buglyConfig));
            Log.i("Init[BuglyMgr]");
        }

        private string GetBuglyID(BuglyConfig config)
        {
#if UNITY_ANDROID
            return config.buglyID_Android;
#elif UNITY_IPHONE
            return config.buglyID_iOS;
#else
            return "";
#endif
        }
    }
}
