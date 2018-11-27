//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public static partial class TDAdPlacementTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDAdPlacementTable.Parse, "ad_placement");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<string, TDAdPlacement> m_DataCache = new Dictionary<string, TDAdPlacement>();
        private static List<TDAdPlacement> m_DataList = new List<TDAdPlacement >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDAdPlacement.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDAdPlacement.GetFieldHeadIndex(), "AdPlacementTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDAdPlacement memberInstance = new TDAdPlacement();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDAdPlacement"));
        }

        private static void OnAddRow(TDAdPlacement memberInstance)
        {
            string key = memberInstance.id;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDAdPlacementTable Id already exists {0}", key));
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

        public static List<TDAdPlacement> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDAdPlacement GetData(string key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDAdPlacement", key));
                return null;
            }
        }
    }
}//namespace LR