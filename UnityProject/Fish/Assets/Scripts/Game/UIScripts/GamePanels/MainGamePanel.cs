using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Qarth;

namespace GameWish.Game
{
    public class MainGamePanel : AbstractPanel
    {
        [SerializeField]
        private Image m_EnergyProgressBar = null;

        protected override void OnPanelOpen(params object[] args)
        {

        }

        protected override void OnUIInit()
        {
            Init();
        }

        private void Init()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register((int)EventID.OnPlayerEnergyChanged, HandleEvent);
        }

        private void HandleEvent(int eventId, params object[] param)
        {
            if (eventId == (int)EventID.OnPlayerEnergyChanged)
            {
                float percent = (float)param[0];
                UpdateEnergyImage(percent);
            }
        }

        private void UpdateEnergyImage(float percent)
        {
            m_EnergyProgressBar.fillAmount = percent;
        }
    }
}
