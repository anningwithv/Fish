using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public partial class TDAdConfig
    {
        private DateTime m_BirthdayTime;
        private bool m_IsBirthDayConfiged = false;

        public void Reset()
        {
            InitBirthdayDateTime();
            m_AdPlatform = m_AdPlatform.ToLower();
        }

        public bool isBirthDayConfiged
        {
            get { return m_IsBirthDayConfiged; }
        }

        private void InitBirthdayDateTime()
        {
            if (string.IsNullOrEmpty(m_Birthday))
            {
                return;
            }

            int[] configs = Helper.String2IntArray(m_Birthday, "_");
            if (configs == null || configs.Length < 3)
            {
                return;
            }

            m_BirthdayTime = new DateTime(configs[0], configs[1], configs[2]);
            m_IsBirthDayConfiged = true;
        }

        public DateTime GetBirthDayTime()
        {
            return m_BirthdayTime;
        }
    }
}