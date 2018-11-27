using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class PlayerController : EntityController
    {
        public enum PlayerMoveDir
        {
            None,
            Left,
            Right,
            Vertical
        }

        public static PlayerController Instance = null;

        private PlayerData m_PlayerData = null;
        private PlayerView m_PlayerView = null;
        private PlayerInputMgr m_PlayerInputMgr = null;
        private PlayerRaycast m_PlayerRaycast = null;
        private PlayerTrailSpawner m_PlayerTrailSpawner = null;
        private PlayerStateMachine m_PlayerStateMachine = null;

        private PlayerMoveDir m_MoveDir = PlayerMoveDir.None;
        private PlayerMoveDir m_PreviousMoveDir = PlayerMoveDir.None;
        private bool m_CanMoveLeft = true;
        private bool m_CanMoveRight = true;
        private bool m_CanMoveUp = true;
        private bool m_CanMoveDown = true;

        public PlayerMoveDir MoveDir
        {
            get { return m_MoveDir; }
        }

        public PlayerMoveDir PreviousMoveDir
        {
            get { return m_PreviousMoveDir; }
        }

        public PlayerData PlayerData
        {
            get { return m_PlayerData; }
        }

        public PlayerView PlayerView
        {
            get { return m_PlayerView; }
        }

        public PlayerStateMachine PlayerStateMachine
        {
            get { return m_PlayerStateMachine; }
        }

        public bool CanMoveLeft
        {
            get { return m_CanMoveLeft; }
        }

        public bool CanMoveRight
        {
            get { return m_CanMoveRight; }
        }

        public bool CanMoveUp
        {
            get { return m_CanMoveUp; }
        }

        public bool CanMoveDown
        {
            get { return m_CanMoveDown; }
        }

        protected override void Init()
        {
            Instance = this;

            m_PlayerData = new PlayerData(this);

            m_PlayerView =  gameObject.AddComponent<PlayerView>();
            m_PlayerView.Init();

            m_PlayerInputMgr = gameObject.AddComponent<PlayerInputMgr>();

            m_PlayerRaycast = new PlayerRaycast(this);

            m_PlayerTrailSpawner = new PlayerTrailSpawner(this);

            m_PlayerStateMachine = new PlayerStateMachine(this);
            m_PlayerStateMachine.SetCurrentStateByID(PlayerStateID.Idle);

            SendEvent(EventID.OnPlayerSpawned, transform);
        }

        protected override void SetInterestEvent()
        {
            m_InteresetEvents = new int[] { (int)EventID.OnGetEnergy };
        }

        private void Update()
        {
            if (m_PlayerRaycast != null)
            {
                m_PlayerRaycast.Update();
            }

            if (m_PlayerTrailSpawner != null)
            {
                m_PlayerTrailSpawner.Update();
            }

            UpdateState();

        }

        private void UpdateState()
        {
            m_CanMoveLeft = !m_PlayerRaycast.HasBoardLeft;
            m_CanMoveRight = !m_PlayerRaycast.HasBoardRight;
            m_CanMoveUp = !m_PlayerRaycast.HasBoardUp;
            m_CanMoveDown = !m_PlayerRaycast.HasBoardDown;

            if (m_PlayerStateMachine.IsJumping())
            {
                m_PlayerStateMachine.UpdateState(Time.deltaTime);

                if (m_PlayerRaycast.HasBoardDown)
                {
                    SetState(PlayerStateID.Idle);
                }
            }

            if (m_PlayerStateMachine.IsFalling())
            {
                m_PlayerStateMachine.UpdateState(Time.deltaTime);

                if (m_PlayerRaycast.HasBoardDown)
                {
                    SetState(PlayerStateID.Idle);
                }
            }

            if (m_PlayerStateMachine.IsMoving())
            {
                m_PlayerStateMachine.UpdateState(Time.deltaTime);
            }
        }

        public void OnControlBtnClicked(PlayerMoveDir moveDir)
        {
            if (m_PlayerData.HasEnergy() == false)
            {
                ChangePlayerStateWithoutInput();
                return;
            }

            ChangePlayerStateWithInput(moveDir);

            m_PlayerData.UseEnergy();

            SendEvent(EventID.OnPlayerEnergyChanged, m_PlayerData.GetEnergyPercent());
        }

        private void ChangePlayerStateWithInput(PlayerMoveDir moveDir)
        {
            m_PreviousMoveDir = m_MoveDir;
            m_MoveDir = moveDir;

            m_PlayerTrailSpawner.StartSpawnTrail(moveDir);

            if (moveDir == PlayerMoveDir.Vertical)
            {
                SetState(PlayerStateID.Jump);
            }
            else
            {
                if (m_PlayerRaycast.HasBoardDown)
                {
                    SetState(PlayerStateID.Move);
                }
                else
                {
                    SetState(PlayerStateID.Jump);
                }
            }
        }

        public void OnNoControlBtnClicked()
        {
            ChangePlayerStateWithoutInput();
        }

        private void ChangePlayerStateWithoutInput()
        {
            m_PlayerTrailSpawner.StopSpawnTrail();

            if (m_PlayerRaycast.HasBoardDown == false)
            {
                SetState(PlayerStateID.Fall);
            }
            else
            {
                SetState(PlayerStateID.Idle);
            }
        }

        //public void Jump(PlayerMoveDir jumpDir)
        //{
        //    Debug.Log("Player Jump: " + jumpDir.ToString());

        //    m_MoveDir = jumpDir;

        //    SetState(PlayerStateID.Jump);
        //}

        //private void Move()
        //{
        //    Debug.Log("Player Move");

        //    SetState(PlayerStateID.Move);
        //}

        private void SetState(PlayerStateID state)
        {
            if (m_PlayerStateMachine.currentStateID == state)
                return;

            Log.i("Player change state to : " + state.ToString());

            m_PlayerStateMachine.SetCurrentStateByID(state);
        }

        public override void HandleEvent(int eventId, params object[] param)
        {
            if (eventId == (int)EventID.OnGetEnergy)
            {
                m_PlayerData.ResetEnergy();
            }
        }

        public void OnReset(Vector3 pos)
        {
            SetState(PlayerStateID.Idle);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Debug.DrawLine(transform.position, transform.position + new Vector3(0, 0.5f, 0));
            Debug.DrawLine(transform.position, transform.position + new Vector3(0, -0.5f, 0));
            Debug.DrawLine(transform.position, transform.position + new Vector3(-0.5f, 0, 0));
            Debug.DrawLine(transform.position, transform.position + new Vector3(0.5f, 0, 0));
        }
    }
}
