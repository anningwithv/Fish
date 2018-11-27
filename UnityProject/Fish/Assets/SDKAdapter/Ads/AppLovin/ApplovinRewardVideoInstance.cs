using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class ApplovinRewardVideoInstance : TSingleton<ApplovinRewardVideoInstance>
    {
        public interface IRewardVideoHandler
        {
            void OnRewardVideoLoaded();
            void OnRewardVideoFailed2Load(string msg);
            void OnRewardVideoOpen();
            void OnRewardVideoClose();
            void OnRewardLeftApplication();
            void OnRewardVideoRewarded();

            TDAdConfig GetAdConfig();
        }

        private IRewardVideoHandler m_RewardVideoHandler;
        protected bool m_HasInit = false;

        public bool isAdReady
        {
            get
            {
                if (m_RewardVideoHandler == null)
                {
                    return false;
                }

                return AppLovin.IsIncentInterstitialReady(m_RewardVideoHandler.GetAdConfig().unitID);
            }
        }

        public bool Bind(IRewardVideoHandler handler)
        {
            if (m_RewardVideoHandler != null)
            {
                return false;
            }

            m_RewardVideoHandler = handler;

            return true;
        }

        public void UnBind(IRewardVideoHandler handler)
        {
            if (m_RewardVideoHandler == handler)
            {
                m_RewardVideoHandler = null;
            }
        }

        public bool ShowAd()
        {
            if (m_RewardVideoHandler == null)
            {
                return false;
            }

            AppLovin.ShowRewardedInterstitialForZoneId(m_RewardVideoHandler.GetAdConfig().unitID);
            return true;
        }

        public bool PreLoadAd()
        {
            if (m_RewardVideoHandler == null)
            {
                return false;
            }

            if (!m_HasInit)
            {
                m_HasInit = true;
                ApplovinEventCenter.S.on_RewardLoaded += HandleOnAdLoaded;
                ApplovinEventCenter.S.on_RewardClose += HandleOnAdClosed;
                ApplovinEventCenter.S.on_RewardLoadFailed += HandleOnAdFailedToLoad;
                ApplovinEventCenter.S.on_RewardReward += HandleOnAdRewarded;
            }

            if (isAdReady)
            {
                return false;
            }

            AppLovin.LoadRewardedInterstitial(m_RewardVideoHandler.GetAdConfig().unitID);

            return true;
        }

        protected void HandleOnAdLoaded()
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoLoaded();
        }

        protected void HandleOnAdFailedToLoad()
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoFailed2Load("");
        }

        //统计点击
        protected void HandleOnAdOpened()
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoOpen();
        }

        //恢复操作
        protected void HandleOnAdClosed()
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoClose();
        }

        protected void HandleOnAdLeftApplication()
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardLeftApplication();
        }

        protected void HandleOnAdStarted()
        {

        }

        protected void HandleOnAdRewarded()
        {
            if (m_RewardVideoHandler == null)
            {
                return;
            }

            m_RewardVideoHandler.OnRewardVideoRewarded();
        }
    }
}
