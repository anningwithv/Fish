using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using Sdkbox;
namespace Qarth
{
    /*
    public class PurchaseInitResult
    {
        public class PurchaseItemInfo
        {
            public string priceLocale;
            public string productIdentifier;
        }

        private List<PurchaseItemInfo> m_PurchaseDataList;

        public PurchaseInitResult(string msg)
        {
            m_PurchaseDataList = JsonMapper.ToObject<List<PurchaseItemInfo>>(msg);
        }
    }
    */

    [TMonoSingletonAttribute("PurchaseMgr")]
    public class PurchaseMgr : TMonoSingleton<PurchaseMgr>
    {
        private PurchaseAdapter m_Adapter;
        private bool m_IsPurchaseReady = false;

        private float m_PrePurchaseTime = -1;
        private bool m_IsWaitPauseEvent = false;

        private bool isPurchaseBeHacked
        {
            get
            {
                return PlayerPrefs.GetInt("PCKED", 0) > 0;
            }

            set
            {
                if (value)
                {
                    PlayerPrefs.SetInt("PCKED", 10);
                }
                else
                {
                    PlayerPrefs.SetInt("PCKED", 0);
                }
            }
        }

        public bool isPurchaseReady
        {
            get { return m_IsPurchaseReady; }
        }

        [SerializeField]
        private IAP m_Iap;

        [SerializeField]
        private Purchases m_Sub;

        public Purchases subscriber
        {
            get { return m_Sub; }
        }

        private bool m_Inited;

        public void Init()
        {
            if (m_Iap == null)
            {
                Log.w("Not Find Valid IAP.");
                return;
            }

            if (m_Inited)
            {
                return;
            }

            m_Inited = true;
            ThreadMgr.S.Init();

            m_Iap.callbacks.onProductRequestSuccess.AddListener(OnProductRequestSuccess);
            m_Iap.callbacks.onSuccess.AddListener(OnProductPurchaseSuccess);
            m_Iap.callbacks.onCanceled.AddListener(OnProductPurchaseFailed);
            m_Iap.callbacks.onFailure.AddListener(OnProductPurchaseCancled);
            m_Iap.callbacks.onProductRequestFailure.AddListener(OnProductRequestFailed);
#if UNITY_ANDROID
            m_Adapter = new PurchaseAdapterAndroid();
#elif UNITY_IOS
			m_Adapter = new PurchaseAdapterIOS();
#else
            m_Adapter = new PurchaseAdapter();
#endif
        }


        public bool IsSupportPurchase
        {
            get
            {
                return m_Adapter.IsSupportPurchase;
            }
        }

        public void InitPurchaseInfo()
        {
            if (m_Iap == null)
            {
                Log.w("Not Find Valid IAP.");
                return;
            }

            //m_Adapter.InitPurchaseInfo(TDPurchaseTable.GetAllKeyJson());
            m_Iap.getProducts();
        }

        public void DoPurchase(TDPurchase data)
        {
            if (m_Iap == null)
            {
                Log.w("Not Find Valid IAP.");
                return;
            }

            if (data == null)
            {
                Log.i("purchase data null");
                return;
            }

            if (SecurityMgr.S.isBeHacked)
            {
                Log.i("security hacked");
                return;
            }

            if (isPurchaseBeHacked)
            {
                Log.i("purchase hacked");
                return;
            }

            //m_Adapter.DoPurchase(TDPurchaseTable.GetConfigKey(data));
            m_IsWaitPauseEvent = true;

            m_Iap.purchase(data.id);

            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.PURCHASE_REQUEST, data.id);
        }

        #region 回调
        public void OnInitPurchaseResult(string msg)
        {
            try
            {
                //PurchaseInitResult result = new PurchaseInitResult(msg);
                m_IsPurchaseReady = true;
                EventSystem.S.Send(SDKEventID.OnPurchaseInitSuccess);
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }

        #endregion

        #region IAP 

        private class PurchaseSuccessTask : IThreadTask
        {
            protected Product m_Product;

            public PurchaseSuccessTask(Product product)
            {
                m_Product = product;
            }

            public bool Execute()
            {
                TDPurchase data = TDPurchaseTable.GetData(m_Product.name);
                if (data == null)
                {
                    Log.e("Invalid Config Key");
                    return true;
                }
                DataAnalysisMgr.S.Pay((float)data.price / 100, data.itemNum);
                DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.PURCHASE_SUCCESS, data.id);
                EventSystem.S.Send(SDKEventID.OnPurchaseSuccess, data);
                return true;
            }

            public void ProcessResult()
            {

            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            m_IsWaitPauseEvent = false;
        }

        public void OnProductPurchaseSuccess(Product product)
        {
            if ((Time.realtimeSinceStartup - m_PrePurchaseTime) < 5)
            {
                isPurchaseBeHacked = true;
                return;
            }

            if (m_IsWaitPauseEvent)
            {
                //isPurchaseBeHacked = true;
                return;
            }

            m_PrePurchaseTime = Time.realtimeSinceStartup;
            ThreadMgr.S.mainThread.PostTask(new PurchaseSuccessTask(product));
        }

        public void OnProductRequestSuccess(Product[] products)
        {
            m_IsPurchaseReady = true;
            for (int i = 0; i < products.Length; ++i)
            {
                TDPurchase data = TDPurchaseTable.GetData(products[i].name);
                if (data == null)
                {
                    Log.w("Purchase Table Config InValid.");
                    continue;
                }

                data.localPriceString = products[i].price;
            }
        }

        public void OnProductPurchaseCancled(Product product, string res)
        {
            TDPurchase data = TDPurchaseTable.GetData(product.name);
            if (data == null)
            {
                Log.e("Invalid Config Key");
                return;
            }
            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.PURCHASE_CANCEL, res);
        }

        public void OnProductPurchaseFailed(Product product)
        {
            TDPurchase data = TDPurchaseTable.GetData(product.name);
            if (data == null)
            {
                Log.e("Invalid Config Key");
                return;
            }
            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.PURCHASE_FAILED, data.id);
        }

        public void OnProductRequestFailed(string res)
        {
            ThreadMgr.S.mainThread.PostAction(() =>
            {
                Timer.S.Post2Really(ReInitPurchaseInfo, 10);
                DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.PURCHASE_PRODUCT_FAILED, res);
            });
        }

        private void ReInitPurchaseInfo(int count)
        {
            InitPurchaseInfo();
        }

        #endregion
    }
}
