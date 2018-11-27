using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class CrossExtensionView : MonoBehaviour
    {
        [SerializeField]
        private NetImageView m_IconImage;
        [SerializeField]
        private NetImageView m_PreviewImage;
        [SerializeField]
        private Text m_DescLabel;
        [SerializeField]
        private Text m_NameLabel;
        [SerializeField]
        private Button m_DownloadButton;
        [SerializeField]
        private bool m_AutoShow = false;

        private TDAppConfigTable.ConfigHandler m_ConfigHandler;

        private void Awake()
        {
            if (m_DownloadButton != null)
            {
                m_DownloadButton.onClick.AddListener(OnClickDownloadButton);
            }

            if (m_AutoShow)
            {
                SetIdentifyer(TDAppConfigTable.GetRandomExtensionConfig());
            }
        }

        public void SetIdentifyer(string identifyer)
        {
            var data = TDAppConfigTable.GetConfigHandler(identifyer);
        }

        public void SetIdentifyer(TDAppConfigTable.ConfigHandler data)
        {
            if (data == null || data == m_ConfigHandler)
            {
                return;
            }

            m_ConfigHandler = data;

            UpdateUI();
        }

        protected void UpdateUI()
        {
            if (m_IconImage != null)
            {
                m_IconImage.imageUrl = m_ConfigHandler.icon;
            }

            if (m_PreviewImage != null)
            {
                m_PreviewImage.imageUrl = m_ConfigHandler.preview_image;
            }

            if (m_DescLabel != null)
            {
                m_DescLabel.text = m_ConfigHandler.desc;
            }

            if (m_NameLabel != null)
            {
                m_NameLabel.text = m_ConfigHandler.name;
            }
        }

        protected void OnClickDownloadButton()
        {
            RemoteConfigMgr.S.OpenAppDownloadUrl(m_ConfigHandler);
        }
    }
}
