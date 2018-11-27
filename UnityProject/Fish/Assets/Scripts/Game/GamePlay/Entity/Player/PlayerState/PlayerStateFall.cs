using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class PlayerStateFall : PlayerState
    {
        private float m_CurFallSpeed = 0f;
        private float m_CurMoveSpeedX = 0f;
        private float m_FallIncreaseSpeed = 8f;
        private float m_MoveXDecreaseSpeed = 8f;

        public PlayerStateFall(PlayerStateID stateEnum) : base(stateEnum)
        {
        }

        public override void Enter(PlayerController mgr)
        {
            if (mgr.PlayerStateMachine.previousState.stateName == "PlayerStateJump")
            {
                if (mgr.PreviousMoveDir == PlayerController.PlayerMoveDir.Vertical)
                {
                    m_CurFallSpeed = -mgr.PlayerData.PlayerJumpVerticalSpeed;
                }
                else
                {
                    m_CurFallSpeed = 0;
                    m_CurMoveSpeedX = mgr.PlayerData.PlayerJumpHorizontalSpeed;
                }
            }
        }

        public override void Exit(PlayerController mgr)
        {
        }

        public override void Execute(PlayerController mgr, float dt)
        {
            m_CurFallSpeed += m_FallIncreaseSpeed * dt;
            m_CurFallSpeed = Mathf.Min(m_CurFallSpeed, mgr.PlayerData.PlayerFallSpeed);
            if (m_CurFallSpeed < 0)
            {
                if (mgr.CanMoveUp)
                {
                    mgr.transform.position -= new Vector3(0, dt * m_CurFallSpeed, 0);
                }
            }
            else
            {
                if (mgr.CanMoveDown)
                {
                    mgr.transform.position -= new Vector3(0, dt * m_CurFallSpeed, 0);
                }
            }

            if (m_CurMoveSpeedX > 0)
            {
                m_CurMoveSpeedX -= m_MoveXDecreaseSpeed * Time.deltaTime;

                if (mgr.PreviousMoveDir == PlayerController.PlayerMoveDir.Left)
                {
                    if (mgr.CanMoveLeft)
                    {
                        mgr.transform.position -= new Vector3(m_CurMoveSpeedX * dt, 0, 0);
                    }
                }
                else if (mgr.PreviousMoveDir == PlayerController.PlayerMoveDir.Right)
                {
                    if (mgr.CanMoveRight)
                    {
                        mgr.transform.position += new Vector3(m_CurMoveSpeedX * dt, 0, 0);
                    }
                }
            }
        }
    }
}
