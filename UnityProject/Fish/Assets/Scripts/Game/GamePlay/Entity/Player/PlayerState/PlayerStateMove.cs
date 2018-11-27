using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class PlayerStateMove : PlayerState
    {
        public PlayerStateMove(PlayerStateID stateEnum) : base(stateEnum)
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
            if (mgr.MoveDir == PlayerController.PlayerMoveDir.Left)
            {
                if (mgr.CanMoveLeft)
                    mgr.transform.position -= new Vector3(dt * mgr.PlayerData.PlayerJumpHorizontalSpeed, 0, 0);
            }
            else if (mgr.MoveDir == PlayerController.PlayerMoveDir.Right)
            {
                if (mgr.CanMoveRight)
                    mgr.transform.position += new Vector3(dt * mgr.PlayerData.PlayerJumpHorizontalSpeed, 0, 0);
            }
        }
    }
}
