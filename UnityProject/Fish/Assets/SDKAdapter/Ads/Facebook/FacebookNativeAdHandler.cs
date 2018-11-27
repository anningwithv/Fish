using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using AudienceNetwork;
using AudienceNetwork.Utility;
using UnityEngine.UI;

namespace Qarth
{
    public class FacebookNativeAdHandler : NativeAdHandler
    {

        private NativeAd m_NativeAd;

        public override string adTitle
        {
            get
            {
                if (m_NativeAd == null)
                {
                    return "";
                }

                return m_NativeAd.Title;
            }
        }

        public override string socialContext
        {
            get
            {
                if (m_NativeAd == null)
                {
                    return "";
                }

                return m_NativeAd.SocialContext;
            }
        }

        public override string callToAction
        {
            get
            {
                if (m_NativeAd == null)
                {
                    return "";
                }

                return m_NativeAd.CallToAction;
            }
        }

        public override Sprite iconSprite
        {
            get
            {
                if (m_NativeAd == null)
                {
                    return null;
                }

                return m_NativeAd.IconImage;
            }
        }

        public override Sprite coverImage
        {
            get
            {
                if (m_NativeAd == null)
                {
                    return null;
                }

                return m_NativeAd.CoverImage;
            }
        }

        protected IEnumerator LoadIconImage()
        {
            yield return m_NativeAd.LoadIconImage(m_NativeAd.IconImageURL);

            OnIconLoadFinish();
        }

        protected IEnumerator LoadCoverImage()
        {
            yield return m_NativeAd.LoadCoverImage(m_NativeAd.CoverImageURL);

            OnCoverImageLoadFinish();
        }

        protected override void LoadAdInner()
        {
#if UNITY_EDITOR
            return;
#endif

            TDAdConfig data = TDAdConfigTable.GetData(m_AdName);
            if (data == null)
            {
                return;
            }

            m_AdUnitID = data.unitID;

            m_NativeAd = new AudienceNetwork.NativeAd(m_AdUnitID);

            m_NativeAd.RegisterGameObjectForImpression(gameObject, null);

            m_NativeAd.NativeAdDidLoad = (delegate ()
            {
                if (On_AdTextLoadFinish != null)
                {
                    OnTextLoadFinish();
                }

                StartCoroutine(LoadIconImage());
                StartCoroutine(LoadCoverImage());
            });

            m_NativeAd.NativeAdDidFailWithError = (delegate (string error)
            {
                Log.w("Native ad failed to load with error: ");
            });

            m_NativeAd.NativeAdWillLogImpression = (delegate ()
            {
                Log.i("Native ad logged impression.");
            });

            m_NativeAd.NativeAdDidClick = (delegate ()
            {
                Log.i("Native ad clicked.");
            });

            m_NativeAd.LoadAd();
            return;
        }

        public override void RegisterGameObjectForImpression(GameObject gb, Button[] buttons)
        {
            if (m_NativeAd == null)
            {
                return;
            }

            m_NativeAd.RegisterGameObjectForImpression(gb, buttons);
        }

        public override void UnRegisterGameObjectForImpression()
        {
            if (m_NativeAd == null)
            {
                return;
            }

            m_NativeAd.RegisterGameObjectForImpression(gameObject, null);
        }

        protected override void CleanCurrentAD()
        {
            if (m_NativeAd == null)
            {
                return;
            }

            m_NativeAd.NativeAdDidLoad = null;
            m_NativeAd.NativeAdDidFailWithError = null;
            m_NativeAd.NativeAdWillLogImpression = null;
            m_NativeAd.NativeAdDidClick = null;

            m_NativeAd.Dispose();
            m_NativeAd = null;
        }
    }
}
