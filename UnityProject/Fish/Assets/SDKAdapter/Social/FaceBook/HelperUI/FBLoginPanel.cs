using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class FBLoginPanel : AbstractPanel
    {
        [SerializeField]
        private Button m_RetryButton;
        [SerializeField]
        private Button m_OKButton;
        [SerializeField]
        private Button m_CloseButton;
        [SerializeField]
        private Text m_LoginState;

        protected override void OnUIInit()
        {
            m_RetryButton.onClick.AddListener(Login2FB);
            m_CloseButton.onClick.AddListener(CloseSelfPanel);
            m_OKButton.onClick.AddListener(CloseSelfPanel);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);
            RegisterEvent(SDKEventID.OnFBLoginEvent, OnFBLoginEvent);
            Login2FB();
        }

        private void Login2FB()
        {
            m_RetryButton.gameObject.SetActive(false);
            m_CloseButton.gameObject.SetActive(false);
            //m_OKButton.gameObject.SetActive(false);

            if (FacebookSocialAdapter.S.isPublishLoggedIn)
            {
                m_OKButton.gameObject.SetActive(true);
            }
            else
            {
                FacebookSocialAdapter.S.PromptForPublishLogin();
            }
        }

        private void OnFBLoginEvent(int key, params object[] args)
        {
            bool result = (bool)args[0];

            if (result)
            {
                m_OKButton.gameObject.SetActive(true);
            }
            else
            {
                m_CloseButton.gameObject.SetActive(true);
                m_RetryButton.gameObject.SetActive(true);
            }
        }

        public override BackKeyCodeResult OnBackKeyDown()
        {
            CloseSelfPanel();
            return BackKeyCodeResult.PROCESS_AND_BLOCK;
        }
    }
}
