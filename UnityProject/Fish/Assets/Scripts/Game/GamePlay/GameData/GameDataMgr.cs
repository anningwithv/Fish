using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    /// <summary>
    /// GameData对外交互类
    /// </summary>
    public class GameDataMgr : TSingleton<GameDataMgr>
    {
        private GameDataHandler m_GameDataHandler = null;

        public void Init()
        {
            m_GameDataHandler = new GameDataHandler();

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            //EventSystem.S.Register();
        }

        private void HandleEvent(int eventId, params object[] param)
        {
          
        }

        public void Save()
        {
            m_GameDataHandler.Save();
        }

        /// <summary>
        /// 获取所有游戏数据
        /// </summary>
        /// <returns></returns>
        public GameData GetGameData()
        {
            return m_GameDataHandler.GetGameData();
        }

    }
}