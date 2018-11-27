//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public static partial class TDRemoteConfigTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDRemoteConfigTable.Parse, "remote_config");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<string, TDRemoteConfig> m_DataCache = new Dictionary<string, TDRemoteConfig>();
        private static List<TDRemoteConfig> m_DataList = new List<TDRemoteConfig >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDRemoteConfig.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDRemoteConfig.GetFieldHeadIndex(), "RemoteConfigTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDRemoteConfig memberInstance = new TDRemoteConfig();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }

            Log.i(string.Format("Parse Success TDRemoteConfig"));
        }

        private static void OnAddRow(TDRemoteConfig memberInstance)
        {
            string key = memberInstance.id;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDRemoteConfigTable Id already exists {0}", key));
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

        public static List<TDRemoteConfig> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDRemoteConfig GetData(string key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDRemoteConfig", key));
                return null;
            }
        }
    }
}//namespace LR