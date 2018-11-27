using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public interface IGameStateObserver
    {
        //private LevelStateMgr m_LevelControl;

        //public LevelStateMgr levelControl
        //{
        //    get { return m_LevelControl; }
        //    set
        //    {
        //        m_LevelControl = value;
        //        OnLevelComponmentInit();
        //    }
        //}

        //public bool isLevelRunning
        //{
        //    get
        //    {
        //        if (m_LevelControl == null)
        //        {
        //            return false;
        //        }

        //        return levelControl.isLevelRunning;
        //    }
        //}

        //void OnLevelComponmentInit();

        //void OnGameInit();

        void OnGameStart();

        void OnGamePlaying();

        void OnGamePaused();

        void OnGameResumed();

        void OnGameOver();

        void OnGameRestarted();
    }
}
