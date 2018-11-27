using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class PlayerRaycast
    {
        private PlayerController m_PlayerController = null;

        private float m_BodyHeight = 0.7f;
        private float m_BodyWidth = 0.7f;
        private int m_IgnoreLayerMask = ~(1<<LayerMask.NameToLayer(Define.PLAYER_LAYER));

        private bool m_HasBoardLeft = false;
        private RaycastHit2D m_RaycastHitLeft;

        private bool m_HasBoardRight = false;
        private RaycastHit2D m_RaycastHitRight;

        private bool m_HasBoardUp = false;
        private RaycastHit2D m_RaycastHitUp;

        private bool m_HasBoardBottom = false;
        private RaycastHit2D m_RaycastHitBottom;

        private Vector2 m_RaycastOriginPos;

        public bool HasBoardLeft
        {
            get { return m_HasBoardLeft; }
        }

        public bool HasBoardRight
        {
            get { return m_HasBoardRight; }
        }

        public bool HasBoardUp
        {
            get { return m_HasBoardUp; }
        }

        public bool HasBoardDown
        {
            get { return m_HasBoardBottom; }
        }

        public PlayerRaycast(PlayerController playerController)
        {
            m_PlayerController = playerController;
        }

        public void Update()
        {
            m_RaycastOriginPos = new Vector2(m_PlayerController.transform.position.x, m_PlayerController.transform.position.y);

            RaycastToLeft();
            RaycastToRight();
            RaycastToUp();
            RaycastToBottom();
        }

        private void RaycastToLeft()
        {
            m_HasBoardLeft = Raycast(Vector2.left, m_BodyWidth / 2f, out m_RaycastHitLeft);
        }

        private void RaycastToRight()
        {
            m_HasBoardRight = Raycast(Vector2.right, m_BodyWidth / 2f, out m_RaycastHitRight);
        }

        private void RaycastToUp()
        {
            m_HasBoardUp = Raycast(Vector2.up, m_BodyHeight / 2f, out m_RaycastHitUp);
        }

        private void RaycastToBottom()
        {
            m_HasBoardBottom = Raycast(Vector2.down, m_BodyHeight / 2f, out m_RaycastHitBottom);
            //Log.i("Has board bottom : " + m_HasBoardBottom);
        }

        private bool Raycast(Vector2 dir, float distance, out RaycastHit2D raycastHit2D)
        {
            raycastHit2D = Physics2D.Raycast(m_RaycastOriginPos, dir, distance, m_IgnoreLayerMask);

            if (raycastHit2D.transform != null)
            {
                if(raycastHit2D.transform.GetComponent<BoardBaseController>()!= null)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
