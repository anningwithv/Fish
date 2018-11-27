using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AdPlacement
    {
        private static Dictionary<int, float> m_TimeRecordMap = new Dictionary<int, float>();

        protected TDAdPlacement m_Data;
        protected float m_RecordShowTime;

        public float lastShowTime
        {
            get
            {
                if (m_Data.timeIntervalGroup < 0)
                {
                    return m_RecordShowTime;
                }
                else
                {
                    float time = 0;

                    m_TimeRecordMap.TryGetValue(m_Data.timeIntervalGroup, out time);

                    return time;
                }
            }
        }

        public TDAdPlacement data
        {
            get { return m_Data; }
        }

        public AdPlacement(TDAdPlacement data)
        {
            m_Data = data;
        }

        public void CheckAdInterfaceValid()
        {
            if (m_Data == null)
            {
                return;
            }


            if (!string.IsNullOrEmpty(m_Data.adInterface0))
            {
                if (AdsMgr.S.GetAdInterface(m_Data.adInterface0) == null)
                {
                    Log.e("Not Find AdInterface For AdPlacement:" + m_Data.id);
                }
            }

            if (!string.IsNullOrEmpty(m_Data.adInterface1))
            {
                if (AdsMgr.S.GetAdInterface(m_Data.adInterface1) == null)
                {
                    Log.e("Not Find AdInterface For AdPlacement:" + m_Data.id);
                }
            }

        }

        public void RecordShowTime()
        {
            if (m_Data.timeIntervalGroup < 0)
            {
                m_RecordShowTime = Time.realtimeSinceStartup;
            }
            else
            {
                if (m_TimeRecordMap.ContainsKey(m_Data.timeIntervalGroup))
                {
                    m_TimeRecordMap.Remove(m_Data.timeIntervalGroup);
                }

                m_TimeRecordMap.Add(m_Data.timeIntervalGroup, Time.realtimeSinceStartup);
            }
        }

        public bool IsTimeShowAble()
        {
            if (m_Data.timeInterval <= 0)
            {
                return true;
            }

            return Time.realtimeSinceStartup - lastShowTime >= m_Data.timeInterval;
        }
    }
}
