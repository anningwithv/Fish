using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public partial class TDPurchase
    {
        public void Reset()
        {

        }

        public string localPriceString
        {
            get;
            set;
        }

        public PurchaseState GetPurchaseState()
        {
            return (PurchaseState) purchaseState;
        }
    }
}