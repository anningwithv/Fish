using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class SecurityMgr : TSingleton<SecurityMgr>
    {
        private bool m_IsBeHacked = false;
        private bool m_IsInValidBundleID = false;
        private int m_OfficialVersionTimer = -1;
        private delegate bool CheckerFunc();

        public bool isBeHacked
        {
            get { return m_IsBeHacked; }
        }

        public bool isInvalidBundleID
        {
            get { return m_IsInValidBundleID; }
        }

        public void DoSecurityChecker()
        {
            m_IsBeHacked = false;

            CheckerFunc[] checkers = new CheckerFunc[]
                {
                    CheckBundleID,
                    CheckSignID,
                };

            for (int i = 0; i < checkers.Length; ++i)
            {
                if (!checkers[i]())
                {
                    QuitGameWithMsg("-");
                    return;
                }
            }

            CheckIsUnpublishPackage();
        }

        protected void QuitGameWithMsg(string msg)
        {
            if (m_IsBeHacked)
            {
                return;
            }

            m_IsBeHacked = true;
            //Timer.S.Post2Scale(ShowErrorTipsTick, 5);
            //Timer.S.Post2Scale(QuitApplicationTick, 8);
            DataAnalysisMgr.S.CustomEventSingleton(DataAnalysisDefine.FAKE_APP, Application.identifier);

            if (m_OfficialVersionTimer <= 0)
            {
                if (TDAppConfigTable.CheckIsFakeApp(Application.identifier))
                {
                    m_OfficialVersionTimer = Timer.S.Post2Scale(ShowOfficalVersionAD, 1, -1);
                }
            }

#if UNITY_EDITOR
            Log.e("Your version is unsafe!:" + msg);
#endif
        }

        private void ShowErrorTipsTick(int count)
        {
            //FloatMessage.S.ShowMsg("Your version is unsafe, please download the official version.");
        }

        private void QuitApplicationTick(int count)
        {
            //Application.Quit();
        }

        private void ShowOfficalVersionAD(int count)
        {
            UIMgr.S.OpenPanel(SDKUI.OfficialVersionAdPanel);
        }

        private void CheckIsUnpublishPackage()
        {
            if (Application.identifier != TDRemoteConfigTable.GetOfficialBundleID())
            {
                DataAnalysisMgr.S.CustomEventSingleton(DataAnalysisDefine.FAKE_APP, Application.identifier);

                if (m_OfficialVersionTimer <= 0)
                {
                    m_OfficialVersionTimer = Timer.S.Post2Scale(ShowOfficalVersionAD, 1);
                }
            }
        }

        private bool CheckBundleID()
        {
            if (SDKConfig.S.bundleID == Application.identifier)
            {
                return true;
            }

            m_IsInValidBundleID = true;
            return false;
        }

        private bool CheckSignID()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return IsCorrect();
#endif
            return true;
        }

        private bool IsCorrect()
        {
            AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject Activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject PackageManager = Activity.Call<AndroidJavaObject>("getPackageManager");

            string packageName = Activity.Call<string>("getPackageName");

            if (SDKConfig.S.bundleID != packageName)
            {
                m_IsInValidBundleID = true;
                return false;
            }

            int GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
            AndroidJavaObject PackageInfo = PackageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
            AndroidJavaObject[] Signatures = PackageInfo.Get<AndroidJavaObject[]>("signatures");

            if (Signatures != null && Signatures.Length > 0)
            {
                int hashCode = Signatures[0].Call<int>("hashCode");
                Log.i("#Android: + hasCode:" + hashCode);

                if (SDKConfig.S.signatures == -1)
                {
                    return true;
                }
                else
                {
                    return hashCode == SDKConfig.S.signatures;
                }
            }

            return false;
        }
    }
}
