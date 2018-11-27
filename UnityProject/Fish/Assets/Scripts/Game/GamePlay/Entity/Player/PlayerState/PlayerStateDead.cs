using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class PlayerStateDead : PlayerState
    {
        public PlayerStateDead(PlayerStateID stateEnum) : base(stateEnum)
        {
        }

        public override void Enter(PlayerController mgr)
        {
            GameStateMgr.S.EndGame();

        }

        public override void Exit(PlayerController mgr)
        {
        }

        public override void Execute(PlayerController mgr, float dt)
        {
        }
    }
}
