using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Runtime.InteropServices;
using Firebase.RemoteConfig;

namespace Qarth
{
    public class FirebaseDataWrap
    {
        private string m_Key;
        private object m_DefaultValue;

        public string key
        {
            get { return m_Key; }
        }

        public object defaultValue
        {
            get { return m_DefaultValue; }
        }

        public long defaultValueLong
        {
            get
            {
                if (m_DefaultValue == null)
                {
                    return 0;
                }

                return (long)m_DefaultValue;
            }
        }

        public bool defaultValueBool
        {
            get
            {
                if (m_DefaultValue == null)
                {
                    return false;
                }

                return (bool)m_DefaultValue;
            }
        }

        public string defaultValueString
        {
            get
            {
                if (m_DefaultValue == null)
                {
                    return "";
                }

                return (string)m_DefaultValue;
            }
        }

        public double defaultValueDouble
        {
            get
            {
                if (m_DefaultValue == null)
                {
                    return 0;
                }

                return (double)m_DefaultValue;
            }
        }

        public FirebaseDataWrap(string key, object defaultValue)
        {
            m_Key = key;
            m_DefaultValue = defaultValue;
            FirebaseRemoteConfigMgr.S.AddDefaultsValue(this);
            
        }
    }
}
