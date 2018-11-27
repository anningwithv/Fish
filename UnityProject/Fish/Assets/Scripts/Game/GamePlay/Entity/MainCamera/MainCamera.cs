using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class MainCamera : EventObj, IGameStateObserver
    {
        public static MainCamera S = null;

        [SerializeField]
        private Vector3 m_Offset = Vector3.zero;
        [SerializeField]
        private float m_Smooth = 5f;

        private Camera m_Camera = null;

        private Transform m_Target = null;
        //private float m_MinPosY = 0;
        private Vector3 m_InitPos;

        public float orthographicSize
        {
            get
            {
                return m_Camera.orthographicSize;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            S = this;
        }

        public void Init()
        {
            GameStateMgr.S.AddObserver(this);

            InitParam();

            //AdjustSize();
        }

        private void InitParam()
        {
            if (m_Camera == null)
            {
                m_Camera = GetComponent<Camera>();
            }

            //m_MinPosY = transform.position.y;
            m_InitPos = transform.position;
        }

        private void Update()
        {
            SmoothFollow();
        }

        private void SmoothFollow()
        {
            if (m_Target == null)
                return;

            Vector3 playerPos = m_Target.position;

            Vector3 lerpTargetPos = m_Offset + new Vector3(playerPos.x, playerPos.y, transform.position.z);
            //if (lerpTargetPos.y < m_MinPosY)
            //{
            //    lerpTargetPos.y = m_MinPosY;
            //}

            transform.position = Vector3.Lerp(transform.position, lerpTargetPos, m_Smooth * Time.deltaTime);
        }

        private void AdjustSize()
        {
            float aspectRatio = Screen.width * 1.0f / Screen.height;
            float height = Define.WORLD_WIDTH / aspectRatio * 0.5f;

            m_Camera.orthographicSize = height;
        }

        protected override void SetInterestEvent()
        {
            m_InteresetEvents = new int[] { (int)EventID.OnPlayerSpawned};
        }

        public override void HandleEvent(int eventId, params object[] param)
        {
            if (eventId == (int)EventID.OnPlayerSpawned)
            {
                m_Target = (Transform)param[0];
            }
        }

        #region IGameStateObserver
        public void OnGameStart()
        {
        }

        public void OnGamePlaying()
        {
        }

        public void OnGamePaused()
        {
        }

        public void OnGameResumed()
        {
        }

        public void OnGameOver()
        {
        }

        public void OnGameRestarted()
        {
            transform.position = m_InitPos;
            //m_MinPosY = transform.position.y;
        }
        #endregion
    }
}