using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;
using UnityEngine.SceneManagement;

namespace GameWish.Game
{
    public class WorldLoader : TSingleton<WorldLoader>
    {
        private ResLoader m_WorldResLoader = null;
        private Transform m_EntityRoot = null;
        private WorldController m_WorldController = null;

        public void Init(Transform entityRoot)
        {
            m_EntityRoot = entityRoot;

            //UnityExtensions.CallWithDelay(GameplayMgr.S.GetComponent<MonoBehaviour>(), () => { LoadWorld(); }, -1);
            m_WorldResLoader = ResLoader.Allocate("WorldResLoader");

            InitPatternPool();
            //InitPlayerPool();
            InitPlayerTrailPool();

            LoadWorld();
        }

        private void LoadWorld()
        {
            var worldPrefab = m_WorldResLoader.LoadSync(Define.WORLD_PREFAB);
            var worldObj = GameObject.Instantiate(worldPrefab, m_EntityRoot) as GameObject;

            m_WorldController = worldObj.GetComponent<WorldController>();
        }

        private void InitPatternPool()
        {
            for (int i = 0; i < Define.BLOCK_GROUP_COUNT; ++i)
            {
                string poolName = string.Format("Pattern_{0}", i + 1);
                GameObject prefab = m_WorldResLoader.LoadSync(poolName) as GameObject;
                GameObjectPoolMgr.S.AddPool(poolName, prefab, 4, 1);
            }
        }

        //private void InitPlayerPool()
        //{
        //    GameObject playerPrefab = m_WorldResLoader.LoadSync(Define.PLAYER_PREFAB) as GameObject;
        //    GameObjectPoolMgr.S.AddPool(Define.PLAYER_TAG, playerPrefab, 2, 1);
        //}

        private void InitPlayerTrailPool()
        {
            GameObject playerTrialPrefab = m_WorldResLoader.LoadSync(Define.PLAYER_TRAIL_PREFAB) as GameObject;
            GameObjectPoolMgr.S.AddPool(Define.PLAYER_TRAIL_PREFAB, playerTrialPrefab, 10, 5);
        }
    }
}