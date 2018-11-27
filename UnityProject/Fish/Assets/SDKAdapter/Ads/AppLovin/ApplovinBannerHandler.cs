using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ApplovinBannerHandler : AdBannerHandler, IBannerHandler
    {
        public Vector2 GetBannerSizeInPixel()
        {
            return Vector2.zero;
        }
    }
}
