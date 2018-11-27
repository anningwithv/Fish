using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using Firebase.Unity;
using System.Threading.Tasks;

namespace Qarth
{
    [TMonoSingletonAttribute("[SDK]/SDKMgr")]
    public class SDKMgr : TMonoSingleton<SDKMgr>
    {
        private List<System.Action> m_FirebaseDepFuncs = new List<System.Action>();
        private DependencyStatus m_FirebaseDepStatus = DependencyStatus.UnavailableOther;

        public void Init()
        {
            Log.i("Init[SDKMgr]");
            BuglyMgr.S.Init();
            DataAnalysisMgr.S.Init();
            AdsMgr.S.Init();
            SocialMgr.S.Init();
            FacebookSocialAdapter.S.Init();
#if !UNITY_EDITOR
            InitFirebaseDependence();
#endif            
        }
        public void RegisterFilebaseDepInitCB(System.Action callback)
        {
            m_FirebaseDepFuncs.Add(callback);
        }

        public void InitFirebaseDependence()
        {       
            try
            {
                FirebaseRemoteConfigMgr.S.Init();
                FirebaseMessageMgr.S.Init();
                FirebaseInstanceIDMgr.S.Init();
                
                FirebaseApp app = FirebaseApp.DefaultInstance;

                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                {
                    m_FirebaseDepStatus = task.Result;
                    if (m_FirebaseDepStatus == DependencyStatus.Available && m_FirebaseDepFuncs.Count > 0)
                    {

                        m_FirebaseDepFuncs.ForEach(p => p.Invoke());

                    }
                    else
                    {
                        Log.i("Could not resolve all Firebase dependencies: " + m_FirebaseDepStatus);
                    }
                });

                
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }
    }


}
