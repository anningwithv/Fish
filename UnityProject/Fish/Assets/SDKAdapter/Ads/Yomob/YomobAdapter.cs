using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
using Together;

namespace Qarth
{
    public class YomobAdapter : IAdsAdapter
    {
        public bool InitWithConfig(SDKConfig config)
        {
            if (string.IsNullOrEmpty(GetAdsAppID(config)))
            {
                Log.w("Invalid Yomob config id.");
                return false;
            }

            TGSDK.SetDebugModel(config.debugMode);
            TGSDK.Initialize(GetAdsAppID(config));
            Log.i("Init[YomobAdapter]");
            return true;
        }

        public void PreloadAd()
        {
            //配置加载的回调
            TGSDK.PreloadAdSuccessCallback = OnPreloadAdConfigSuccess;
            TGSDK.PreloadAdFailedCallback = OnPreloadAdConfigFailed;
            TGSDK.CPAdLoadedCallback = OnCPAdLoaded;
            TGSDK.VideoAdLoadedCallback = OnVideoAdLoaded;

            //显示回调
            TGSDK.AdShowSuccessCallback = OnAdShowSuccess;
            TGSDK.AdShowFailedCallback = OnAdShowFailed;
            TGSDK.AdCompleteCallback = OnAdComplete;
            TGSDK.AdCloseCallback = OnAdClose;
            TGSDK.AdClickCallback = OnAdClick;
            TGSDK.AdRewardSuccessCallback = OnAdRewardSuccess;
            TGSDK.AdRewardFailedCallback = OnAdRewardFailed;

            TGSDK.PreloadAd();
        }

        public bool ShowAd(string name)
        {
            string adsID = AdsName2YomobID(name);
            if (string.IsNullOrEmpty(adsID))
            {
                return false;
            }

            if (!TGSDK.CouldShowAd(adsID))
            {
                //Log.i("Yomob Ads not Ready:" + name + " AdsID:" + adsID);
                EventSystem.S.Send(EngineEventID.OnAdNotReadyEvent, name);
                return false;
            }

            TGSDK.ShowAd(adsID);
            //Log.i("Success Show Yomob Ads:" + name + " AdsID:" + adsID);
            return true;
        }

        public void ShowTestView(string name)
        {
            string adsID = AdsName2YomobID(name);
            if (string.IsNullOrEmpty(adsID))
            {
                return;
            }

            TGSDK.ShowTestView(adsID);
        }

        public void ReportShowAd(string name)
        {
            string adsID = AdsName2YomobID(name);
            if (string.IsNullOrEmpty(adsID))
            {
                return;
            }

            TGSDK.ShowAdScene(adsID);
        }

        public void ReportRejectShowAd(string name)
        {
            string adsID = AdsName2YomobID(name);
            if (string.IsNullOrEmpty(adsID))
            {
                return;
            }

            TGSDK.ReportAdRejected(adsID);
        }

        private string GetAdsAppID(SDKConfig config)
        {
#if UNITY_ANDROID
            return config.yomobAppID_Android;
#elif UNITY_IPHONE
            return config.yomobAppID_iOS;
#else
            return "";
#endif
        }

        private string AdsName2YomobID(string name)
        {
            TDAdsAdapter data = TDAdsAdapterTable.GetData(name);
            if (data == null)
            {
                return null;
            }
#if UNITY_ANDROID
            return data.yomobAndroid;
#elif UNITY_IPHONE
            return data.yomobIos;
#else
            return "";
#endif
        }

#region 加载回调
        //配置加载
        private void OnPreloadAdConfigSuccess(string msg)
        {

        }

        private void OnPreloadAdConfigFailed(string msg)
        {

        }

        private void OnCPAdLoaded(string msg)
        {

        }

        private void OnVideoAdLoaded(string msg)
        {

        }

        private void OnAdShowSuccess(string msg)
        {

        }

        private void OnAdShowFailed(string error)
        {
        }

        private void OnAdComplete(string ret)
        {
        }

        private void OnAdClose(string ret)
        {
            //Log.i("#AdClose:" + ret);
            EventSystem.S.Send(EngineEventID.OnAdCloseEvent);
        }

        private void OnAdClick(string ret)
        {
            EventSystem.S.Send(EngineEventID.OnAdClickEvent);
        }

        private void OnAdRewardSuccess(string ret)
        {
            EventSystem.S.Send(EngineEventID.OnAdRewardSuccess);
        }

        private void OnAdRewardFailed(string error)
        {
            EventSystem.S.Send(EngineEventID.OnAdRewardFailed);
        }

#endregion
    }
}
*/