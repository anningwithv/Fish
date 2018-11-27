using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class AdTestPanel : AbstractPanel
    {
        [SerializeField]
        private Button m_CloseButton;
        [SerializeField]
        private Button m_OpenNativeBannerTestButton;
        [SerializeField]
        private Toggle[] m_AdapterToggles;

        [SerializeField]
        private InputField m_BannerInputField;
        [SerializeField]
        private Button m_ShowBannerButton;
        [SerializeField]
        private Button m_HideBannerButton;
        [SerializeField]
        private Text m_BannerStateLabel;

        [SerializeField]
        private InputField m_InterstitialAdInputField;
        [SerializeField]
        private Button m_ShowInterstitialAdButton;
        [SerializeField]
        private Button m_LoadInterstitialAdButton;
        [SerializeField]
        private Text m_InterstitialStateLabel;

        [SerializeField]
        private InputField m_RewardVideoAdInputField;
        [SerializeField]
        private Button m_ShowRewardVideoAdButton;
        [SerializeField]
        private Button m_LoadRewardVideoAdButton;
        [SerializeField]
        private Text m_RewardStateLabel;

        [SerializeField]
        private Text m_BannerDataLabel;
        [SerializeField]
        private Text m_InterstitialDataLabel;
        [SerializeField]
        private Text m_RewardVideoDataLabel;

        private int m_SelectAdapterIndex = -1;

        private IAdAdapter m_AdsAdapter;
        private List<IAdAdapter> m_AllAdsAdapter;

        protected override void OnUIInit()
        {
            m_CloseButton.onClick.AddListener(CloseSelfPanel);
            m_ShowBannerButton.onClick.AddListener(OnClickShowBannerButton);
            m_HideBannerButton.onClick.AddListener(OnClickHideBannerButton);
            m_OpenNativeBannerTestButton.onClick.AddListener(OnClickOpenNativeBannerTestButton);
            m_ShowInterstitialAdButton.onClick.AddListener(OnClickShowInterstitialAdButton);
            m_LoadInterstitialAdButton.onClick.AddListener(OnClickLoadInterstitialAdButton);

            m_ShowRewardVideoAdButton.onClick.AddListener(OnClickShowRewardVideoButton);
            m_LoadRewardVideoAdButton.onClick.AddListener(OnClickLoadRewardVideoAdButton);



            m_BannerInputField.text = "MainBanner";
            m_InterstitialAdInputField.text = "MainInternitial";
            m_RewardVideoAdInputField.text = "MainRewardVideo";

            for (int i = 0; i < m_AdapterToggles.Length; ++i)
            {
                if (i >= m_AllAdsAdapter.Count)
                {
                    m_AdapterToggles[i].gameObject.SetActive(false);
                    continue;
                }

                Text label = UIFinder.Find<Text>(m_AdapterToggles[i].transform, "Label");
                label.text = m_AllAdsAdapter[i].GetType().Name;

                int index = i;
                m_AdapterToggles[i].onValueChanged.AddListener((result) =>
                {
                    if (result)
                    {
                        SwitchAdapterAsIndex(index);
                    }
                });
            }

            SwitchAdapterAsIndex(0);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            UpdateDataUI();
        }

        private void UpdateDataUI()
        {
            //m_BannerDataLabel.text = string.Format("Banner: Load:{0}, Click:{1}, TryLoad:{2}", AdAnalysisMgr.S.bannerLoadCount, AdAnalysisMgr.S.bannerClickCount, AdAnalysisMgr.S.bannerTryLoadCount);
            //m_InterstitialDataLabel.text = string.Format("Inter: Load:{0}, Click:{1}", AdAnalysisMgr.S.interstitialLoadCount, AdAnalysisMgr.S.interstitialClickCount);
            //m_RewardVideoDataLabel.text = string.Format("Reward: Load:{0}, Click:{1}", AdAnalysisMgr.S.rewardVideoLoadCount, AdAnalysisMgr.S.rewardVideoSuccessCount);
        }

        private void Update()
        {

        }

        private string AdState2Name(AdState state)
        {
            switch (state)
            {
                case AdState.Loaded:
                    return "Loaded";
                case AdState.Loading:
                    return "Loading";
                case AdState.Showing:
                    return "Showing";
                case AdState.NONE:
                    return "NONE";
                default:
                    break;
            }

            return string.Empty;
        }

        private void OnClickOpenNativeBannerTestButton()
        {
            UIMgr.S.OpenPanel(SDKUI.NativeBannerTestPanel);
        }

        private void OnClickShowBannerButton()
        {
            if (m_AdsAdapter == null)
            {
                return;
            }

        }

        private void OnClickHideBannerButton()
        {
            if (m_AdsAdapter == null)
            {
                return;
            }

        }

        private void OnClickShowInterstitialAdButton()
        {
            if (m_AdsAdapter == null)
            {
                return;
            }

        }

        private void OnClickLoadInterstitialAdButton()
        {
            if (m_AdsAdapter == null)
            {
                return;
            }

        }

        private void OnClickShowRewardVideoButton()
        {
            if (m_AdsAdapter == null)
            {
                return;
            }
        }

        private void OnClickLoadRewardVideoAdButton()
        {
            if (m_AdsAdapter == null)
            {
                return;
            }
        }

        private void SwitchAdapterAsIndex(int index)
        {
            if (m_SelectAdapterIndex == index)
            {
                return;
            }

            CleanPreAdAdapter();

            m_SelectAdapterIndex = index;
            m_AdapterToggles[index].isOn = true;

            m_AdsAdapter = m_AllAdsAdapter[index];
        }

        private void CleanPreAdAdapter()
        {
            OnClickHideBannerButton();
        }
    }
}
