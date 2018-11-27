using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qarth
{
    [Serializable]
    public class SocialConfig
    {
        public bool isEnable = true;
        public FBConfig fbConfig;
    }

    [Serializable]
    public class FBConfig
    {
        public string appLinkAndroid;
        public string appLinkiOS;
        public string previewImageAndroid;
        public string previewImageiOS;
        public string shareLink;
    }
}