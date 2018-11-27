//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public partial class TDSocialAdapter
    {
        
       
        private string m_Id;   
        private string m_Param1;   
        private string m_Param2;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  string  id {get { return m_Id; } }
       
        /// <summary>
        /// 平台场景ID
        /// </summary>
        public  string  param1 {get { return m_Param1; } }
       
        /// <summary>
        /// 平台场景ID
        /// </summary>
        public  string  param2 {get { return m_Param2; } }
       

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
                    m_Param1 = dataR.ReadString();
                    break;
                case 2:
                    m_Param2 = dataR.ReadString();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(3);
          
          ret.Add("Id", 0);
          ret.Add("Param1", 1);
          ret.Add("Param2", 2);
          return ret;
        }
    } 
}//namespace LR