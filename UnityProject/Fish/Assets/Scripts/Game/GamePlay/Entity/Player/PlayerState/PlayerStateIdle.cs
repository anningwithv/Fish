using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class PlayerStateIdle : PlayerState
    {
        public PlayerStateIdle(PlayerStateID stateEnum) : base(stateEnum)
        {
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
