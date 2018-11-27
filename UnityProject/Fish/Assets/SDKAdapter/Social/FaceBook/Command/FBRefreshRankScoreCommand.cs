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
        public class FBRefreshRankScoreCommand : FBCommand
        {
        
            public void Execute(int limit)
            {
                if (!isExecuteAble)
                {
                    return;
                }

                OnExecuteBegin();
                RefreshRankScore(limit);
            }

            protected void RefreshRankScore(int limit)
            {
                FB.API(string.Format("/app/scores?fields=score,user.limit({0})", limit), HttpMethod.GET, (result) =>
                {
                    OnExecuteFinish();

                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        EventSystem.S.Send(SDKEventID.OnFBRefreshRankScoreEvent, false);
                    }
                    else
                    {

                        FacebookSocialAdapter.S.OnRefreshScoreCallBack(result);

                        EventSystem.S.Send(SDKEventID.OnFBRefreshRankScoreEvent, true);
                    }
                });
            }
        }
    }
}