using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Qarth;

namespace GameWish.Game
{
    public class WaitForStartPanel : AbstractPanel
    {
        [SerializeField]
        private Button m_GameStartBtn = null;

        protected override void OnPanelOpen(params object[] args)
        {

        }

        protected override void OnUIInit()
        {
            Init();
        }

        private void Init()
        {
            m_GameStartBtn.onClick.AddListener(()=> 
            {
                GameStateMgr.S.StartGame();

                CloseSelfPanel();
            });
        }
    }
}
