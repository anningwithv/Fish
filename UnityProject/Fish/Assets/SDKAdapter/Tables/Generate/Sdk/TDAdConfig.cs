//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public partial class TDAdConfig
    {

        private string m_Id;
        private int m_AdType;
        private string m_AdInterface;
        private string m_AdPlatform;
        private int m_Ecpm;
        private string m_Keyword;
        private int m_Gender;
        private string m_Birthday;
        private bool m_ForFamilies;
        private bool m_ForChild;
        private string m_UnitIDAndroid;
        private string m_UnitIDIos;
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();

        /// <summary>
        /// ID
        /// </summary>
        public string id { get { return m_Id; } }

        public int adType { get { return m_AdType; } }

        public string adInterface { get { return m_AdInterface; } }

        public string adPlatform { get { return m_AdPlatform; } }

        public int ecpm { get { return m_Ecpm; } }

        public string unitID
        {
            get
            {
#if UNITY_ANDROID
                return m_UnitIDAndroid;
#endif
                return m_UnitIDIos;
            }
        }

        public string keyword { get { return m_Keyword; } }

        public int gender { get { return m_Gender; } }

        public string birthday { get { return m_Birthday; } }

        public bool forFamilies { get { return m_ForFamilies; } }

        public bool forChild { get { return m_ForChild; } }

        public void ReadRow(DataStreamReader dataR, int[] filedIndex)
        {
            //var schemeNames = dataR.GetSchemeName();
            int col = 0;
            while (true)
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
                    m_AdType = dataR.ReadInt();
                    break;
                    case 2:
                        m_AdInterface = dataR.ReadString();
                        break;
                    case 3:
                        m_AdPlatform = dataR.ReadString();
                        break;
                    case 4:
                        m_Ecpm = dataR.ReadInt();
                        break;
                    case 5:
                    m_Keyword = dataR.ReadString();
                    break;
                case 6:
                    m_Gender = dataR.ReadInt();
                    break;
                case 7:
                    m_Birthday = dataR.ReadString();
                    break;
                case 8:
                    m_ForFamilies = dataR.ReadBool();
                    break;
                case 9:
                m_ForChild = dataR.ReadBool();
                    break;
                    case 10:
                        m_UnitIDAndroid = dataR.ReadString();
                        break;
                    case 11:
                        m_UnitIDIos = dataR.ReadString();
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
            ret.Add("AdType", 1);
            ret.Add("AdInterface", 2);
            ret.Add("AdPlatform", 3);
            ret.Add("Ecpm", 4);
            ret.Add("Keyword", 5);
            ret.Add("Gender", 6);
            ret.Add("Birthday", 7);
            ret.Add("ForFamilies", 8);
            ret.Add("ForChild", 9);
            ret.Add("UnitIDAndroid", 10);
            ret.Add("UnitIDIos", 11);
            return ret;
        }
    } 
}//namespace LR