using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public class NativeAdPanel : AbstractPanel
    {
        [SerializeField]
        private NativeAdView m_NativeAdView;

        protected override void OnPanelOpen(params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                CloseSelfPanel();
                return;
            }

            string adName = args[0] as string;

            bool load = true;

            if (args.Length > 1)
            {
                load = (bool)args[1];
            }

            m_NativeAdView.ShowAd(adName, load);
        }

        public override BackKeyCodeResult OnBackKeyDown()
        {
            return BackKeyCodeResult.BLOCK;
        }
    }
}
