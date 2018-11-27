using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{

    public enum PurchaseState
    {
        NORMAL = 0,
        RECOMMAND,
        HOT,
        SOLDOUT,
        LIMITTIME,
    }

    public partial class TDPurchaseTable
    {
        static void CompleteRowAdd(TDPurchase tdData)
        {

        }

        private static string m_AllKeyJson;
        public static string GetAllKeyJson()
        {
            if (m_AllKeyJson == null)
            {
                LitJson.JsonData data = new LitJson.JsonData();

                for (int i = 0; i < m_DataList.Count; ++i)
                {
                    data.Add(GetConfigKey(m_DataList[i]));
                }

                m_AllKeyJson = data.ToJson();
            }
            return m_AllKeyJson;
        }

        public static string GetConfigKey(TDPurchase data)
        {
#if UNITY_ANDROID
                string configKey = data.androidKey;
#elif UNITY_IOS
                string configKey = data.iOSKey;
#else
                string configKey = data.androidKey;
#endif
            return configKey;
        }

        public static TDPurchase GetPurchaseDataByKey(string key)
        {
            for (int i = 0; i < m_DataList.Count; ++i)
            {
                string configKey = GetConfigKey(m_DataList[i]);
                if (configKey == key)
                {
                    return m_DataList[i];
                }
            }

            return null;
        }


    }
}