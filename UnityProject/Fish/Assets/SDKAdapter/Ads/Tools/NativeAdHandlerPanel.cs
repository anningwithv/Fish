using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class NativeAdHandlerPanel : MonoBehaviour
    {
        public static Transform adRoot;

        public static void Open()
        {
            if (adRoot != null)
            {
                return;
            }

            GameObject root = new GameObject("NativeAdHandler");
            var rectTr = root.AddComponent<RectTransform>();
            adRoot = root.transform;
            adRoot.SetParent(UIMgr.S.uiRoot.panelRoot.parent, true);
            adRoot.localPosition = Vector3.zero;
            adRoot.localScale = Vector3.one;
        }
    }
}
