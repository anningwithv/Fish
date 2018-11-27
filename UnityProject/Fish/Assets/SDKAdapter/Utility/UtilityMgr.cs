using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qarth {

    public class UtilityMgr : TSingleton<UtilityMgr> {

        public void Vibrate()
        {
#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
            Handheld.Vibrate();
#else
            return;
#endif
        }
    }
}