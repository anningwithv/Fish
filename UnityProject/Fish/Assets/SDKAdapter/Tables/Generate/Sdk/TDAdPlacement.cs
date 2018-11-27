//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public partial class TDAdPlacement
    {
        
       
        private string m_Id;   
        private string m_AdInterface0;   
        private string m_AdInterface1;   
        private bool m_IsEnable = false;   
        private bool m_RewardWhenDisable = false;   
        private EInt m_DisplayPrecent = 0;   
        private EInt m_TimeInterval = 0;
        private EInt m_TimeIntervalGroup = 0;
        private EInt m_MaxWaitTime;
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();

        /// <summary>
        /// ID
        /// </summary>
        public  string  id {get { return m_Id; } }
       
        /// <summary>
        /// 广告组
        /// </summary>
        public  string  adInterface0 {get { return m_AdInterface0; } }
       
        /// <summary>
        /// 广告组
        /// </summary>
        public  string  adInterface1 {get { return m_AdInterface1; } }
       
        /// <summary>
        /// 是否开启
        /// </summary>
        public  bool  isEnable {get { return m_IsEnable; } }
       
        /// <summary>
        /// 奖励方式
        /// </summary>
        public  bool rewardWhenDisable { get { return m_RewardWhenDisable; } }
       
        /// <summary>
        /// 展示百分比
        /// </summary>
        public  int  displayPrecent {get { return m_DisplayPrecent; } }
       
        /// <summary>
        /// 时间间隔
        /// </summary>
        public  int  timeInterval {get { return m_TimeInterval; } }
       
        public int timeIntervalGroup { get { return m_TimeIntervalGroup; } }

        public int maxWaitTime { get { return m_MaxWaitTime; } }

        public void ReadRow(DataStreamReader dataR, int[] filedIndex)
        {
          //var schemeNames = dataR.GetSchemeName();
          int col = 0;
          while(true)
          {
            col = dataR.MoreFieldOnRow();
            if (col == -1)
            {
              break;
            }
            switch (filedIndex[col])
            { 
            
                case 0:
                    m_Id = dataR.ReadString();
                    break;
                case 1:
                    m_AdInterface0 = dataR.ReadString();
                    break;
                case 2:
                    m_AdInterface1 = dataR.ReadString();
                    break;
                case 3:
                    m_IsEnable = dataR.ReadBool();
                    break;
                case 4:
                    m_RewardWhenDisable = dataR.ReadBool();
                    break;
                case 5:
                    m_DisplayPrecent = dataR.ReadInt();
                    break;
                case 6:
                    m_TimeInterval = dataR.ReadInt();
                    break;
                case 7:
                    m_TimeIntervalGroup = dataR.ReadInt();
                    break;
                    case 8:
                        m_MaxWaitTime = dataR.ReadInt();
                        break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
            Dictionary<string, int> ret = new Dictionary<string, int>(7);

            ret.Add("Id", 0);
            ret.Add("AdInterface0", 1);
            ret.Add("AdInterface1", 2);
            ret.Add("IsEnable", 3);
            ret.Add("RewardWhenDisable", 4);
            ret.Add("DisplayPrecent", 5);
            ret.Add("TimeInterval", 6);
            ret.Add("TimeIntervalGroup", 7);
            ret.Add("MaxWaitTime", 8);
            return ret;
        }
    } 
}//namespace LR