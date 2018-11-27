using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Qarth;

namespace GameWish.Game
{
    public class PlayerTrail : PoolObjectComponent
    {
        private float m_TotalMoveTime = 0.3f;
        private float m_MoveTime = 0f;
        private float m_MoveSpeed = 5f;
        private bool m_Move = false;
        private PlayerController.PlayerMoveDir m_MoveDir;

        public void OnSpawned(PlayerController.PlayerMoveDir dir)
        {
            m_Move = true;
            m_MoveDir = dir;
            m_MoveTime = 0f;
            //if (dir == PlayerController.PlayerMoveDir.Left)
            //{
            //    transform.DOMoveX(m_MoveDis, m_MOveTime).SetRelative().SetEase(Ease.Linear).OnComplete(()=> { GameObjectPoolMgr.S.Recycle(gameObject); });
            //}
            //else if (dir == PlayerController.PlayerMoveDir.Right)
            //{
            //    transform.DOMoveX(-m_MoveDis, m_MOveTime).SetRelative().SetEase(Ease.Linear).OnComplete(() => { GameObjectPoolMgr.S.Recycle(gameObject); });
            //}
            //else if (dir == PlayerController.PlayerMoveDir.Vertical)
            //{
            //    transform.DOMoveY(-m_MoveDis, m_MOveTime).SetRelative().SetEase(Ease.Linear).OnComplete(() => { GameObjectPoolMgr.S.Recycle(gameObject); });
            //}
        }

        private void Update()
        {
            if (m_Move == true)
            {
                m_MoveTime += Time.deltaTime;
                if (m_MoveTime > m_TotalMoveTime)
                {
                    m_Move = false;
                    //GameObjectPoolMgr.S.Recycle(gameObject);
                    //StartCoroutine(RecycleCor());
                    Destroy(gameObject);
                    return;
                }

                if (m_MoveDir == PlayerController.PlayerMoveDir.Left)
                {
                    transform.position += Vector3.right * m_MoveSpeed * Time.deltaTime;
                }
                else if (m_MoveDir == PlayerController.PlayerMoveDir.Right)
                {
                    transform.position += Vector3.left * m_MoveSpeed * Time.deltaTime;
                }
                else if (m_MoveDir == PlayerController.PlayerMoveDir.Vertical)
                {
                    transform.position += Vector3.down * m_MoveSpeed * Time.deltaTime;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<BoardBaseController>() != null)
            {
                StartCoroutine(RecycleCor());
            }
        }

        private IEnumerator RecycleCor()
        {
            yield return null;
            GameObjectPoolMgr.S.Recycle(gameObject);
        }

        public override void OnReset2Cache()
        {
            m_Move = false;
            m_MoveTime = 0;
        }
    }
}
