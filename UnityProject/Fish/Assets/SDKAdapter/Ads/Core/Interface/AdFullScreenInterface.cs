using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AdFullScreenInterface : AdInterface
    {
        private int m_ReloadTimer;

        public override bool ShowAd(string rewardID)
        {
            if (m_IsShowing)
            {
                return false;
            }

            m_RewardID = rewardID;
            m_HasReward = false;

            for (int i = 0; i < m_AdHandler.Count; ++i)
            {
                if (m_AdHandler[i].isAdReady)
                {
                    if (m_AdHandler[i].ShowAd())
                    {
                        m_IsShowing = true;
                        CheckAdState();
                        return true;
                    }
                }
            }

            return false;
        }

        protected void OnReloadTimer(int count)
        {
            DoPreLoadWork();
        }

        protected int GetAlreadyLoadCount()
        {
            int loadedCount = 0;
            for (int i = 0; i < m_AdHandler.Count; ++i)
            {
                switch (m_AdHandler[i].adState)
                {
                    case AdState.Loaded:
                        ++loadedCount;
                        break;
                    default:
                        break;
                }
            }

            return loadedCount;
        }

        protected void DoPreLoadWork()
        {
            CheckAdState();
            int totalLoadedCount = GetAlreadyLoadCount();
            if (totalLoadedCount >= MAX_LOADED_AD_COUNT)
            {
                return;
            }
            //1.每个Handler 失败后有30秒冷却时间
            //2.有2个handler已经准备好就不需要加载新广告
            //3.同时加载的个数
            int loadedCount = 0;
            int loadingCount = 0;

            for (int i = 0; i < m_AdHandler.Count; ++i)
            {
                if (!m_AdHandler[i].isAdPlatformEnable)
                {
                    continue;
                }

                switch (m_AdHandler[i].adState)
                {
                    case AdState.Loaded:
                        ++loadedCount;
                        if (loadedCount >= MAX_LOADING_AD_COUNT)
                        {
                            return;
                        }
                        break;
                    case AdState.Loading:
                        break;
                    case AdState.NONE:
                        if (m_AdHandler[i].PreLoadAd())
                        {
                            ++loadingCount;
                            if (loadingCount >= (MAX_LOADING_AD_COUNT - loadedCount))
                            {
                                return;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public override void PreLoadAd()
        {
            if (m_ReloadTimer <= 0)
            {
                m_ReloadTimer = Timer.S.Post2Really(OnReloadTimer, FULLSCREEN_AD_LOAD_OFFSET, -1);
                OnReloadTimer(0);
            }
        }

        public override void OnAdLoad()
        {
            base.OnAdLoad();

            CheckAdState();
        }
    }
}
