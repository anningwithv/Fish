using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.RemoteConfig;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace Qarth
{
	public class FirebaseRemoteConfigMgr : TSingleton<FirebaseRemoteConfigMgr>
	{
		private bool m_IsInit = false;
		private Dictionary<string ,object> m_Datawarp = new Dictionary<string, object>();

        public void Init()
		{
            if (!SDKConfig.S.firebaseConfig.isEnable)
            {
                return;
            }

			SDKMgr.S.RegisterFilebaseDepInitCB(()=>
			{
				FetchData();

				Log.i("FirebaseRemoteConfig init success");
			});
		}

		public void FetchData()
		{
			Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(System.TimeSpan.Zero);
            fetchTask.ContinueWith(FetchComplete);
		}

        public long GetValueLong(FirebaseDataWrap data)
        {
            return GetValueLong(data.key, data.defaultValueLong);
        }

		public long GetValueLong(string key, long defaultValue)
		{
			if(!m_IsInit)
			{
				return defaultValue;
			}

			return Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(key).LongValue;
		}

        public bool GetValueBool(FirebaseDataWrap data)
        {
            return GetValueBool(data.key, data.defaultValueBool);
        }

		public bool GetValueBool(string key, bool defaultValue)
		{
			if(!m_IsInit)
			{
				return defaultValue;
			}

			return Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(key).BooleanValue;
		}

        public string GetValueString(FirebaseDataWrap data)
        {
            return GetValueString(data.key, data.defaultValueString);
        }

        public string GetValueString(string key, string defaultValue)
		{
			if(!m_IsInit)
			{
				return defaultValue;
			}

			return Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(key).StringValue;
		}

        public double GetValueDouble(FirebaseDataWrap data)
        {
            return GetValueDouble(data.key, data.defaultValueDouble);
        }

        public double GetValueDouble(string key, double defaultvalue)
		{		
			if(!m_IsInit)
			{
				return defaultvalue;
			}
            
			return Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(key).DoubleValue;			
		}

		public ConfigValue GetConfigValue(string key)
		{
			return Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(key);
		}

        private void FetchComplete(Task fetchTask)
        {
    		var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
    		switch (info.LastFetchStatus) 
			{
    			case Firebase.RemoteConfig.LastFetchStatus.Success:
     	 			Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                    m_IsInit = true;
                    break;

    			case Firebase.RemoteConfig.LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            break;
                        default:
                            break;
                    }

                    break;
   		 		case Firebase.RemoteConfig.LastFetchStatus.Pending:

         			break;
    		}
        }

		public void AddDefaultsValue(FirebaseDataWrap wrap)
		{
			if(!m_Datawarp.ContainsKey(wrap.key))
			{
				m_Datawarp.Add(wrap.key, wrap.defaultValue);

				Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(m_Datawarp);
			}
		}
    }
}