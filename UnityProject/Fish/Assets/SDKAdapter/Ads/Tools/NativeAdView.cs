using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;
using DG.Tweening;

namespace Qarth
{
    public class NativeAdView : MonoBehaviour
    {
        [SerializeField]
        protected String m_AdName;
        [SerializeField]
        protected bool m_AutoShowOnEnable = true;

        [SerializeField]
        protected Text m_TitleLabel;
        [SerializeField]
        protected Text m_SocialContextLabel;
        [SerializeField]
        protected Text m_CallToActionLabel;
        [SerializeField]
        protected Image m_CoverImage;
        [SerializeField]
        protected Image m_IconImage;
        [SerializeField]
        protected GameObject m_Root;
        [SerializeField]
        protected Button m_ActionButton;
        [SerializeField]
        private GameObject m_LoadingAnim;
        [SerializeField]
        private float m_RefreshTime = 30;

        protected int m_Timer = -1;

        public string adName
        {
            get { return m_AdName; }
        }

        protected INativeAdHandler m_AdHandler;

        private void Awake()
        {
            if (m_LoadingAnim != null)
            {
                m_LoadingAnim.transform.DORotate(new Vector3(0, 0, -360), 5, RotateMode.LocalAxisAdd)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
            }
        }

        private void OnEnable()
        {
            if (m_AutoShowOnEnable)
            {
                ShowAd(m_AdName, true);
                m_Timer = Timer.S.Post2Really(OnTimeTick, m_RefreshTime, -1);
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_Timer > 0)
            {
                if (MonoSingleton.isApplicationQuit)
                {
                    return;
                }
                Timer.S.Cancel(m_Timer);
                m_Timer = -1;
            }

            UnBindCurrentHandler();
        }

        private void OnDisable()
        {
            HideAdUI();
            if (m_Timer > 0)
            {
                if (MonoSingleton.isApplicationQuit)
                {
                    return;
                }
                Timer.S.Cancel(m_Timer);
                m_Timer = -1;
            }
        }

        private void OnTimeTick(int count)
        {
            ShowAd(m_AdName, true);
        }

        public void ShowAd(string name, bool load)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            UnBindCurrentHandler();

            m_AdName = name;

            m_AdHandler = AdsMgr.S.GetNativeAdHandler(m_AdName);
            BindCurrentHandler();

            if (m_AdHandler == null)
            {
                Log.w("Not Find AdHandler:" + m_AdName);
                return;
            }

            UpdateUIAsAdState();

            if (load)
            {
                m_AdHandler.LoadAd();
            }

            if (m_ActionButton != null)
            {
                m_AdHandler.RegisterGameObjectForImpression(m_Root, new Button[] { m_ActionButton });
            }
        }

        protected void UnBindCurrentHandler()
        {
            if (m_AdHandler == null)
            {
                return;
            }

            if (m_ActionButton != null)
            {
                m_AdHandler.UnRegisterGameObjectForImpression();
            }
            m_AdHandler.UnBind(OnAdTextLoadFinish, OnAdIconLoadFinish, OnAdCoverImageLoadFinish);

            m_AdHandler = null;
        }

        protected void BindCurrentHandler()
        {
            if (m_AdHandler == null)
            {
                return;
            }

            m_AdHandler.Bind(OnAdTextLoadFinish, OnAdIconLoadFinish, OnAdCoverImageLoadFinish);
        }

        protected void OnAdTextLoadFinish(INativeAdHandler handler)
        {
            UpdateText();
            if (m_ActionButton != null)
            {
                m_AdHandler.RegisterGameObjectForImpression(m_Root, new Button[] { m_ActionButton });
            }
        }

        protected void OnAdIconLoadFinish(INativeAdHandler handler)
        {
            UpdateIcon();
        }

        protected void OnAdCoverImageLoadFinish(INativeAdHandler handler)
        {
            UpdateCoverImage();
        }

        protected void UpdateUIAsAdState()
        {
            if (m_AdHandler == null)
            {
                HideAdUI();
                SetLoadingAnimState(true);
            }
            else
            {
                AdState state = m_AdHandler.adState;
                switch (state)
                {
                    case AdState.Loaded:
                    case AdState.Showing:
                        UpdateText();
                        UpdateIcon();
                        UpdateCoverImage();
                        SetLoadingAnimState(false);
                        break;
                    case AdState.Loading:
                    case AdState.NONE:
                        HideAdUI();
                        SetLoadingAnimState(true);
                        break;
                    default:
                        break;
                }
            }
        }

        protected void UpdateText()
        {
            if (m_TitleLabel != null)
            {
                m_TitleLabel.text = m_AdHandler.adTitle;
                m_TitleLabel.enabled = true;
            }

            if (m_SocialContextLabel != null)
            {
                m_SocialContextLabel.text = m_AdHandler.socialContext;
                m_SocialContextLabel.enabled = true;
            }

            if (m_CallToActionLabel != null)
            {
                m_CallToActionLabel.text = m_AdHandler.callToAction;
                m_CallToActionLabel.enabled = true;
                m_ActionButton.gameObject.SetActive(true);
            }
        }

        protected void UpdateIcon()
        {
            if (m_IconImage == null)
            {
                return;
            }

            m_IconImage.sprite = m_AdHandler.iconSprite;
            m_IconImage.enabled = m_IconImage.sprite != null;
        }

        protected void UpdateCoverImage()
        {
            if (m_CoverImage == null)
            {
                return;
            }

            m_CoverImage.sprite = m_AdHandler.coverImage;
            m_CoverImage.enabled = m_CoverImage.sprite != null;
        }

        private void SetLoadingAnimState(bool state)
        {
            if (m_LoadingAnim == null)
            {
                return;
            }

            m_LoadingAnim.SetActive(state);
        }

        public void HideAdUI()
        {
            if (m_TitleLabel != null)
            {
                m_TitleLabel.enabled = false;
            }

            if (m_SocialContextLabel != null)
            {
                m_SocialContextLabel.enabled = false;
            }

            if (m_CallToActionLabel != null)
            {
                m_CallToActionLabel.enabled = false;
            }

            if (m_CoverImage != null)
            {
                m_CoverImage.enabled = false;
            }

            if (m_IconImage != null)
            {
                m_IconImage.enabled = false;
            }

            if (m_ActionButton != null)
            {
                m_ActionButton.gameObject.SetActive(false);
            }
        }
    }
}
