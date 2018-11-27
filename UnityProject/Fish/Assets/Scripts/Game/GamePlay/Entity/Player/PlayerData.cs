using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class PlayerData : EntityData
    {
        private PlayerController m_PlayerController = null;

        private float m_PlayerJumpHorizontalSpeed = 5f;
        private float m_PlayerJumpVerticalSpeed = 5f;
        private float m_PlayerFallSpeed = 4f;

        private float m_PlayerTotalEnergy = 5f;
        private float m_PlayerCurEnenrgy = 5f;
        private float m_PlayerEnergyDecreaseSpeed = 1f;

        public float PlayerJumpHorizontalSpeed
        {
            get { return m_PlayerJumpHorizontalSpeed; }
        }

        public float PlayerJumpVerticalSpeed
        {
            get { return m_PlayerJumpVerticalSpeed; }
        }

        public float PlayerFallSpeed
        {
            get { return m_PlayerFallSpeed; }
        }

        public PlayerData(PlayerController controller) : base(controller)
        {
            m_PlayerController = controller;

        }

        public void UseEnergy()
        {
            m_PlayerCurEnenrgy -= m_PlayerEnergyDecreaseSpeed * Time.deltaTime;
            m_PlayerCurEnenrgy = Mathf.Max(m_PlayerCurEnenrgy, 0f);
        }

        public bool HasEnergy()
        {
            bool hasEnergy = m_PlayerCurEnenrgy > 0;
            Log.i("Player has energy : " + m_PlayerCurEnenrgy);
            return hasEnergy;
        }

        public float GetEnergyPercent()
        {
            return m_PlayerCurEnenrgy / m_PlayerTotalEnergy;
        }

        public void ResetEnergy()
        {
            m_PlayerCurEnenrgy = m_PlayerTotalEnergy;
        }
    }
}
