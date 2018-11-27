using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;
using UnityEngine.SceneManagement;

namespace GameWish.Game
{
    public partial class GameplayMgr : TMonoSingleton<GameplayMgr>
    {
        [SerializeField]
        private Transform m_EntityRoot = null;

        public void InitGameplay()
        {
            // Init Tables

            // Init Managers
            InputMgr.S.Init();
            GameDataMgr.S.Init();
            GameStateMgr.S.Init();
            GameInfoMgr.S.Init();
            OfflineRewardMgr.S.Init();
            UIMsgHandler.S.Init();
            //EffectMgr.S.Init();

            UILoader.S.Init();
            WorldLoader.S.Init(m_EntityRoot);
            MainCamera.S.Init();

            //Set language
            I18Mgr.S.SwitchLanguage(SystemLanguage.English);

            UIMgr.S.OpenPanel(UIID.MainGamePanel);

            UIMgr.S.OpenPanel(UIID.WaitForStartPanel);
            //StartGameplay();
        }

    }
}