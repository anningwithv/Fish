//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public partial class TDPurchase
    {
        
       
        private string m_Id;   
        private EInt m_ItemID = 0;   
        private bool m_IsConsume = false;   
        private EInt m_Price = 0;   
        private EInt m_ItemNum = 0;   
        private string m_IOSKey;   
        private string m_AndroidKey;
        private string m_ServiceKey;
        private EInt m_PurchaseState;
        private string m_ItemIcon;
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  string  id {get { return m_Id; } }
       
        /// <summary>
        /// 类型
        /// </summary>
        public  int  itemID {get { return m_ItemID; } }
       
        /// <summary>
        /// 是不是消耗品
        /// </summary>
        public  bool  isConsume {get { return m_IsConsume; } }
       
        /// <summary>
        /// 价格
        /// </summary>
        public  int  price {get { return m_Price; } }
       
        /// <summary>
        /// 购买数量
        /// </summary>
        public  int  itemNum {get { return m_ItemNum; } }
       
        /// <summary>
        /// 苹果ID
        /// </summary>
        public  string  iOSKey {get { return m_IOSKey; } }
       
        /// <summary>
        /// 安卓ID
        /// </summary>
        public  string  androidKey {get { return m_AndroidKey; } }
       
        public string serviceKey { get { return m_ServiceKey; } } 

        public int purchaseState { get { return m_PurchaseState; } }

        public string itemIcon { get { return m_ItemIcon; } }

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
                        m_ItemID = dataR.ReadInt();
                        break;
                    case 2:
                        m_IsConsume = dataR.ReadBool();
                        break;
                    case 3:
                        m_Price = dataR.ReadInt();
                        break;
                    case 4:
                        m_ItemNum = dataR.ReadInt();
                        break;
                    case 5:
                        m_IOSKey = dataR.ReadString();
                        break;
                    case 6:
                        m_AndroidKey = dataR.ReadString();
                        break;
                    case 7:
                        m_ServiceKey = dataR.ReadString();
                        break;
                    case 8:
                        m_PurchaseState = dataR.ReadInt();
                        break;
                    case 9:
                        m_ItemIcon = dataR.ReadString();
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
            ret.Add("ItemID", 1);
            ret.Add("IsConsume", 2);
            ret.Add("Price", 3);
            ret.Add("ItemNum", 4);
            ret.Add("IOSKey", 5);
            ret.Add("AndroidKey", 6);
            ret.Add("ServiceKey", 7);
            ret.Add("PurchaseState", 8);
            ret.Add("ItemIcon", 9);
            return ret;
        }
    } 
}//namespace LR