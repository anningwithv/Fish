using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public partial class GameStateMgr : TSingleton<GameStateMgr>
    {
        public enum GameState
        {
            None,
            Start,
            Playing,
            Paused,
            Resumed,
            Restarted,
            GameOver
        }
        private GameState m_GameState = GameState.None;

        private List<IGameStateObserver> m_GameStateObserverList = new List<IGameStateObserver>();

        //private bool m_IsRunning = false;
 
        public bool IsGameRunning
        {
            get { return m_GameState == GameState.Playing; }
        }

        public void Init()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            //EventSystem.S.Register();
        }

        public void AddObserver(IGameStateObserver ob)
        {
            if (!m_GameStateObserverList.Contains(ob))
            {
                m_GameStateObserverList.Add(ob);
            }
            else
            {
                Log.w("This observer has been added before");
            }
        }

        public void RemoveObserver(IGameStateObserver ob)
        {
            if (m_GameStateObserverList.Contains(ob))
            {
                m_GameStateObserverList.Remove(ob);
            }
            else
            {
                Log.w("This observer is not in list");
            }
        }

        public void StartGame()
        {
            bool isValid = SetGameState(GameState.Start);
            if (isValid == false)
                return;

            for (int i = 0; i < m_GameStateObserverList.Count; ++i)
            {
                m_GameStateObserverList[i].OnGameStart();
            }

            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.EVENTID_STARTLEVEL);

            SetGameState(GameState.Playing);
        }

        public void PlayingGame()
        {
            bool isValid = SetGameState(GameState.Playing);
            if (isValid == false)
                return;

            for (int i = 0; i < m_GameStateObserverList.Count; ++i)
            {
                m_GameStateObserverList[i].OnGamePlaying();
            }
        }

        public void PauseGame()
        {
            bool isValid = SetGameState(GameState.Paused);
            if (isValid == false)
                return;

            for (int i = 0; i < m_GameStateObserverList.Count; ++i)
            {
                m_GameStateObserverList[i].OnGamePaused();
            }
        }

        public void ResumeGame()
        {
            bool isValid = SetGameState(GameState.Resumed);
            if (isValid == false)
                return;

            for (int i = 0; i < m_GameStateObserverList.Count; ++i)
            {
                m_GameStateObserverList[i].OnGameResumed();
            }

            SetGameState(GameState.Playing);
        }

        public void EndGame()
        {
            bool isValid = SetGameState(GameState.GameOver);
            if (isValid == false)
                return;

            for (int i = 0; i < m_GameStateObserverList.Count; ++i)
            {
                m_GameStateObserverList[i].OnGameOver();
            }
        }
        
        public void RestartGame()
        {
            bool isValid = SetGameState(GameState.Restarted);
            if (isValid == false)
                return;

            for (int i = 0; i < m_GameStateObserverList.Count; ++i)
            {
                m_GameStateObserverList[i].OnGameRestarted();
            }

            SetGameState(GameState.Playing);
        }

        protected void HandleMsgOnPlayerDead(int key, params object[] args)
        {
            if (args != null)
            {
                OnGameOver((int)args[0]);
            }
        }

        private void OnGameOver(int diedTime)
        {
        }

        private void HandleMsgOnPlayerRevive()
        {
            //m_StateMachine.SetCurrentStateByID(GameplayStateID.LevelRevive);
        }

        private bool SetGameState(GameState state)
        {
            switch (state)
            {
                case GameState.None:
                    break;
                case GameState.Start:
                    break;
                case GameState.Playing:
                    break;
                case GameState.Paused:
                    break;
                case GameState.Resumed:
                    break;
                case GameState.Restarted:
                    break;
            }

            m_GameState = state;

            return true;
        }
    }
}
