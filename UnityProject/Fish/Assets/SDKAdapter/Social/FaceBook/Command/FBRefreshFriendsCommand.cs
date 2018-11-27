using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;
using Facebook.Unity;

namespace Qarth
{

    public partial class FacebookSocialAdapter
    {
        public class FBRefreshFriendsCommand : FBCommand
        {
        
            public void Execute()
            {
                if (!isExecuteAble)
                {
                    return;
                }

                OnExecuteBegin();
                RefreshFriends();
            }

            protected void RefreshFriends()
            {
                string queryString = "/me/friends?fields=first_name,last_name";
                FB.API(queryString, HttpMethod.GET, result =>
                {
                    OnExecuteFinish();

                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        EventSystem.S.Send(SDKEventID.OnFBRefreshFriendEvent, false);
                        return;
                    }
                    else
                    {
                        FacebookSocialAdapter.S.OnRefreshFriendsCallback(result);

                        EventSystem.S.Send(SDKEventID.OnFBRefreshFriendEvent, true);   
                    }
                });
            }

        }
    }
}