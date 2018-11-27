using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
	public class PatternController : EventObj,IGameStateObserver
	{

        //[SerializeField]
        //private Transform m_StartTransform; // 默认第一个Pattern的底边
        [SerializeField]
        private Vector3 m_StartPosition = Vector3.zero;
		//[SerializeField]
		private Transform m_PatternRoot;
		private int m_StartPatternIndex = 1;

		private Pattern m_CurrentTopPattern;
		private Pattern m_CurrentStartPattern;
		
		private List<Pattern> m_ActivePatternList = new List<Pattern>();

		public int startPatternIndex
		{
			get
			{
				return m_StartPatternIndex;
			}
			set
			{
				startPatternIndex = value;
			}
		}

        private void Start()
		{
            m_PatternRoot = transform;

            //
            LoadPatternByIndex(startPatternIndex);
			LoadPatternByIndex(RandomHelper.Range(1, Define.BLOCK_GROUP_COUNT + 1));
		}
	
		private void Update()
		{
			//判断
			//if(levelControl == null)
			//{
			//	return;
			//}

			if(m_CurrentTopPattern == null && !GameStateMgr.S.IsGameRunning)
			{
				return;
			}

			Vector3 cameraPos = MainCamera.S.transform.position;
			float dis =  MainCamera.S.orthographicSize;
			if( Mathf.Abs(cameraPos.y - m_CurrentTopPattern.transform.position.y - m_CurrentTopPattern.gridY / 2) <= dis)
			{
				LoadPatternByIndex(RandomHelper.Range(1, Define.BLOCK_GROUP_COUNT + 1));
			}

			if( (cameraPos.y - dis) - m_ActivePatternList[1].transform.position.y  >= m_ActivePatternList[1].gridY /2)	
			{
				 RecyleBottomPattern();
			}		
		}

		private  void LoadPatternByIndex(int index)
		{
			string poolName = string.Format("Pattern_{0}", index);
			Pattern go = GameObjectPoolMgr.S.Allocate(poolName).GetComponent<Pattern>();

			if(go == null)
			{
				Log.e("error: load prefab without pattern scripts");
				return;
			}
			go.transform.SetParent(m_PatternRoot);
			if(m_CurrentTopPattern != null)
			{
				go.transform.position = m_CurrentTopPattern.transform.position + Vector3.up * 0.5f *( go.gridY + m_CurrentTopPattern.gridY);	
				//m_CurrentTopPattern = go;
			}
			else
			{
				go.transform.position = m_StartPosition + Vector3.up * 0.5f * go.gridY;
				m_CurrentStartPattern = go;	
										
				
			}
			m_CurrentTopPattern = go;
			m_ActivePatternList.Add(go);
			
		}

		private void RecyleBottomPattern()
		{
			GameObjectPoolMgr.S.Recycle(m_ActivePatternList[1].gameObject);
			m_ActivePatternList.RemoveAt(1);
		}

        protected override void SetInterestEvent()
        {
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
        }
        #endregion
    }
}