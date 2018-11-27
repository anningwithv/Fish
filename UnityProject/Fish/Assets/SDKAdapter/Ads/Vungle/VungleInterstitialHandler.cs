//using System;
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using Qarth;

//namespace Qarth
//{
//    public class VungleInterstitialHandler : AdFullScreenHandler
//    {
//        private bool m_HasInit;

//        public override bool isAdReady
//        {
//            get
//            {
//                return Vungle.isAdvertAvailable(m_Config.unitID);
//            }
//        }

//        protected override bool DoPreLoadAd()
//        {
//            if (string.IsNullOrEmpty(m_Config.unitID))
//            {
//                return false;
//            }

//            if (isAdReady)
//            {
//                return false;
//            }

//            if (!m_HasInit)
//            {
//                m_HasInit = true;

//                Vungle.onAdStartedEvent += OnAdStartEvent;
//                Vungle.onAdFinishedEvent += OnAdFinishEvent;
//                Vungle.adPlayableEvent += OnAdPlayableEvent;
//                Vungle.onLogEvent += OnAdLogEvent;
//            }

//            Vungle.loadAd(m_Config.unitID);

//            return true;
//        }

//        protected override bool DoShowAd()
//        {
//            Vungle.playAd(m_Config.unitID);
//            return true;
//        }

//        protected bool CheckIsCurrentAdUnit(string adunit)
//        {
//            return adunit == m_Config.unitID;
//        }

//        protected void OnAdStartEvent(string unitID)
//        {
//            if (!CheckIsCurrentAdUnit(unitID))
//            {
//                return;
//            }
//        }

//        protected void OnAdFinishEvent(string unitID, AdFinishedEventArgs args)
//        {
//            if (!CheckIsCurrentAdUnit(unitID))
//            {
//                return;
//            }

//            if (args.IsCompletedView)
//            {
//                HandleOnAdOpened();
//            }

//            if (args.WasCallToActionClicked)
//            {
//                HandleOnAdClick();
//            }

//            HandleOnAdClosed();
//        }

//        protected void OnAdPlayableEvent(string unitID, bool result)
//        {
//            if (!CheckIsCurrentAdUnit(unitID))
//            {
//                return;
//            }

//            if (result)
//            {
//                HandleOnAdLoaded();
//            }
//            else
//            {
//                HandleOnAdFailedToLoad("");
//            }
//        }

//        protected void OnAdLogEvent(string unitID)
//        {
//            if (!CheckIsCurrentAdUnit(unitID))
//            {
//                return;
//            }
//        }
//    }
//}
