﻿//  Desc:        Framework For Game Develop with Unity3d
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
    public enum EngineEventID
    {
        EngineEventIDMin = 1000000,
        OnPanelUpdate,

        OnApplicationFocusChange,
        OnApplicationPauseChange,
        OnAfterApplicationPauseChange,
        OnAfterApplicationFocusChange,

        OnApplicationQuit,

        BackKeyDown,
        OnShare2Social,
        OnAchievementComplate,
        OnAchievementFinish,

        OnDateUpdate,//日期更新
        OnSignStateChange,

        OnShareCaptureBegin,
        OnShareCaptureEnd,
        OnLanguageChange,
        OnLanguageTableSwitchFinish,
        OnNeedShowBanner,
        OnNeedHideBanner,
        ///////////////
    }
}
