using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Qarth
{
    public partial class TDAppConfigTable
    {
        static void CompleteRowAdd(TDAppConfig tdData)
        {

        }

        public class ConfigHandler
        {
            public string identifier;

            public string version
            {
                get
                {
                    var data = TDAppConfigTable.GetData(string.Format("{0}.version", identifier));

                    if (data != null)
                    {
                        return data.value;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            public string name
            {
                get
                {

                    var data = TDAppConfigTable.GetData(string.Format("{0}.name_{1}", identifier, I18Mgr.S.langugePrefix));
                    if (data == null)
                    {
                        data = TDAppConfigTable.GetData(string.Format("{0}.name", identifier));
                    }

                    if (data != null)
                    {
                        return data.value;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            public string desc
            {
                get
                {

                    var data = TDAppConfigTable.GetData(string.Format("{0}.desc_{1}", identifier, I18Mgr.S.langugePrefix));
                    if (data == null)
                    {
                        data = TDAppConfigTable.GetData(string.Format("{0}.desc", identifier));
                    }

                    if (data != null)
                    {
                        return data.value;
                    }
                    else
                    {
                        return "";
                    }
                }
            }


            public string app_id
            {
                get
                {
                    var data = TDAppConfigTable.GetData(string.Format("{0}.app_id", identifier));

                    if (data != null)
                    {
                        return data.value;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            public string download_url
            {
                get
                {
                    TDAppConfig data = null;
#if UNITY_ANDROID
                    data = TDAppConfigTable.GetData(string.Format("{0}.download_android", identifier));
#else
                    data = TDAppConfigTable.GetData(string.Format("{0}.download_ios", identifier));
#endif
                    if (data != null)
                    {
                        return data.value;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            public string icon
            {
                get
                {
                    var data = TDAppConfigTable.GetData(string.Format("{0}.icon", identifier));

                    if (data != null)
                    {
                        return data.value;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            public string preview_image
            {
                get
                {
                    var data = TDAppConfigTable.GetData(string.Format("{0}.preview_image", identifier));

                    if (data != null)
                    {
                        return data.value;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            public ConfigHandler(string identifier)
            {
                this.identifier = identifier;
            }
        }

        public static void Reset()
        {
            m_ConfigHandlerList = null;
            m_ExtensionConfig = null;
        }

        private static List<ConfigHandler> m_ConfigHandlerList;
        private static List<ConfigHandler> m_ExtensionConfig;
        private static Dictionary<string, bool> m_FakeAppMap;

        private static void InitConfigHandlerList()
        {
            if (m_ConfigHandlerList == null)
            {
                m_ConfigHandlerList = new List<ConfigHandler>();
                m_FakeAppMap = new Dictionary<string, bool>();

                string platform = "";
#if UNITY_ANDROID
                platform = "Android";
#elif UNITY_IPHONE
                platform = "iOS";
#endif
                for (int i = 0; i < m_DataList.Count; ++i)
                {
                    if (m_DataList[i].value == platform || m_DataList[i].value == "Universal")
                    {
                        m_ConfigHandlerList.Add(new ConfigHandler(m_DataList[i].id));
                    }
                    else if (m_DataList[i].value == "Fake")
                    {
                        m_FakeAppMap.Add(m_DataList[i].id, true);
                    }
                }
            }
        }

        public static List<ConfigHandler> GetConfigHandlerList()
        {
            InitConfigHandlerList();

            return m_ConfigHandlerList;
        }

        public static ConfigHandler GetConfigHandler(string bundleID)
        {
            var list = GetConfigHandlerList();
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].identifier == bundleID)
                {
                    return list[i];
                }
            }

            return null;
        }

        public static bool CheckIsFakeApp(string identity)
        {
            InitConfigHandlerList();
            return m_FakeAppMap.ContainsKey(identity);
        }

        public static List<ConfigHandler> GetExtensionConfigList()
        {
            if (m_ExtensionConfig == null)
            {
                string identifier = Application.identifier;
                var list = GetConfigHandlerList();
                m_ExtensionConfig = new List<ConfigHandler>();
                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i].identifier != identifier)
                    {
                        m_ExtensionConfig.Add(list[i]);
                    }
                }
            }

            return m_ExtensionConfig;
        }

        public static ConfigHandler GetRandomExtensionConfig()
        {
            var list = GetExtensionConfigList();
            if (list == null || list.Count == 0)
            {
                return null;
            }

            int randomIndex = RandomHelper.Range(0, list.Count);
            return list[randomIndex];
        }
    }
}