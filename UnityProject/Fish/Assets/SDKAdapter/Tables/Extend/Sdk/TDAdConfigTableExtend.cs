using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public partial class TDAdConfigTable
    {
        static void CompleteRowAdd(TDAdConfig tdData)
        {

        }

        public static List<TDAdConfig> GetAdDataByPlatform(string platform)
        {
            platform = platform.ToLower();

            List<TDAdConfig> result = new List<TDAdConfig>();

            for (int i = 0; i < m_DataList.Count; ++i)
            {
                if (m_DataList[i].adPlatform == platform)
                {
                    result.Add(m_DataList[i]);
                }
            }

            return result;
        }

        public static List<TDAdConfig> GetAdDataByInterface(string interfaceName)
        {
            List<TDAdConfig> result = new List<TDAdConfig>();

            for (int i = 0; i < m_DataList.Count; ++i)
            {
                if (m_DataList[i].adInterface == interfaceName)
                {
                    result.Add(m_DataList[i]);
                }
            }

            return result;
        }
    }
}