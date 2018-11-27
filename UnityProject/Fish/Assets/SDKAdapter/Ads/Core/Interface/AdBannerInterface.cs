using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    //1.30秒检测一次广告单元是否正常,不正常就切换。
    //2.如果正常检测当前平台的展示时间，如果在2分钟以内就刷新，如果超过2分钟切换
    public class AdBannerInterface : AdInterface
    {
        private const float MAX_DISPLAY_DURATION = 120;
        private const float REFRESH_DURATION = 30;

        private AdHandler m_CurrentHandler;
        private int m_RefreshTickTimer;
        private int m_CheckTickTimer;
        private float m_AlreadyShowDuration;
        private float m_RefreshTime;
        private bool m_IsBannerShow = false;

        protected override bool isLogEnable
        {
            get
            {
                return false;
            }
        }

        public override bool ShowAd(string rewardID)
        {
            if (m_IsBannerShow)
            {
                return false;
            }

            SelectAd2Show();

            m_IsBannerShow = true;

            return true;
        }

        public override void HideAd()
        {
            if (!m_IsBannerShow)
            {
                return;
            }

            m_IsBannerShow = false;
            if (m_CurrentHandler != null)
            {
                m_CurrentHandler.HideAd();
                m_CurrentHandler = null;
                m_AlreadyShowDuration = Time.realtimeSinceStartup - m_RefreshTime;
                m_RefreshTime = -1;
            }

            if (m_RefreshTickTimer > 0)
            {
                Timer.S.Cancel(m_RefreshTickTimer);
                m_RefreshTickTimer = -1;
            }

            if (m_CheckTickTimer > 0)
            {
                Timer.S.Cancel(m_CheckTickTimer);
                m_CheckTickTimer = -1;
            }
        }

        public override void SyncAdPosition()
        {
            if (m_CurrentHandler == null)
            {
                return;
            }

            m_CurrentHandler.SyncAdPosition();
        }

        protected void OnRefreshTimeTick(int tick)
        {
            m_RefreshTickTimer = -1;
            SelectAd2Show();
        }

        protected void StartCheckTimeTick()
        {
            if (m_RefreshTickTimer > 0)
            {
                Timer.S.Cancel(m_RefreshTickTimer);
            }

            m_RefreshTickTimer = Timer.S.Post2Really(OnRefreshTimeTick, 31);

            if (m_CheckTickTimer > 0)
            {
                Timer.S.Cancel(m_CheckTickTimer);
            }

            m_CheckTickTimer = Timer.S.Post2Really(OnCheckCurrentAdStateTick, 5);
        }

        protected void SelectAd2Show()
        {
            AdHandler next = null;

            for (int i = 0; i < m_AdHandler.Count; ++i)
            {
                if (!m_AdHandler[i].isAdPlatformEnable)
                {
                    continue;
                }

                if (m_AdHandler[i].adState != AdState.Failed)
                {
                    next = m_AdHandler[i];
                    break;
                }
            }

            if (next != m_CurrentHandler)
            {
                if(m_CurrentHandler != null)
                {
                    m_CurrentHandler.HideAd();
                }

                m_CurrentHandler = next;

                m_AlreadyShowDuration = 0;
                if (m_CurrentHandler != null)
                {
                    m_CurrentHandler.PreLoadAd();
                    m_CurrentHandler.SyncAdPosition();
                    m_RefreshTime = Time.realtimeSinceStartup;
                    m_CurrentHandler.ShowAd();
                    m_CurrentHandler.RefreshAd();

                    StartCheckTimeTick();
                }
            }
            else
            {
                if (m_CurrentHandler == null)
                {
                    return;
                }

                float showDuration = m_AlreadyShowDuration;
                if (m_RefreshTime > 0)
                {
                    showDuration += Time.realtimeSinceStartup - m_RefreshTime;
                }

                if (!m_IsBannerShow)
                {
                    m_CurrentHandler.ShowAd();
                }

                if (showDuration >= REFRESH_DURATION)
                {
                    m_CurrentHandler.RefreshAd();
                    m_RefreshTime = Time.realtimeSinceStartup;
                    StartCheckTimeTick();
                }
            }
        }

        protected void OnCheckCurrentAdStateTick(int count)
        {
            m_CheckTickTimer = -1;

            if (m_CurrentHandler == null)
            {
                return;
            }

            if (m_CurrentHandler.adState == AdState.Loading)
            {
                m_CurrentHandler.HideAd();
                m_CurrentHandler.SetAdStateFailed();
                SelectAd2Show();
            }
        }

        public override void OnAdLoadFailed()
        {
            base.OnAdLoadFailed();
            if (m_IsBannerShow)
            {
                SelectAd2Show();
            }
        }
    }
}
