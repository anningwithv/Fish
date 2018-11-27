//using System;
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using Qarth;

//namespace Qarth
//{
//    public class DataeyeAdapter : AbstractSDKAdapter, IDataAnalysisAdapter
//    {
//        protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
//        {
//            if (string.IsNullOrEmpty(config.dataAnalysisConfig.dataeyeConfig.appID) || string.IsNullOrEmpty(config.channelID))
//            {
//                Log.w("Invalid Dataeye Config.");
//                return false;
//            }
//            //BeginCallApi();

//            DataeyeConfig dataeyeConfig = adapterConfig as DataeyeConfig;

//            DCAgent.setDebugMode(adapterConfig.isDebugMode);
//            DCAgent.getInstance().initWithAppIdAndChannelId(dataeyeConfig.appID, config.channelID);
//            //EndCallApi();

//            return true;
//        }

//        public void OnApplicationQuit()
//        {
//#if UNITY_ANDROID && !UNITY_EDITOR
//            //BeginCallApi();
//            DCAgent.onKillProcessOrExit();
//            //EndCallApi();
//#endif
//        }

//        public void LevelBegin(string levelID)
//        {
//            DCLevels.begin(levelID);
//        }

//        public void LevelComplate(string levelID)
//        {
//            DCLevels.complete(levelID);
//        }

//        public void LevelFailed(string levelID, string reason)
//        {
//            DCLevels.fail(levelID, reason);
//        }

//        public void CustomEvent(string eventID, string label = null)
//        {
//            if (string.IsNullOrEmpty(label))
//            {
//                DCEvent.onEvent(eventID);
//            }
//            else
//            {
//                DCEvent.onEvent(eventID, label);
//            }
//        }

//        public void CustomEventDuration(string eventID, long duration)
//        {
//            DCEvent.onEventDuration(eventID, duration);
//        }

//        private Dictionary<string, string> m_EventMap;
//        public void CustomEventMapValue(string key, string value)
//        {
//            if (m_EventMap == null)
//            {
//                m_EventMap = new Dictionary<string, string>();
//            }
//            m_EventMap.Add(key, value);
//        }

//        public void CustomEventMapSend(string eventID)
//        {
//            DCEvent.onEvent(eventID, m_EventMap);
//            m_EventMap.Clear();
//        }

//        private void BeginCallApi()
//        {
//#if UNITY_ANDROID && !UNITY_EDITOR
//            DCAgent.attachCurrentThread();
//#endif
//        }

//        private void EndCallApi()
//        {
//#if UNITY_ANDROID && !UNITY_EDITOR
//            DCAgent.detachCurrentThread();
//#endif
//        }
//    }
//}
