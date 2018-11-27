using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class OfficialVersionAdPanel : AbstractPanel
    {
        [SerializeField]
        private Button m_DownloadButton;
        [SerializeField]
        private Button m_CloseButton;

        protected override void OnUIInit()
        {
            m_DownloadButton.onClick.AddListener(OnClickDownloadButton);
            m_CloseButton.onClick.AddListener(CloseSelfPanel);
        }

        protected void OnClickDownloadButton()
        {
            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.DOWNLOAD_OFFICIAL_VERSION, Application.identifier);
            SocialMgr.S.OpenMarketDownloadPage(TDRemoteConfigTable.GetOfficialBundleID());
        }

        protected override void OnOpen()
        {
            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.OPEN_OFFICIAL_AD_PANEL, Application.identifier);
            OpenDependPanel(EngineUI.MaskPanel, -1);
        }
    }
}
