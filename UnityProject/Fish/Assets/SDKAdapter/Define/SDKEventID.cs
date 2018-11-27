//  Desc:        Framework For Game Develop with Unity3d
//  Copyright:   Copyright (C) 2017 SnowCold. All rights reserved.
//  WebSite:     https://github.com/SnowCold/Qarth
//  Blog:        http://blog.csdn.net/snowcoldgame
//  Author:      SnowCold
//  E-mail:      snowcold.ouyang@gmail.com
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public enum SDKEventID
    {
        SDKEventIDMin = 2000000,

        OnNoAdModeChange,

        //FB
        OnFBRefreshPlayerInfoEvent, //ID, RESULT
        OnFBLoginEvent, //bool
        OnFBLogoutEvent,
        OnFBRefreshSelfScoreEvent,
        OnFBRefreshRankScoreEvent,
        OnFBRefreshFriendEvent,
        OnFBPostScoreEvent,
        OnShareImageFinish,
        //
        OnPurchaseInitSuccess,
        OnPurchaseInitFailed,
        OnPurchaseSuccess,
        OnPurchaseFailed,
        OnFBRequestFinsh
    }
}