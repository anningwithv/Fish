using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class PlayerTrailSpawner
    {
        private PlayerController m_PlayerController = null;
        private float m_SpawnTime = 0f;
        private bool m_Spawn = false;
        private float m_SpawnTrailInterval = 0.15f;

        public PlayerTrailSpawner(PlayerController playerController)
        {
            m_PlayerController = playerController;
        }

        public void Update()
        {
            if (m_Spawn)
            {
                m_SpawnTime += Time.deltaTime;
                if (m_SpawnTime > m_SpawnTrailInterval)
                {
                    SpawnTrail();
                    m_SpawnTime = 0f;
                }
            }
        }

        public void StartSpawnTrail(PlayerController.PlayerMoveDir dir)
        {
            if (m_Spawn == false)
            {
                m_Spawn = true;
                m_SpawnTime = m_SpawnTrailInterval;
            }
            //m_PlayerController.GetComponent<MonoBehaviour>().StartCoroutine("SpawnTrailCor");
        }

        public void StopSpawnTrail()
        {
            m_Spawn = false;
            //m_PlayerController.GetComponent<MonoBehaviour>().StopCoroutine("SpawnTrailCor");
        }

        //private IEnumerator SpawnTrailCor()
        //{
        //    while(true)
        //    {
        //        SpawnTrail();
        //        yield return new WaitForSeconds(m_PlayerController.PlayerData.SpawnTrailInterval);
        //    }
        //}

        private void SpawnTrail()
        {
            GameObject trailObj = GameObjectPoolMgr.S.Allocate(Define.PLAYER_TRAIL_PREFAB);
            trailObj.transform.parent = m_PlayerController.transform.parent;
            trailObj.transform.position = m_PlayerController.transform.position;

            PlayerTrail playerTrail = trailObj.GetComponent<PlayerTrail>();
            playerTrail.OnSpawned(m_PlayerController.MoveDir);
        }
    }
}
