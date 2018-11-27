using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class NativeBannerTestPanel : AbstractPanel
    {
        [SerializeField]
        private Button m_LoadButton;
        [SerializeField]
        private Button m_CloseButton;
        [SerializeField]
        private Button m_HideButton;
        [SerializeField]
        private InputField m_PositionXLabel;
        [SerializeField]
        private InputField m_PositionYLabel;
        [SerializeField]
        private Slider m_PositionXSlider;
        [SerializeField]
        private Slider m_PositionYSlider;
        [SerializeField]
        private Text m_ScreenState;

        [SerializeField]
        private AdBannerView m_NativeBannerView;

        private const string Banner_Name = "LeardBanner";

        protected override void OnUIInit()
        {
            m_LoadButton.onClick.AddListener(OnClickLoadButton);
            m_CloseButton.onClick.AddListener(CloseSelfPanel);
            m_HideButton.onClick.AddListener(OnClickHideButton);
            m_PositionXSlider.onValueChanged.AddListener(OnXPosition);
            m_PositionYSlider.onValueChanged.AddListener(OnYPosition);

            m_ScreenState.text = string.Format("Dpi:{0},ResolutionW:{1}-{2}, W-H:{3}-{4}", 
                Screen.dpi, Screen.currentResolution.width, Screen.currentResolution.height, Screen.width, Screen.height);
        }

        private void DeleteZPosition(Transform target)
        {
            Vector3 localPos = target.localPosition;
            localPos.z = 0;
            target.localPosition = localPos;
        }

        private void OnXPosition(float x)
        {
            m_NativeBannerView.transform.localPosition = new Vector3(StringToInt(m_PositionXLabel.text) * m_PositionXSlider.value,
                StringToInt(m_PositionYLabel.text) * m_PositionYSlider.value, 0);
        }

        private void OnYPosition(float y)
        {
            m_NativeBannerView.transform.localPosition = new Vector3(StringToInt(m_PositionXLabel.text) * m_PositionXSlider.value,
                StringToInt(m_PositionYLabel.text) * m_PositionYSlider.value, 0);
        }

        private int StringToInt(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return 0;
            }

            return int.Parse(msg);
        }

        private void OnClickHideButton()
        {
            m_NativeBannerView.HideAd();
        }

        private void OnClickLoadButton()
        {
            m_NativeBannerView.ShowAd(Banner_Name);
        }
    }
}
