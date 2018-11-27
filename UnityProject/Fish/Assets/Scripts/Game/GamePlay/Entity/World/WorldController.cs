using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class WorldController : EntityController, IGameStateObserver
    {
        public static WorldController S = null;

        [SerializeField]
        private Vector3 m_PlayerSpawnPoition = Vector3.zero;
        [SerializeField]
        private Vector3 m_SawtoothSpawnPoition = Vector3.zero;

        private WorldData m_WorldData = null;
        private WorldView m_WorldView = null;

        private ResLoader m_WorldLoader = null;

        private PlayerController m_PlayerController = null;

        protected override void Init()
        {
            S = this;

            GameStateMgr.S.AddObserver(this);

            m_WorldData = new WorldData(this);
            m_WorldView = gameObject.AddComponent<WorldView>();

            m_WorldLoader = ResLoader.Allocate("WorldLoader");
        }

        private void Start()
        {
            SpawnGameObjs();

            //gameObject.AddComponent<PatternController>();
        }

        private void SpawnGameObjs()
        {
            SpawnPlayer();
        }

        protected override void SetInterestEvent()
        {
        }

        private void SpawnPlayer()
        {
            GameObject playerPrefab = m_WorldLoader.LoadSync(Define.PLAYER_PREFAB) as GameObject;
            var playerObj = GameObject.Instantiate(playerPrefab, transform) as GameObject;

            playerObj.transform.position = m_PlayerSpawnPoition;

            m_PlayerController = playerObj.GetComponent<PlayerController>();
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
            m_PlayerController.OnReset(m_PlayerSpawnPoition);
            //m_SawtoothController.OnReset(m_SawtoothSpawnPoition);
        }
#endregion
    }
}
