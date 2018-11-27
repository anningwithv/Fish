using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Qarth;

namespace GameWish.Game
{
    public class LogoPanel : AbstractPanel
    {
        [SerializeField]
        private float m_ShowTime = 1;

        private Action m_Listener;
        private int m_TimeID;

        [SerializeField] private Image m_LoadingIcon;

        protected override void OnPanelOpen(params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                m_Listener = args[0] as Action;
            }

            if (m_TimeID > 0)
            {
                Timer.S.Cancel(m_TimeID);
            }

            m_TimeID = Timer.S.Post2Really(OnTimeReach, m_ShowTime);
        }

        private void OnTimeReach(int count)
        {
            m_TimeID = -1;

            if (m_Listener != null)
            {
                m_Listener();
                m_Listener = null;
            }
        }

        void Update()
        {
            m_LoadingIcon.transform.Rotate(Vector3.back,45*Time.deltaTime);
        }
    }
}
