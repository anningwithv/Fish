using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;

namespace Qarth
{
    public partial class TDRemoteConfigTable
    {
        private static string OFFICIAL_PACKAGE_IOS = "official_ios";
        private static string OFFICIAL_PACKAGE_ANDROID = "official_android";

        static void CompleteRowAdd(TDRemoteConfig tdData)
        {

        }

        public static int QueryInt(string type, int defaultValue = 0)
        {
            TDRemoteConfig td = GetData(type);

            if (td != null)
            {
                return Helper.String2Int(td.value);
            }

            return defaultValue;
        }

        public static float QueryFloat(string type, float defaultValue = 0)
        {
            TDRemoteConfig td = GetData(type);

            if (td != null)
            {
                return Helper.String2Float(td.value);
            }

            return defaultValue;
        }

        public static string QueryString(string type, string defaultValue = "")
        {
            TDRemoteConfig td = GetData(type);

            if (td != null)
            {
                return td.value;
            }

            return defaultValue;
        }

        public static List<float> QueryFloatList(string type, List<float> defaultValue = null)
        {
            TDRemoteConfig td = GetData(type);
            if (td != null)
            {
                return Helper.String2ListFloat(td.value, "#");
            }

            return defaultValue;
        }

        public static List<int> QueryIntList(string type, List<int> defaultValue = null)
        {
            TDRemoteConfig td = GetData(type);
            if (td != null)
            {
                return Helper.String2ListInt(td.value, "#");
            }

            return defaultValue;
        }

        public static string GetOfficialBundleID()
        {
#if UNITY_IOS
            return QueryString(OFFICIAL_PACKAGE_IOS, SDKConfig.S.bundleID);
#endif
            return QueryString(OFFICIAL_PACKAGE_ANDROID, SDKConfig.S.bundleID);
        }
    }
}