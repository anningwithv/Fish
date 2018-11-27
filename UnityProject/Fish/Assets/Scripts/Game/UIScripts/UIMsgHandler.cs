using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class UIMsgHandler : TMonoSingleton<UIMsgHandler>, IGameStateObserver
    {
        public void Init()
        {
            GameStateMgr.S.AddObserver(this);
        }

        public void OnGameOver()
        {
            UIMgr.S.OpenPanel(UIID.GameOverPanel);
        }

        public void OnGamePaused()
        {
        }

        public void OnGamePlaying()
        {
        }

        public void OnGameRestarted()
        {
        }

        public void OnGameResumed()
        {
        }

        public void OnGameStart()
        {
        }

    }
}