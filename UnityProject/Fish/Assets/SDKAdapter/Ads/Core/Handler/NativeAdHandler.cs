using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class NativeAdHandler : MonoBehaviour, INativeAdHandler
    {
        protected string m_AdName;
        protected string m_AdUnitID;

        public Action<INativeAdHandler> On_AdTextLoadFinish;
        public Action<INativeAdHandler> On_AdIconLoadFinish;
        public Action<INativeAdHandler> On_AdCoverImageLoadFinsh;

        protected AdState m_AdState = AdState.NONE;
        protected int m_RefCount;

        public string adName
        {
            get { return m_AdName; }
        }

        public virtual string adTitle
        {
            get
            {
                return "";
            }
        }

        public virtual string socialContext
        {
            get
            {
                return "";
            }
        }

        public virtual string callToAction
        {
            get
            {
                return "";
            }
        }

        public virtual Sprite iconSprite
        {
            get { return null; }
        }

        public virtual Sprite coverImage
        {
            get { return null; }
        }

        public AdState adState
        {
            get
            {
                return m_AdState;
            }
        }

        public void SetAdName(string name)
        {
            m_AdName = name;
        }

        public void LoadAd()
        {
            CleanCurrentAD();
            m_AdState = AdState.Loading;
            LoadAdInner();
        }

        protected void OnDestroy()
        {
            CleanCurrentAD();
        }

        protected virtual void CleanCurrentAD()
        {

        }

        protected virtual void LoadAdInner()
        {

        }

        public void Bind(Action<INativeAdHandler> textLoadL, Action<INativeAdHandler> iconLoadL, Action<INativeAdHandler> coverImageL)
        {
            ++m_RefCount;
            On_AdTextLoadFinish += textLoadL;
            On_AdIconLoadFinish += iconLoadL;
            On_AdCoverImageLoadFinsh += coverImageL;
        }

        public void UnBind(Action<INativeAdHandler> textLoadL, Action<INativeAdHandler> iconLoadL, Action<INativeAdHandler> coverImageL)
        {
            --m_RefCount;
            On_AdTextLoadFinish -= textLoadL;
            On_AdIconLoadFinish -= iconLoadL;
            On_AdCoverImageLoadFinsh -= coverImageL;
        }

        protected void OnTextLoadFinish()
        {
            m_AdState = AdState.Loaded;

            if (On_AdTextLoadFinish != null)
            {
                On_AdTextLoadFinish(this);
            }
        }

        protected void OnIconLoadFinish()
        {
            if (On_AdIconLoadFinish != null)
            {
                On_AdIconLoadFinish(this);
            }
        }

        protected void OnCoverImageLoadFinish()
        {
            if (On_AdCoverImageLoadFinsh != null)
            {
                On_AdCoverImageLoadFinsh(this);
            }
        }

        public virtual void RegisterGameObjectForImpression(GameObject gb, Button[] buttons)
        {
            
        }

        public virtual void UnRegisterGameObjectForImpression()
        {
            
        }
    }
}
