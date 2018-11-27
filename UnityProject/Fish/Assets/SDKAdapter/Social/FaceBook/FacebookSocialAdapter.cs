using System;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

namespace Qarth
{
    public enum SocialState
    {
        UnInit = 0,
        Error = 1,
        Init = 1 << 2,
        Logining = 1 << 3,
        Logined = 1 << 4,
        AskPublishPermissioning = 1 << 5,
        PublishPermission = 1 << 6,
    }

    public partial class FacebookSocialAdapter : TSingleton<FacebookSocialAdapter>
    {
        private static readonly List<string> readPermissions = new List<string> { "public_profile", "user_friends" };
        private static readonly List<string> publishPermissions = new List<string> { "publish_actions" };
        
        private FBRefreshSelfScoreCommand m_RefreshSelfScoreCommand = new FBRefreshSelfScoreCommand();
        private FBRefreshRankScoreCommand m_RefreshRankScoreCommand = new FBRefreshRankScoreCommand();
        private FBRefreshFriendsCommand m_RefreshFriendsCommand = new FBRefreshFriendsCommand();
        private FBLoginCommand m_LoginCommand = new FBLoginCommand();

        private DataRefreshTimeRecorder m_RankDataTimeRecorder = new DataRefreshTimeRecorder(5 * 60);
        private DataRefreshTimeRecorder m_FriendsDataTimeRecorder = new DataRefreshTimeRecorder(20 * 60);

        private FacebookUserInfoMgr m_Mgr = new FacebookUserInfoMgr();

        private SocialState m_SocialState = SocialState.UnInit;

        public bool isRankDataNeedRefresh
        {
            get { return isPublishLoggedIn && m_RankDataTimeRecorder.needRefresh; }
        }

        public bool isFriendsDataNeedRefresh
        {
            get { return isPublishLoggedIn && m_FriendsDataTimeRecorder.needRefresh; }
        }

        public FacebookUserInfoMgr fbUserInfoMgr
        {
            get { return m_Mgr; }
        }

        public List<FacebookUserInfo> allUserInfoList
        {
            get
            {
                return m_Mgr.allUserInfoList;
            }
        }

        //参与游戏的玩家
        public List<FacebookUserInfo> allGamerUserInfoList
        {
            get
            {
                return m_Mgr.gamerUserInfoList;
            }
        }

        public FacebookUserInfo selfUserInfo
        {
            get { return m_Mgr.GetSelfUserBase(); }
        }

        public string selfUserID
        {
            get { return m_Mgr.userID; }
        }

        public bool isPublishLoggedIn
        {
            get
            {
                return m_SocialState == SocialState.PublishPermission;
            }
        }

        public bool isLoggedIn
        {
            get
            {
                return m_SocialState >= SocialState.Logined;
            }
        }

        public bool havePublishActions
        {
            get
            {
                return (FB.IsLoggedIn &&
                       (AccessToken.CurrentAccessToken.Permissions as List<string>).Contains("publish_actions")) ? true : false;
            }
        }

        public void Init()
        {
            InitFBSDK();

            EventSystem.S.Register(EngineEventID.OnApplicationPauseChange, OnApplicationPauseChange);
            ResFactory.RegisterResCreate(FBPhotoRes.PREFIX_KEY, FBPhotoRes.Allocate);
        }

        private void InitFBSDK()
        {
            if (!FB.IsInitialized)
            {
                Log.i("Init[Facebook Socail adapter]");
                FB.Init(OnInitComplateDelegate, null);
            }
            else
            {
                OnInitComplateDelegate();
            }
        }

        private void OnApplicationPauseChange(int key, params object[] args)
        {
            bool pauseStatus = (bool)args[0];
            if (!pauseStatus)
            {
                InitFBSDK();
            }
        }

        public void PromptForLogin()
        {
            if (m_SocialState >= SocialState.Logined)
            {
                return;
            }

            m_LoginCommand.Execute(false);
        }

        public void PromptForPublishLogin()
        {
            if (m_SocialState >= SocialState.PublishPermission)
            {
                return;
            }

            m_LoginCommand.Execute(true);
        }

        public void RefreshSelfPlayerInfo()
        {
            RefreshPlayerInfo(selfUserID);
        }

        public void RefreshPlayerInfo(string userID)
        {
            if (!isPublishLoggedIn)
            {
                return;
            }

            string queryString = string.Format("/{0}?fields=first_name,last_name", userID);
            FB.API(queryString, HttpMethod.GET, result =>
            {
                if (!string.IsNullOrEmpty(result.Error))
                {
                    EventSystem.S.Send(SDKEventID.OnFBRefreshPlayerInfoEvent, userID, false);
                    return;
                }

                var data = m_Mgr.GetUserBase(userID, true);
                var dic = result.ResultDictionary;
                data.userName = GetStringSafety(dic, "first_name") + GetStringSafety(dic, "last_name");
                //data.pictureUrl = GetStringSafety(dic, "picture");
                EventSystem.S.Send(SDKEventID.OnFBRefreshPlayerInfoEvent, userID, true);
            });
        }

        public void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, Action<bool> callback = null)
        {
            if (!FB.IsInitialized)
            {
                if (callback != null)
                {
                    callback(false);
                    return;
                }
            }

            FB.ShareLink(contentURL,
                contentTitle,
                contentDescription,
                photoURL,
                delegate (IShareResult result)
                {
                    if (string.IsNullOrEmpty(result.Error))
                    {
                        if (callback != null)
                        {
                            callback(true);
                        }
                    }
                    else
                    {
                        Log.e("Share Context 2 FB Error:" + result.Error);
                        if (callback != null)
                        {
                            callback(false);
                        }
                    }
                });
        }

        public void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, Action<bool> callback = null)
        {
            if (!FB.IsInitialized)
            {
                if (callback != null)
                {
                    callback(false);
                    return;
                }
            }

            FB.FeedShare(toId, link, linkName, linkCaption, linkDescription, picture, mediaSource, (result) =>
            {
                if (string.IsNullOrEmpty(result.Error))
                {
                    if (callback != null)
                    {
                        callback(true);
                    }
                }
                else
                {
                    Log.e("FeedShare Context 2 FB Error:" + result.Error);
                    if (callback != null)
                    {
                        callback(false);
                    }
                }
            });
        }

        public void RequestFriends(string message,string title,string data, Action<bool>callBack)
        {
            if (!FB.IsInitialized)
            {
                if (callBack != null)
                {
                    callBack(false);
                    return;
                }
            }

            List<string> recipient = null;
            
            FB.AppRequest(message, recipient, null, null,null,data, title, result => 
            {
                if (string.IsNullOrEmpty(result.Error))
                {
                    if (callBack != null)
                    {
                        callBack(true);
                    }
                }
                else
                {
                    Log.e("RequestFriends2 FB Error:" + result.Error);
                    if (callBack != null)
                    {
                        callBack(false);
                    }
                }
            });
        }

        public void RequestFriends(string message, Action<bool> callBack)
        {
            if (!FB.IsInitialized)
            {
                if (callBack != null)
                {                    
                    callBack(false);
                    return;
                }
            }

            FB.AppRequest(message,null,null,null,null,null,null ,result=> 
            {
                if (string.IsNullOrEmpty(result.Error))
                {
                    if (callBack != null)
                    {
                        callBack(true);
                    }
                }
                else
                {
                    Log.e("RequestFriends2 FB Error:" + result.Error);
                    if (callBack != null)
                    {
                        callBack(false);
                    }
                }
            });
        }

        public void InviteFriends(Action<bool> callBack)
        {
            if (!FB.IsInitialized)
            {
                if (callBack != null)
                {
                    callBack(false);
                }               
                return;
            }

            string appLink = "";
            string previewImage = "";

#if UNITY_IOS
            appLink = SDKConfig.S.socialConfig.fbConfig.appLinkiOS;
            previewImage = SDKConfig.S.socialConfig.fbConfig.previewImageiOS;
#elif UNITY_ANDROID
            appLink = SDKConfig.S.socialConfig.fbConfig.appLinkAndroid;
            previewImage = SDKConfig.S.socialConfig.fbConfig.previewImageAndroid;
#endif

            if (string.IsNullOrEmpty(appLink))
            {
                Log.e("Invalid FB Config!");

                if (callBack != null)
                {
                    callBack(false);
                }
                return;
            }
            

            Uri appLinkUri = new Uri(appLink);
            Uri imageUri = null;

            if (!string.IsNullOrEmpty(previewImage))
            {
                imageUri = new Uri(previewImage);
            }

            FB.Mobile.AppInvite(
                appLinkUri,
                imageUri, (result) =>
                {
                    if (string.IsNullOrEmpty(result.Error))
                    {
                        if (callBack != null)
                        {
                            callBack(true);
                        }
                    }
                    else
                    {
                        Log.e("InviteFriends:" + result.Error);
                        if (callBack != null)
                        {
                            callBack(false);
                        }
                    }
                });
        }

        public void RefreshFriends()
        {
            if (!isPublishLoggedIn)
            {
                return;
            }

            m_RefreshFriendsCommand.Execute();
        }

        public void RefreshRankScore(int limit = 30)
        {
            if (!isPublishLoggedIn)
            {
                return;
            }

            m_RefreshRankScoreCommand.Execute(limit);
        }

        public void RefreshSelfScore()
        {
            if (!isPublishLoggedIn)
            {
                return;
            }
            m_RefreshSelfScoreCommand.Execute();
        }

        public void PostScore(int score)
        {
            if (!isPublishLoggedIn)
            {
                return;
            }

            var dataBase = m_Mgr.GetSelfUserBase();

            if (dataBase != null)
            {
                if (score < dataBase.gameSocre)
                {
                    return;
                }

                m_Mgr.GetSelfUserBase().gameSocre = score;
                m_Mgr.SortGamerUserInfo();
            }

            if (havePublishActions)
            {
                var scoredata = new Dictionary<string, string>();
                scoredata["score"] = score.ToString();
                FB.API("/me/scores", HttpMethod.POST, OnPostScoreCallback, scoredata);
            }
            else
            {
                EventSystem.S.Send(SDKEventID.OnFBPostScoreEvent, false);
            }
        }

        public void LogOutSocialPlatform()
        {
            if (!FB.IsInitialized)
            {
                return;
            }

            if (m_SocialState < SocialState.Logined)
            {
                return;
            }

            m_SocialState = SocialState.Init;
            FB.LogOut();
            m_Mgr.ClearAllData();

            m_FriendsDataTimeRecorder.Reset();
            m_RankDataTimeRecorder.Reset();

            EventSystem.S.Send(SDKEventID.OnFBLogoutEvent);
        }

        private void OnInitComplateDelegate()
        {
            if (FB.IsInitialized)
            {
                m_SocialState = SocialState.Init;
                FB.ActivateApp();
            }
            else
            {
                m_SocialState = SocialState.Error;
                Log.e("FaceBook SDK Init Failed");
            }

            if (FB.IsLoggedIn)
            {
                m_SocialState = SocialState.Logined;
            }
        }

        private void OnRefreshFriendsCallback(IGraphResult result)
        {
            m_FriendsDataTimeRecorder.RecordRefreshTime();

            var dict = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendlist = (List<object>)dict["data"];

            for (int i = friendlist.Count - 1; i >= 0; --i)
            {
                Dictionary<string, object> data = friendlist[i] as Dictionary<string, object>;

                string id = GetStringSafety(data,"id");
                if (string.IsNullOrEmpty(id))
                {
                    continue;
                }
                string userName = GetStringSafety(data, "first_name") + GetStringSafety(data, "last_name");
                //string imageUri = GetStringSafety(data, "picture");
                var dataBase = m_Mgr.GetUserBase(id, true);
                dataBase.userName = userName;
            }
        }

        public static string GetStringSafety(IDictionary<string, object> dic, string key)
        {
            string result = "";
            dic.TryGetValue(key, out result);
            return result;
        }

        private void OnPostScoreCallback(IGraphResult result)
        {
            if (!string.IsNullOrEmpty(result.Error))
            {
                Log.e("The Post Score Error" + result.Error);
                EventSystem.S.Send(SDKEventID.OnFBPostScoreEvent, false);
                return;
            }

            EventSystem.S.Send(SDKEventID.OnFBPostScoreEvent, true);
        }

        private void OnRefreshScoreCallBack(IResult result)
        {
            IDictionary<string, object> data = result.ResultDictionary;

            if (!data.ContainsKey("data"))
            {
                return;
            }

            List<object> dataList = (List<object>)data["data"];
            m_RankDataTimeRecorder.RecordRefreshTime();

            for (int i = dataList.Count - 1; i >= 0; --i)
            {
                var entry = (Dictionary<string, object>)dataList[i];
                var user = (Dictionary<string, object>)entry["user"];

                string userID = GetStringSafety(user, "id");
                if (string.IsNullOrEmpty(userID))
                {
                    continue;
                }

                FacebookUserInfo dataBase = m_Mgr.GetUserBase(userID, true);
                dataBase.gameSocre = Convert.ToInt32(entry["score"]);
                dataBase.userName = GetStringSafety(user, "name");
            }
            m_Mgr.RebuildGamerUserInfoList();
        }

        private void OnLoginWithReadPermissions()
        {
            m_SocialState = SocialState.Logined;
            RefreshPlayerInfo(selfUserID);
        }

        private void OnLoginWithPublishPermission()
        {
            m_SocialState = SocialState.PublishPermission;
            RefreshPlayerInfo(selfUserID);
        }
    }
}

