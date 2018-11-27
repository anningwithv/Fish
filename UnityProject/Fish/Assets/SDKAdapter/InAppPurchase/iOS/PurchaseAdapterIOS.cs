using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Runtime.InteropServices;

namespace Qarth
{
    public class PurchaseAdapterIOS : PurchaseAdapter
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport ("__Internal")]  
        private static extern void _queryAllProductsInfo(string msg);
        [DllImport ("__Internal")]  
        private static extern void _buyProductWithId(string msg);  
#endif

        public override void DoPurchase(string key)
        {
#if UNITY_IOS && !UNITY_EDITOR
            _buyProductWithId(key);
#endif
        }

        public override void InitPurchaseInfo(string keyJson)
        {
#if UNITY_IOS && !UNITY_EDITOR
            _queryAllProductsInfo(keyJson);
#endif
        }
    }
}
