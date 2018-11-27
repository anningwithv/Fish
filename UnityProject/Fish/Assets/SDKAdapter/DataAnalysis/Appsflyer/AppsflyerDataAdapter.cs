using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameWish.Game;
using Qarth;

namespace Qarth
{
    public class AppsflyerDataAdapter : IDataAnalysisAdapter
    {
        public bool InitWithConfig(SDKConfig config, SDKAdapterConfig adapterConfig)
        {
            var name = AppsFlyerTrackerCallbacks.S.name;
            AppsflyerConfig appsflyerConfig = adapterConfig as AppsflyerConfig;
            AppsFlyer.setAppsFlyerKey(appsflyerConfig.appKey);
            if (appsflyerConfig.isDebugMode)
            {
                AppsFlyer.setIsDebug(true);
            }

#if UNITY_IOS
   /* Mandatory - set your apple app ID
      NOTE: You should enter the number only and not the "ID" prefix */
            AppsFlyer.setAppID(config.iosAppID);
            AppsFlyer.trackAppLaunch();
#elif UNITY_ANDROID
            /* Mandatory - set your Android package name */
            AppsFlyer.setAppID(Application.identifier);
            /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
            //AppsFlyer.init(appsflyerConfig.appKey, "AppsFlyerTrackerCallbacks");
            AppsFlyer.init(appsflyerConfig.appKey);
            AppsFlyer.loadConversionData("AppsFlyerTrackerCallbacks");

#endif

            return true;
        }

        public void CustomEvent(string eventID, string label = null)
        {
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            if (label != null)
            {
                eventValue.Add("description", label);
            }
            AppsFlyer.trackRichEvent(eventID, eventValue);
        }

        public void CustomEventDuration(string eventID, long duration)
        {

        }

        public void CustomEventMapSend(string eventID)
        {

        }

        public void CustomEventMapValue(string key, string value)
        {

        }

        public int GetPriorityScore()
        {
            return 0;
        }

        public void LevelBegin(string levelID)
        {
            
        }

        public void LevelComplate(string levelID)
        {
            
        }

        public void LevelFailed(string levelID, string reason)
        {
            
        }

        public void OnApplicationQuit()
        {
            
        }

        public void Pay(double cash, double coin)
        {
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("af_revenue", cash.ToString());
            eventValue.Add("af_content_id", coin.ToString());
            eventValue.Add("af_currency", "USD");

            AppsFlyer.trackRichEvent("af_purchase", eventValue);
        }

        public void SetUserLevel(int level)
        {
            
        }

        public void CustomEventDic(string eventId, Dictionary<string, string> dic)
        {
            if (dic != null)
            {
                AppsFlyer.trackRichEvent(eventId,dic);
            }
        }
    }
}
