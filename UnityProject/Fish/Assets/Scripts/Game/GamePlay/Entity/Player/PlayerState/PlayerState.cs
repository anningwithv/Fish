using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public enum PlayerStateID
    {
        None,
        Idle,
        Move,
        Jump,
        Fall,
        Dead
    }

    public class PlayerState : FSMState<PlayerController>
    {
        public PlayerStateID stateID
        {
            get;
            set;
        }

        public PlayerState(PlayerStateID stateEnum)
        {
            stateID = stateEnum;
        }

        public override void Enter(PlayerController mgr)
        {
        }

        public override void Exit(PlayerController mgr)
        {
        }

        public override void Execute(PlayerController mgr, float dt)
        {
        }
    }
}
