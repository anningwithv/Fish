using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Text;

namespace Qarth
{
    public class RemoteConfigMgr : TSingleton<RemoteConfigMgr>
    {
        private Action[] m_CheckFuns;
        private int m_CheckIndex;
        private string m_RelativePath = "config/";
        private ResLoader m_ResLoader;
        private const string KEY_LAST_UPDATE_TIME = "k_lpt123";
        private Action m_Listener;
        private List<string> m_ExtendConfigs;
        private bool m_HasInit = false;
        private bool m_IsDownloadRemoteSuccess = true;

        protected void Init()
        {
            if (m_HasInit)
            {
                return;
            }

            m_HasInit = true;

            m_ResLoader = ResLoader.Allocate("RemoteConfig");
            m_CheckFuns = new Action[]
            {
                UpdateConfigFile,
            };
        }

        public void AddExtendConfig(string name)
        {
            if (m_ExtendConfigs == null)
            {
                m_ExtendConfigs = new List<string>();
            }

            m_ExtendConfigs.Add(name);
        }

        public void StartChecker(Action listener)
        {
            Init();

            m_Listener = listener;
            if (AppConfig.S.isResUpdateActive)
            {
                m_CheckIndex = -1;
                OnStepFinish();
            }
            else
            {
                if (listener != null)
                {
                    listener();
                }
            }
        }

        public bool CheckAppHasNewVersion()
        {
            var data = TDAppConfigTable.GetConfigHandler(Application.identifier);
            if (data == null || string.IsNullOrEmpty(data.version))
            {
                return false;
            }

            float configVersion = SafetyParseFloat(data.version);
            float version = SafetyParseFloat(Application.version);
            return configVersion > version;
        }

        public float SafetyParseFloat(string value)
        {
            int a = value.IndexOf('.');
            string integerV = value.Substring(0, a);
            string decimalV = value.Substring(a, value.Length - a);
            decimalV = decimalV.Replace('.', '0');
            string totalValue = string.Format("{0}.{1}", integerV, decimalV);
            return float.Parse(totalValue);
        }

        public void OpenAppDownloadUrl(TDAppConfigTable.ConfigHandler data)
        {
            if (data == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(data.download_url))
            {
#if UNITY_ANDROID
                SocialMgr.S.OpenMarketDownloadPage(data.identifier);
#elif UNITY_IPHONE
                SocialMgr.S.OpenMarketDownloadPage(data.app_id);
#endif
            }
            else
            {
                Application.OpenURL(data.download_url);
            }
        }

        protected void UpdateConfigFile()
        {
            if (CheckNeedUpdateConfigFile())
            {
                DownloadRemoteConfigFile();
            }
            else
            {
                OnStepFinish();
            }
        }

        public bool CheckNeedUpdateConfigFile()
        {
#if UNITY_EDITOR
            return false;
#endif

            string lastUpdateTime = PlayerPrefs.GetString(KEY_LAST_UPDATE_TIME, "");

            if (string.IsNullOrEmpty(lastUpdateTime))
            {
                return true;
            }

            long passSecond = TimeHelper.PassSecond(lastUpdateTime);

            if (passSecond > 21600)
            {
                return true;
            }

            return false;
        }

        public void DownloadRemoteConfigFile()
        {
            //AddDownloadFile("ads_adapter.txt");
            AddDownloadFile("app_config.txt");
            AddDownloadFile("remote_config.txt");
            AddDownloadFile("ad_config.txt");

            if (m_ExtendConfigs != null)
            {
                for (int i = 0; i < m_ExtendConfigs.Count; ++i)
                {
                    AddDownloadFile(m_ExtendConfigs[i]);
                }
            }

            m_IsDownloadRemoteSuccess = true;
            m_ResLoader.LoadAsync(OnConfigDownloadFinish);
        }

        protected void AddDownloadFile(string name)
        {
            string resName = ResPackageHandler.AssetName2ResName(name);
            m_ResLoader.Add2Load(resName, OnConfigFileDownloadResult);

            HotUpdateRes res = ResMgr.S.GetRes<HotUpdateRes>(resName);
            string relativePath = GetAssetRelativePath(name);
            string fullPath = FilePath.persistentDataPath4Res + relativePath;
            res.SetUpdateInfo(fullPath, GetAssetUrl(relativePath), 1000, ConfigFileValidChecker);
        }

        protected bool ConfigFileValidChecker(byte[] raw)
        {
            if (raw == null || raw.Length == 0)
            {
                return false;
            }

            string config = UTF8Encoding.UTF8.GetString(raw);

            if (config.StartsWith("ID"))
            {
                return true;
            }

            return false;
        }

        protected void OnConfigFileDownloadResult(bool result, IRes res)
        {
            if (!result)
            {
                m_IsDownloadRemoteSuccess = false;
                Log.w("Update Config Error:" + res.name);
            }
            else
            {
                Log.i("Update Config Success:" + res.name);
            }
        }

        protected void OnConfigDownloadFinish()
        {
            if (m_IsDownloadRemoteSuccess)
            {
                PlayerPrefs.SetString(KEY_LAST_UPDATE_TIME, DateTime.Now.Ticks.ToString());

                TDTableMetaData[] tables = new TDTableMetaData[]
                {
                     TDRemoteConfigTable.metaData,
                     TDAdConfigTable.metaData,
                };

                ResMgr.S.PostIEnumerator(TableMgr.S.ReadAll(tables, OnTableReloadFinish));
            }

            OnStepFinish();
        }

        protected void OnTableReloadFinish()
        {
            //1.重新做安全监测
            SecurityMgr.S.DoSecurityChecker();
            //2.广告设置更新
            AdsMgr.S.ReInitAllAdData();
        }

        protected string GetAssetUrl(string path)
        {
            if (string.IsNullOrEmpty(AppConfig.S.resUpdateVersion))
            {
                return string.Format("http://35.201.102.198/{0}/{1}", SDKConfig.S.bundleID, path);
            }
            else
            {
                return string.Format("http://35.201.102.198/{0}/{1}/{2}", SDKConfig.S.bundleID, AppConfig.S.resUpdateVersion, path);
            }
            //return string.Format("https://storage.googleapis.com/game-ads-config/{0}/{1}", Application.identifier, path);
        }

        protected string GetAssetRelativePath(string abName)
        {
            return string.Format("{0}{1}", m_RelativePath, abName);
        }

        protected void OnStepFinish()
        {
            ++m_CheckIndex;
            if (m_CheckIndex >= m_CheckFuns.Length)
            {
                if (m_Listener != null)
                {
                    m_Listener();
                    m_Listener = null;
                }
                Log.i("Config Update Finish.");
                return;
            }

            m_CheckFuns[m_CheckIndex]();
        }

    }
}
