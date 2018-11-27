using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class FBPlayerInfoPanel : AbstractPanel
    {
        [SerializeField]
        private FBPhotoView m_HighQualityIcon;
        [SerializeField]
        private FBPhotoView m_LowQualityIcon;
        [SerializeField]
        private Button m_CloseButton;
        [SerializeField]
        private Text m_NameLabel;
        [SerializeField]
        private Text m_ScoreLabel;
        [SerializeField]
        private Text m_RankLabel;

        protected override void OnUIInit()
        {
            m_CloseButton.onClick.AddListener(CloseSelfPanel);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);
            RegisterEvent(SDKEventID.OnFBRefreshPlayerInfoEvent, OnFBRefreshPlayerInfoHandler);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            UpdateUI();
        }

        protected void UpdateUI()
        {
            var selfData = FacebookSocialAdapter.S.selfUserInfo;
            if (selfData == null)
            {
                ShowLoadingUI();
                FacebookSocialAdapter.S.RefreshSelfPlayerInfo();
                return;
            }

            m_NameLabel.text = selfData.userName;
            m_ScoreLabel.text = selfData.gameSocre.ToString();
            m_RankLabel.text = selfData.rank.ToString();
            m_HighQualityIcon.userBase = selfData;
            m_LowQualityIcon.userBase = selfData;
        }

        protected void ShowLoadingUI()
        {

        }

        protected void OnFBRefreshPlayerInfoHandler(int key, params object[] args)
        {
            UpdateUI();
        }
    }
}
