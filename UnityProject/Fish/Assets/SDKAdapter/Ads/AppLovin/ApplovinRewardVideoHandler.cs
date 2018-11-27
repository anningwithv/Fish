using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ApplovinRewardVideoHandler : AdFullScreenHandler, ApplovinRewardVideoInstance.IRewardVideoHandler
    {
        protected ApplovinRewardVideoInstance m_Instance;

        public override bool isAdReady
        {
            get
            {
                if (m_Instance == null)
                {
                    return false;
                }

                return m_Instance.isAdReady;
            }
        }

        protected override bool DoPreLoadAd()
        {
            if (m_Instance == null)
            {
                if (ApplovinRewardVideoInstance.S.Bind(this))
                {
                    m_Instance = ApplovinRewardVideoInstance.S;

                    return m_Instance.PreLoadAd();
                }
                return false;
            }
            else
            {
                return m_Instance.PreLoadAd();
            }
        }

        protected override bool DoShowAd()
        {
            if (m_Instance == null)
            {
                return false;
            }

            return m_Instance.ShowAd();
        }

        protected override void DoCleanAd()
        {
            if (m_Instance != null)
            {
                m_Instance.UnBind(this);
                m_Instance = null;
            }
        }

        public void OnRewardVideoLoaded()
        {
            HandleOnAdLoaded();
        }

        public void OnRewardVideoFailed2Load(string msg)
        {
            HandleOnAdFailedToLoad(msg);
        }

        public void OnRewardVideoOpen()
        {
            HandleOnAdOpened();
        }

        public void OnRewardVideoClose()
        {
            HandleOnAdClosed();
        }

        public void OnRewardLeftApplication()
        {
            HandleOnAdLeftApplication();
        }

        public void OnRewardVideoRewarded()
        {
            HandleOnAdRewarded();
        }

        public TDAdConfig GetAdConfig()
        {
            return m_Config;
        }
    }
}
