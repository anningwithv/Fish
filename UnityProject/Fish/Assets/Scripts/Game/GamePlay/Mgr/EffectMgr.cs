using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qarth;

namespace GameWish.Game
{
    public class EffectMgr : TMonoSingleton<EffectMgr>
    {
        //[SerializeField]
        //private Transform  m_EffectRoot;
        private ResLoader  m_ResLoader;

        public void PlayHarborGoldParticle(Transform parent, Vector3 position, Quaternion rotation ,float recycleTime = 3.0f)
        {
            GameObject particle = GameObjectPoolMgr.S.Allocate("Particle_gold_harbor");
            PlayParticleSystem(particle, parent, position, rotation,recycleTime,false);
        }

        public void PlayMonsterAttackedParticle(Transform parent, Vector3 position, Quaternion rotation, float recycleTime = 3.0f)
        {
            GameObject particle = GameObjectPoolMgr.S.Allocate("Particle_monster_hit");
            PlayParticleSystem(particle, parent, position, rotation, recycleTime, false);
        }

        public void PlayEffByPro(string effName, Vector3 position, Quaternion rotation, bool isUseSelfTime, float recycleTime = 3.0f,Transform parent=null)
        {
            GameObject objParticle = GameObjectPoolMgr.S.Allocate(effName);
            if(parent!=null)
            {
                SetEffectOrder setEffectOrder = objParticle.GetComponent<SetEffectOrder>();
                Canvas canvas = parent.GetComponent<Canvas>();
                if (setEffectOrder != null && canvas != null)
                    setEffectOrder.SetCanvas(canvas);
            }
            PlayParticleSystem(objParticle, parent, position, rotation, recycleTime, isUseSelfTime);
        }

        public GameObject PlayLoopEffect(string effName,Transform parent, Vector3 position, Quaternion rotation)
        {
            GameObject p = GameObjectPoolMgr.S.Allocate(effName);
            ParticleSystem ps = p.GetComponent<ParticleSystem>();
            if (ps == null)
            {
                ps = p.GetComponentInChildren<ParticleSystem>();
            }
            ps.Clear();

            if (parent != null)
                p.transform.SetParent(parent);
            p.transform.localPosition = position;
            p.transform.localRotation = rotation;
            p.transform.localScale=Vector3.one;

            ps.Play();
            return p;
        }

        protected void PlayParticleSystem(GameObject p, Transform parent, Vector3 position, Quaternion rotation,float recycleTime,bool isUseSelfTime)
        {
            ParticleSystem ps = p.GetComponent<ParticleSystem>();
            if (ps == null)
            {
                ps = p.GetComponentInChildren<ParticleSystem>();
            }
            ps.Clear();

            if(parent!=null)
            p.transform.SetParent(parent);
            p.transform.position = position;
            p.transform.rotation = rotation;

            ps.Play();

            if (isUseSelfTime)
                recycleTime = ps.main.duration;
            UnityExtensions.CallWithDelay(this, () => { GameObjectPoolMgr.S.Recycle(p.gameObject); },recycleTime);
        }

        public void RecycleLoopPartical(GameObject loopPartical)
        {
            GameObjectPoolMgr.S.Recycle(loopPartical);
        }

        public void Init()
        {
            m_ResLoader = ResLoader.Allocate("EffectLoader");

            //GameObject goldInHarborParticle = m_ResLoader.LoadSync("Particle_gold_harbor") as GameObject;
            //GameObjectPoolMgr.S.AddPool("Particle_gold_harbor", goldInHarborParticle, 10, 5);

            //GameObject monsterAttackedParticle = m_ResLoader.LoadSync("Particle_monster_hit") as GameObject;
            //GameObjectPoolMgr.S.AddPool("Particle_monster_hit", monsterAttackedParticle, 2, 1);

            //GameObject shipUpEffPrefab = m_ResLoader.LoadSync(Define.EXP_SHIPMERGE_UPLEVEL) as GameObject;
            //GameObjectPoolMgr.S.AddPool(Define.EXP_SHIPMERGE_UPLEVEL, shipUpEffPrefab, 3, 1);

            //GameObject shopGoldEffect=m_ResLoader.LoadSync(Define.SHOP_GOLD)as GameObject;
            //GameObjectPoolMgr.S.AddPool(Define.SHOP_GOLD, shopGoldEffect, 10, 6);

            //GameObject shopDiamondEffect = m_ResLoader.LoadSync(Define.SHOP_DIAMOND) as GameObject;
            //GameObjectPoolMgr.S.AddPool(Define.SHOP_DIAMOND, shopDiamondEffect, 10, 6);

            //GameObject boostEffect = m_ResLoader.LoadSync(Define.SHIP_BOOST_WIND) as GameObject;
            //GameObjectPoolMgr.S.AddPool(Define.SHIP_BOOST_WIND, boostEffect, 20, 1);

            //GameObject ballLevel2Particle = m_ResLoader.LoadSync("Fire_Level_02") as GameObject;
            //GameObjectPoolMgr.S.AddPool("FireLevel2", ballLevel2Particle, 2, 1);

            //GameObject ballLevel3Particle = m_ResLoader.LoadSync("Fire_Level_03") as GameObject;
            //GameObjectPoolMgr.S.AddPool("FireLevel3", ballLevel3Particle, 2, 1);

            //GameObject ballDunkParticle = m_ResLoader.LoadSync("Effect_Star_Particle") as GameObject;
            //GameObjectPoolMgr.S.AddPool("StarParticle", ballDunkParticle, 2, 1);

        }

       

    }
}
