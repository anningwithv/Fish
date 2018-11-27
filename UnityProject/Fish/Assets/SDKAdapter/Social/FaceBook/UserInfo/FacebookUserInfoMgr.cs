using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Facebook.Unity;

namespace Qarth
{
    public class FacebookUserInfo : UserInfoBase
    {

    }

    public class DataRefreshTimeRecorder
    {
        protected float m_MinRefreshOffset;
        protected float m_LastRefreshTime = float.MinValue;

        public bool needRefresh
        {
            get
            {
                return (Time.realtimeSinceStartup - m_LastRefreshTime) > m_MinRefreshOffset;
            }
        }

        public DataRefreshTimeRecorder(float minRefreshOffset)
        {
            m_MinRefreshOffset = minRefreshOffset;
        }

        public void RecordRefreshTime()
        {
            m_LastRefreshTime = Time.realtimeSinceStartup;
        }

        public void Reset()
        {
            m_LastRefreshTime = float.MinValue;
        }
    }

    public class FacebookUserInfoMgr 
    {
        private Dictionary<string, FacebookUserInfo> m_UserInfoDic = new Dictionary<string, FacebookUserInfo>();
        private List<FacebookUserInfo> m_AllUserInfoList = new List<FacebookUserInfo>();
        private List<FacebookUserInfo> m_GamerUserInfoList = new List<FacebookUserInfo>();

        public string userID
        {
            get
            {
                if (AccessToken.CurrentAccessToken == null)
                {
                    return "";
                }

                return AccessToken.CurrentAccessToken.UserId;
            }
        }

        public List<FacebookUserInfo> allUserInfoList
        {
            get
            {
                return m_AllUserInfoList;
            }
        }

        public List<FacebookUserInfo> gamerUserInfoList
        {
            get
            {
                return m_GamerUserInfoList;
            }
        }

        public void ClearAllData()
        {
            m_UserInfoDic.Clear();
            m_AllUserInfoList.Clear();
        }

        public void RebuildGamerUserInfoList()
        {
            m_GamerUserInfoList.Clear();

            for (int i = m_AllUserInfoList.Count - 1; i >= 0; --i)
            {
                if (m_AllUserInfoList[i].gameSocre >= 0)
                {
                    m_GamerUserInfoList.Add(m_AllUserInfoList[i]);
                }
            }

            SortGamerUserInfo();
        }

        public UserInfoBase GetNextUserInfoBeyondScore(int score)
        {
            if (m_GamerUserInfoList == null || m_GamerUserInfoList.Count == 0)
            {
                return null;
            }

            for (int i = m_GamerUserInfoList.Count - 1; i >= 0; --i)
            {
                if (m_GamerUserInfoList[i].gameSocre > score)
                {
                    return m_GamerUserInfoList[i];
                }
            }

            return null;
        }

        public void SortGamerUserInfo()
        {
            m_GamerUserInfoList.Sort(SortComparor);

            for (int i = 0; i < m_GamerUserInfoList.Count; ++i)
            {
                m_GamerUserInfoList[i].rank = i + 1;
            }
        }

        public FacebookUserInfo GetSelfUserBase()
        {
            return GetUserBase(userID);
        }

        public FacebookUserInfo GetUserBase(string userID)
        {
            FacebookUserInfo data = null;
            if (m_UserInfoDic.TryGetValue(userID, out data))
            {
                return data;
            }

            return null;
        }

        public FacebookUserInfo GetUserBase(string userID, bool createForce)
        {
            FacebookUserInfo data = GetUserBase(userID);
            if (createForce && data == null)
            {
                data = new FacebookUserInfo();
                m_UserInfoDic.Add(userID, data);
                m_AllUserInfoList.Add(data);

                data.userID = userID;
            }

            return data;
        }

        private int SortComparor(FacebookUserInfo a, FacebookUserInfo b)
        {
            return b.gameSocre - a.gameSocre;
        }
    }
}
