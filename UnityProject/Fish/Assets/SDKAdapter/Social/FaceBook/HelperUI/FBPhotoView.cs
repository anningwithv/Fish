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
using UnityEngine.UI;

namespace Qarth
{
    public class FBPhotoView : NetImageView
    {
        [SerializeField]
        private int     m_PhotoWidth = -1;
        [SerializeField]
        private int     m_PhotoHeight = -1;

        private UserInfoBase m_UserBase;

        public UserInfoBase userBase
        {
            get { return m_UserBase; }
            set
            {
                m_UserBase = value;

                if (m_UserBase == null)
                {
                    imageUrl = null;
                }
                else
                {
                    prefixKey = FBPhotoRes.PREFIX_KEY;
                    if (m_PhotoWidth < 0 || m_PhotoHeight < 0)
                    {
                        imageUrl = m_UserBase.userID;
                    }
                    else
                    {
                        imageUrl = FBPhotoRes.CreatePhotoURL(m_UserBase.userID, m_PhotoWidth, m_PhotoHeight);
                    }
                }
            }
        }
    }
}