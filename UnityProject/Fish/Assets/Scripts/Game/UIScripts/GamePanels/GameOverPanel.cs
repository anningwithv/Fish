using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Qarth;

namespace GameWish.Game
{
    public class GameOverPanel : AbstractPanel
    {
        [SerializeField]
        private Text m_Title = null;
        [SerializeField]
        private Button m_RestartStartBtn = null;

        protected override void OnPanelOpen(params object[] args)
        {

        }

        protected override void OnUIInit()
        {
            Init();
        }

        private void Init()
        {
            m_RestartStartBtn.onClick.AddListener(()=> 
            {
                GameStateMgr.S.RestartGame();

                CloseSelfPanel();
            });
        }
    }
}
