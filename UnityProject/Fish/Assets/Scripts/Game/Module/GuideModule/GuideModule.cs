using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class GuideModule : AbstractModule
    {
        public void StartGuide()
        {
            if (!AppConfig.S.isGuideActive)
            {
                return;
            }

            GuideMgr.S.StartGuideTrack();
            InitCustomTrigger();
            InitCustomCommand();
        }

        protected override void OnComAwake()
        {

        }
        protected void InitCustomTrigger()
        {
            GuideMgr.S.RegisterGuideTrigger(typeof(UINodeInvisibleTrigger));
        }

        protected void InitCustomCommand()
        {
            //GuideMgr.S.RegisterGuideCommand(typeof(CharaWordsCommand));
            //GuideMgr.S.RegisterGuideCommand(typeof(EventCommand));
            //GuideMgr.S.RegisterGuideCommand(typeof(UIClipCommand));
            //GuideMgr.S.RegisterGuideCommand(typeof(SpineHandCommand));
            //GuideMgr.S.RegisterGuideCommand(typeof(SkipGuideCommand));
        }

    }
}
