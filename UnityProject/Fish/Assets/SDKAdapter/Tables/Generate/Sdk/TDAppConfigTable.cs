//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public static partial class TDAppConfigTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDAppConfigTable.Parse, "app_config");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<string, TDAppConfig> m_DataCache = new Dictionary<string, TDAppConfig>();
        private static List<TDAppConfig> m_DataList = new List<TDAppConfig >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDAppConfig.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDAppConfig.GetFieldHeadIndex(), "AppConfigTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDAppConfig memberInstance = new TDAppConfig();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDAppConfig"));
        }

        private static void OnAddRow(TDAppConfig memberInstance)
        {
            string key = memberInstance.id;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDAppConfigTable Id already exists {0}", key));
            }
            else
            {
                m_DataCache.Add(key, memberInstance);
                m_DataList.Add(memberInstance);
            }
        }    
        
        public static void Reload(byte[] fileData)
        {
            Parse(fileData);
        }

        public static int count
        {
            get 
            {
                return m_DataCache.Count;
            }
        }

        public static List<TDAppConfig> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDAppConfig GetData(string key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDAppConfig", key));
                return null;
            }
        }
    }
}//namespace LR