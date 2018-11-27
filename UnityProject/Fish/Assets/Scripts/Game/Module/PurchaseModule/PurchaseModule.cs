using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Reflection;

namespace GameWish.Game
{
    public class PurchaseModule : TSingleton<PurchaseModule>
    {
        private Type m_PurchaseServiceType;

        public Type purchaseServiceType
        {
            get
            {
                if (m_PurchaseServiceType == null)
                {
                    m_PurchaseServiceType = Type.GetType("GameWish.Game.PurchaseModule");
                }

                return m_PurchaseServiceType;
            }
        }

        public void Init()
        {
            EventSystem.S.Register(SDKEventID.OnPurchaseSuccess, OnPurchaseSuccess);
            PurchaseMgr.S.Init();
            PurchaseMgr.S.InitPurchaseInfo();
        }

        protected void OnPurchaseSuccess(int key, params object[] args)
        {
            Log.w(key + "  买钻石结束  ");
            TDPurchase data = args[0] as TDPurchase;
            AddDiamondCount(data);
            Log.w(key+"  买钻石结束  data.id = "+data.id+"  data.itemID="+data.itemID);
           

            //if (data.itemID == 1)//gem
            //{
            //    PlayerInfoMgr.AddGems(data.itemNum);
            //    //FloatMessage.S.ShowMsg(TDLanguageTable.GetFormat("UI_PURCHASE_SUCCESS", data.itemNum));
            //}
            //else if (data.itemID == 2)//box
            //{
            //    var lstShow = PlayerInfoMgr.data.equipData.GenRewardPiece(data.itemNum);
            //    UIMgr.S.OpenPanel(UIID.EquipBoxPanel, lstShow);
            //}
            //else if (data.itemID == 3)//gem+spin
            //{
            //    PlayerInfoMgr.AddGems(data.itemNum);
            //    PlayerPrefs.SetInt(Define.FREE_SPIN_SAVEKEY, PlayerPrefs.GetInt(Define.FREE_SPIN_SAVEKEY, 0) + 1);
            //    //spin
            //}

           //ProcessPurchaseService(data);
        }

        void AddDiamondCount(TDPurchase data)
        {
            EventSystem.S.Send(EventID.OnAddDiamondCount, data.itemNum);
        }

        public string GetPriceText(TDPurchase data)
        {
            if (string.IsNullOrEmpty(data.localPriceString))
            {
                return !data.price.Equals(0) ? string.Format("${0}", data.price / 100f) : "";
                //TDLanguageTable.GetFormat("UI_PURCHASETITLE", m_Data.price);           
            }
            else
            {
                return data.localPriceString;
            }
        }

        // protected void ProcessPurchaseSuccess(TDPurchase data)
        // {
        //     // if (!TDShopTable.CheckHasData(data.id))
        //     // {
        //     //     return;
        //     // }

        //     // TDShop shopData = TDShopTable.GetData(data.id);

        //     // for (int i = 0; i < TDShopTable.MAX_PACKAGE_COUNT; ++i)
        //     // {
        //     //     if (shopData.GetItemId(i) > 0)
        //     //     {
        //     //         PlayerInfoMgr.data.AddItemCount(shopData.GetItemId(i), shopData.GetItemCount(i));
        //     //         //PlayerInfoMgr.Save();
        //     //     }
        //     // }
        // }

        protected void ProcessPurchaseService(TDPurchase data)
        {
            if (data == null || string.IsNullOrEmpty(data.serviceKey))
            {
                return;
            }

            Type type = purchaseServiceType;

            if (type == null)
            {
                return;
            }

            MethodInfo servicemethod = type.GetMethod(data.serviceKey);

            if (servicemethod == null)
            {
                Log.e("Invalid Purchase Service Name:" + data.serviceKey);
                return;
            }

            try
            {
                servicemethod.Invoke(null, new object[] { data });
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }

        public void ShowRewardVideo()
        {
            // AdDisplayer.Builder()
            // .SetFirstADName(Define.AD_REWARD)
            // .SetMaxWaitTime(5)
            // .SetOnAdShowResultCallback(OnRewardVideoShowResult)
            // .Show("PurchaseRewardVideo");
        }

        protected void OnRewardVideoShowResult(bool show, bool reward, string rewardID)
        {
            if (show && reward)
            {
                //ItemMgr.data.AddGoldCount(10);
                //MessageTipsPanel.ShowMessageTipsPanel(MessageTipsPanel.UIMode.GREED, TDLanguageTable.GetFormat("UI_BambooTips", 10), null);
                //EventSystem.S.Send(EventID.OnRewardVideoSuccess);
            }
        }
    }
}
