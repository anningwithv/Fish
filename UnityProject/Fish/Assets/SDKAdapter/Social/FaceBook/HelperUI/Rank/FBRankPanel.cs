using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Qarth
{
    public class FBRankPanel : AbstractPanel
    {
        [SerializeField]
        private Button m_LoginButton;
        [SerializeField]
        private USimpleListView m_ListView;
        [SerializeField]
        private Button m_CloseButton;

        private List<FacebookUserInfo> m_DataArray;
        private bool m_IsReadyShow = false;
        private Action m_ReadyShowCallback;

        public bool isReady2Show
        {
            get { return m_IsReadyShow; }
            set
            {
                if (m_IsReadyShow != value)
                {
                    m_IsReadyShow = value;

                    if (m_ReadyShowCallback != null)
                    {
                        m_ReadyShowCallback();
                    }
                }
            }
        }

        public void RegisterReadyShowListener(Action callback)
        {
            m_ReadyShowCallback = callback;
        }

        protected override void OnUIInit()
        {
            if (m_LoginButton != null)
            {
                m_LoginButton.onClick.AddListener(OnClickLoginButton);
            }

            if (m_CloseButton != null)
            {
                m_CloseButton.onClick.AddListener(CloseSelfPanel);
            }
        }

        protected override void OnOpen()
        {
            RegisterEvent(SDKEventID.OnFBLoginEvent, OnFBLoginEvent);
            RegisterEvent(SDKEventID.OnFBRefreshRankScoreEvent, OnFBRefreshRankScoreEvent);
            m_ListView.SetCellRenderer(OnCellRenderer);
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (m_LoginButton != null)
            {
                if (!FacebookSocialAdapter.S.isPublishLoggedIn)
                {
                    m_LoginButton.gameObject.SetActive(true);
                }
                else
                {
                    m_LoginButton.gameObject.SetActive(false);
                }
            }

            if (FacebookSocialAdapter.S.isRankDataNeedRefresh)
            {
                FacebookSocialAdapter.S.RefreshRankScore();
            }

            UpdateRankListview();
        }

        private void UpdateRankListview()
        {
            var dataList = FacebookSocialAdapter.S.allGamerUserInfoList;
            if (dataList.Count == 0)
            {
                m_ListView.gameObject.SetActive(false);
                isReady2Show = false;
                return;
            }

            isReady2Show = true;
            m_DataArray = dataList;
            m_ListView.gameObject.SetActive(true);

            m_ListView.SetDataCount(m_DataArray.Count);
        }

        private void OnFBRefreshRankScoreEvent(int key, params object[] args)
        {
            UpdateRankListview();
        }

        private void OnFBLoginEvent(int key, params object[] args)
        {
            UpdateUI();
        }

        private void OnCellRenderer(Transform root, int index)
        {
            if (index >= m_DataArray.Count)
            {
                return;
            }

            FBRankCell item = root.GetComponent<FBRankCell>();
            item.SetData(m_DataArray[index], index);
        }

        private void OnClickLoginButton()
        {
            FBHelper.S.Login();
        }
    }
}

