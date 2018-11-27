using Qarth;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace GameWish.Game
{
    public class UILoader : TSingleton<UILoader>
    {
        private ResLoader m_UIResLoader = null;

        /// <summary>
        /// 入口方法
        /// </summary>
        public void Init()
        {
            m_UIResLoader = ResLoader.Allocate("UIResLoader");

            InitGameObjectPools();
        }

        private void InitGameObjectPools()
        {
            //GameObjectPoolMgr.S.AddPool(Define.SHIP_MERGE_BTN_NAME, m_UIResLoader.LoadSync(Define.SHIP_MERGE_BTN_NAME) as GameObject, 30, 6);
            //GameObjectPoolMgr.S.AddPool(Define.SHIP_SHDOW_NAME, m_UIResLoader.LoadSync(Define.SHIP_SHDOW_NAME) as GameObject, 3, 1);
            //GameObjectPoolMgr.S.AddPool(Define.SHIP_MERGE_EFF, m_UIResLoader.LoadSync(Define.SHIP_MERGE_EFF) as GameObject, 5,1);
            //GameObjectPoolMgr.S.AddPool(Define.SHIP_EXP_TRAILEFF,m_UIResLoader.LoadSync(Define.SHIP_EXP_TRAILEFF) as GameObject, 3, 1);
            //GameObjectPoolMgr.S.AddPool(Define.DIGIT_JUMP_ATK_MONSTER, m_UIResLoader.LoadSync(Define.DIGIT_JUMP_ATK_MONSTER) as GameObject,8, 3);
            //GameObjectPoolMgr.S.AddPool(Define.DIGIT_SHIP_COIN_REWARD, m_UIResLoader.LoadSync(Define.DIGIT_SHIP_COIN_REWARD) as GameObject, 8, 3);
            //GameObjectPoolMgr.S.AddPool(Define.SHIP_GOINREWARD_TRAIL, m_UIResLoader.LoadSync(Define.SHIP_GOINREWARD_TRAIL) as GameObject, 8, 3);
        }

        //GameObject m_obj = null;
        //public GameObject SpawnMergeBtn()
        //{
        //    m_obj = null;
        //    m_obj = GameObjectPoolMgr.S.Allocate(Define.SHIP_MERGE_BTN_NAME);
        //    if(m_obj==null)
        //    {
        //        Log.e("m_obj =null");
        //    }
        //    return m_obj;
        //}

        //public GameObject SpawShipShadow()
        //{
        //    m_obj = null;
        //    m_obj = GameObjectPoolMgr.S.Allocate(Define.SHIP_SHDOW_NAME);
        //    if (m_obj == null)
        //    {
        //        Log.e("m_obj =null");
        //    }
        //    return m_obj;
        //}

        //public GameObject SpawShipMergeEff()
        //{
        //    m_obj = null;
        //    m_obj = GameObjectPoolMgr.S.Allocate(Define.SHIP_MERGE_EFF);
        //    if (m_obj == null)
        //    {
        //        Log.e("m_obj =null");
        //    }
        //    return m_obj;
        //}

        //public GameObject SpawExpTrailEff()
        //{
        //    m_obj = null;
        //    m_obj = GameObjectPoolMgr.S.Allocate(Define.SHIP_EXP_TRAILEFF);
        //    if (m_obj == null)
        //    {
        //        Log.e("m_obj =null");
        //    }
        //    return m_obj;
        //}

        //public GameObject SpawDigitJump()
        //{
        //    m_obj = null;
        //    m_obj = GameObjectPoolMgr.S.Allocate(Define.DIGIT_JUMP_ATK_MONSTER);
        //    if (m_obj == null)
        //    {
        //        Log.e("m_obj =null");
        //    }
        //    return m_obj;
        //}

        //public GameObject SpawnShipDigitInWorldReward()
        //{
        //    m_obj = null;
        //    m_obj = GameObjectPoolMgr.S.Allocate(Define.DIGIT_SHIP_COIN_REWARD);
        //    if (m_obj == null)
        //    {
        //        Log.e("m_obj =null");
        //    }
        //    return m_obj;
        //}

        //public GameObject SpawnCoinRewardTrail()
        //{
        //    m_obj = null;
        //    m_obj = GameObjectPoolMgr.S.Allocate(Define.SHIP_GOINREWARD_TRAIL);
        //    if (m_obj == null)
        //    {
        //        Log.e("m_obj =null");
        //    }
        //    return m_obj;
        //}

        public void RecycleObj(GameObject go)
        {
            GameObjectPoolMgr.S.Recycle(go);
        }
    }
}