using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Qarth
{
    public class UnityRewardVideoHandler :  AdFullScreenHandler
    {
        private ShowOptions m_Options;
        public override bool isAdReady
        {
            get
            {
                return Advertisement.IsReady(m_Config.unitID);
            }
        }

        public override bool ShowAd()
        {
            if (!isAdReady)
            {
                return false;
            }

            if (DoShowAd())
            {
                m_AdState = AdState.Showing;
            }

            return m_AdState == AdState.Showing;
        }

        protected override bool DoPreLoadAd()
        {
            return false;
        }

        private void AdShowEvent(ShowResult result)
        {
            if (result == ShowResult.Finished)
            {
                HandleOnAdRewarded();
            }

            HandleOnAdClosed();
        }

        protected override bool DoShowAd()
        {
            if (m_Options == null)
            {
                m_Options = new ShowOptions();
                m_Options.resultCallback = AdShowEvent;
            }

            Advertisement.Show(m_Config.unitID, m_Options);
            return true;
        }

        public override string ToString()
        {            
            return "#UnityRewardedVideoHandler:" + m_Config.id;
        }
    }

    
}