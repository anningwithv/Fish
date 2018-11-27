using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class AdFullScreenHandler : AdHandler
    {
        public override bool ShowAd()
        {
            if (!isAdReady)
            {
                return false;
            }

            if (m_AdState != AdState.Loaded)
            {
                return false;
            }

            if (DoShowAd())
            {
                //Log.i("AD-ShowAD:" + m_Config.id);
                m_AdState = AdState.Showing;
            }

            return m_AdState == AdState.Showing;
        }

        public override bool PreLoadAd()
        {
            if (m_AdState != AdState.NONE)
            {
                return false;
            }

            if (string.IsNullOrEmpty(m_Config.unitID))
            {
                return false;
            }

            //Log.i("AD-PreLoadAd:" + m_Config.id);

            if (DoPreLoadAd())
            {
                m_AdState = AdState.Loading;
            }

            return m_AdState == AdState.Loading;
        }

        protected virtual bool DoPreLoadAd()
        {
            return false;
        }

        protected virtual bool DoShowAd()
        {
            return false;
        }
    }
}
