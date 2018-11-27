//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public static partial class TDSocialAdapterTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDSocialAdapterTable.Parse, "social_adapter");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<string, TDSocialAdapter> m_DataCache = new Dictionary<string, TDSocialAdapter>();
        private static List<TDSocialAdapter> m_DataList = new List<TDSocialAdapter >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDSocialAdapter.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDSocialAdapter.GetFieldHeadIndex(), "SocialAdapterTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDSocialAdapter memberInstance = new TDSocialAdapter();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDSocialAdapter"));
        }

        private static void OnAddRow(TDSocialAdapter memberInstance)
        {
            string key = memberInstance.id;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDSocialAdapterTable Id already exists {0}", key));
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

        public static List<TDSocialAdapter> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDSocialAdapter GetData(string key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                //Log.w(string.Format("Can't find key {0} in TDSocialAdapter", key));
                return null;
            }
        }
    }
}//namespace LR