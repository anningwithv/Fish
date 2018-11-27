using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    [RequireComponent(typeof(RectTransform))]
    public class AdBannerView : MonoBehaviour, IAdInterfaceEventListener
    {
        [SerializeField]
        private String m_AdPlacementName;
        [SerializeField]
        private bool m_IsDynamicPosition = false;
        [SerializeField]
        private AdSize m_AdSize = AdSize.MediumRectangle;
        [SerializeField]
        private bool m_AutoShowOnEnable = true;
        [SerializeField]
        private Transform m_CenterRoot;

        private bool m_IsShowingAd = false;

        private AdInterface m_AdInterface;

        public string adPlacementID
        {
            get { return m_AdPlacementName; }
        }

        public AdSize adSize
        {
            get { return m_AdSize; }
            set { m_AdSize = value; }
        }

        public void ShowAd(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = m_AdPlacementName;
            }

            if (name.Equals(m_AdPlacementName))
            {
                InnerShowAd();
                return;
            }

            HideAd();

            BindAdInterface(name);

            if (m_AdInterface == null)
            {
                return;
            }

            InnerShowAd();
        }

        public void BindAdInterface(string placementName)
        {
            if (m_AdPlacementName == placementName)
            {
                return;
            }

            if (m_AdInterface != null)
            {
                m_AdInterface.adEventListener = null;
                m_AdInterface = null;
            }

            m_AdPlacementName = placementName;
            TDAdPlacement data = TDAdPlacementTable.GetData(m_AdPlacementName);

            if (data == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(data.adInterface0))
            {
                m_AdInterface = AdsMgr.S.GetAdInterface(data.adInterface0);
                if (m_AdInterface != null)
                {
                    m_AdInterface.adEventListener = this;
                }
            }
        }

        private void InnerShowAd()
        {
            if (m_IsShowingAd || m_AdInterface == null)
            {
                return;
            }

            m_IsShowingAd = true;
            Vector2 dpPosition = GetBannerDPPosition();
            AdsMgr.S.ShowBannerAd(m_AdInterface.adInterfaceName, m_AdSize, AdPosition.CustomDefine, (int)dpPosition.x, (int)dpPosition.y);

            SyncViewSize();
        }

        private void SyncViewSize()
        {
            Vector2 pixelSize = new Vector2(DP2Pixel(m_AdSize.width / 2), DP2Pixel(m_AdSize.height / 2));

            Vector3 centerScreen = Position2ScreenPosition(transform.position);

            Vector3 topLeft_Screen = new Vector3(centerScreen.x - pixelSize.x, centerScreen.y + pixelSize.y, 0);

            Vector3 topLeftWorld = ScreenPosition2Position(topLeft_Screen);
            Vector3 topLeftLocal = transform.InverseTransformPoint(topLeftWorld);
            topLeftLocal.z = 0;


            Vector2 sizeDelta = new Vector2(Mathf.Abs(topLeftLocal.x) * 2, Mathf.Abs(topLeftLocal.y) * 2);
            GetComponent<RectTransform>().sizeDelta = sizeDelta;
        }

        private void SyncAdPosition()
        {
            if (!m_IsDynamicPosition)
            {
                return;
            }

            if (!m_IsShowingAd || m_AdInterface == null)
            {
                return;
            }

            Vector2 dpPosition = GetBannerDPPosition();
            AdsMgr.S.SetBannerPosition(m_AdInterface.adInterfaceName, AdPosition.CustomDefine, (int)dpPosition.x, (int)dpPosition.y);
        }

        private void Update()
        {
            SyncAdPosition();
        }

        private Vector2 GetBannerDPPosition()
        {
            Vector3 centerPos = transform.position;
            if (m_CenterRoot != null)
            {
                centerPos = m_CenterRoot.position;
            }
            Vector3 centerScreen = Position2ScreenPosition(centerPos);
            centerScreen.y = Screen.currentResolution.height - centerScreen.y;
            return new Vector3(Pixel2DP(centerScreen.x) - m_AdSize.width * 0.5f,
                Pixel2DP(centerScreen.y) - m_AdSize.height * 0.5f, 0);
        }

        public void HideAd()
        {
            if (m_AdInterface == null)
            {
                return;
            }

            m_AdInterface.HideAd();
            m_IsShowingAd = false;
        }

        private void OnEnable()
        {
            if (m_AutoShowOnEnable)
            {
                ShowAd();
            }
        }

        private void OnDisable()
        {
            HideAd();
        }

        private void OnDestroy()
        {
            HideAd();
        }

        public static float Pixel2DP(float pixel)
        {
            return pixel * 160 / Screen.dpi;
        }

        public static float DP2Pixel(float dp)
        {
            return dp * (Screen.dpi / 160);
        }

        public static Vector3 Position2ScreenPosition(Vector3 position)
        {
            return UIMgr.S.uiRoot.uiCamera.WorldToScreenPoint(position);
        }

        public static Vector3 ScreenPosition2Position(Vector3 position)
        {
            return UIMgr.S.uiRoot.uiCamera.ScreenToWorldPoint(position);
        }

        public static void DeleteZPosition(Transform target)
        {
            Vector3 localPos = target.localPosition;
            localPos.z = 0;
            target.localPosition = localPos;
        }

        public void OnAdLoadEvent()
        {
            if (!m_IsShowingAd)
            {
                return;
            }

            SyncViewSize();
        }


        public void OnAdLoadFailedEvent()
        {
            
        }

        public void OnAdRewardEvent()
        {
            
        }

        public void OnAdCloseEvent()
        {
            
        }
    }
}
