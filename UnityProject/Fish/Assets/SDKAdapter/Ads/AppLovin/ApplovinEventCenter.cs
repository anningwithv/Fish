using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    [TMonoSingletonAttribute("ApplovinEventCenter")]
    public class ApplovinEventCenter : TMonoSingleton<ApplovinEventCenter>
    {
        public Action on_InterLoaded;
        public Action on_InterLoadFailed;
        public Action on_InterClose;
        public Action on_InterShow;

        public Action on_RewardLoaded;
        public Action on_RewardLoadFailed;
        public Action on_RewardClose;
        public Action on_RewardReward;
        public Action on_RewardShow;

        public void Init()
        {
            Log.i("Init ApplovinEventCenter");
        }

        protected void onAppLovinEventReceived(string msg)
        {
            switch (msg)
            {
                case "HIDDENINTER"://插屏关闭
                    OnInterCloseEvent();
                    break;
                case "LOADEDINTER"://插屏加载成功
                    OnInterLoadedEvent();
                    break;
                case "LOADINTERFAILED"://插屏加载失败
                    OnInterLoadFailedEvent();
                    break;

                case "HIDDENREWARDED"://激励视屏关闭
                    OnRewardCloseEvent();
                    break;
                case "LOADEDREWARDED"://激励视屏加载成功
                    OnRewardLoadedEvent();
                    break;
                case "LOADREWARDEDFAILED"://激励视屏加载失败
                    OnRewardLoadFailedEvent();
                    break;
                case "REWARDAPPROVED"://激励视屏验证成功
                    OnRewardRewardEvent();
                    break;
                case "REWARDTIMEOUT"://激励视屏验证超时
                    break;

                case "HIDDENBANNER"://Banner 隐藏
                    
                    break;
                case "LOADEDBANNER"://Banner加载成功
                    
                    break;
                case "LOADBANNERFAILED":
                    break;
                case "DISPLAYEDBANNER":
                    break;
            }
        }

        protected void OnInterLoadedEvent()
        {
            if (on_InterLoaded != null)
            {
                on_InterLoaded();
            }
        }

        protected void OnInterLoadFailedEvent()
        {
            if (on_InterLoadFailed != null)
            {
                on_InterLoadFailed();
            }
        }

        protected void OnInterCloseEvent()
        {
            if (on_InterClose != null)
            {
                on_InterClose();
            }
        }

        protected void OnRewardLoadedEvent()
        {
            if (on_RewardLoaded != null)
            {
                on_RewardLoaded();
            }
        }

        protected void OnRewardLoadFailedEvent()
        {
            if (on_RewardLoadFailed != null)
            {
                on_RewardLoadFailed();
            }
        }

        protected void OnRewardCloseEvent()
        {
            if (on_RewardClose != null)
            {
                on_RewardClose();
            }
        }

        protected void OnRewardRewardEvent()
        {
            if (on_RewardReward != null)
            {
                on_RewardReward();
            }
        }
    }
}
