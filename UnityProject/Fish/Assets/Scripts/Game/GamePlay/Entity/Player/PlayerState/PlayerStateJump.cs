using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class PlayerStateJump : PlayerState
    {
        private Vector3 m_MoveDir;

        public PlayerStateJump(PlayerStateID stateEnum) : base(stateEnum)
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
            if(mgr.MoveDir == PlayerController.PlayerMoveDir.Left)
            {
                if(mgr.CanMoveLeft)
                    mgr.transform.position -= new Vector3(dt * mgr.PlayerData.PlayerJumpHorizontalSpeed,0,0);
            }
            else if (mgr.MoveDir == PlayerController.PlayerMoveDir.Right)
            {
                if (mgr.CanMoveRight)
                    mgr.transform.position += new Vector3(dt * mgr.PlayerData.PlayerJumpHorizontalSpeed, 0, 0);
            }
            else if (mgr.MoveDir == PlayerController.PlayerMoveDir.Vertical)
            {
                if (mgr.CanMoveUp)
                    mgr.transform.position += new Vector3(0, dt * mgr.PlayerData.PlayerJumpVerticalSpeed, 0);
            }
        }

        //private void HandlePlayerOutOfRange(PlayerController mgr)
        //{
        //    bool isOutOfRange = IsPlayerOutOfCameraRange(mgr);
        //    if (isOutOfRange)
        //    {
        //        Debug.Log("Player is out of range");
        //        if (mgr.transform.position.x < 0)
        //        {
        //            mgr.transform.position += new Vector3(Define.WORLD_WIDTH, 0, 0);
        //        }
        //        else if (mgr.transform.position.x > 0)
        //        {
        //            mgr.transform.position -= new Vector3(Define.WORLD_WIDTH, 0, 0);
        //        }
        //    }
        //}

        //private bool IsPlayerOutOfCameraRange(PlayerController mgr)
        //{
        //    float posX = mgr.transform.position.x;
        //    if ((posX >= Define.WORLD_WIDTH * 1.0f / 2 && mgr.PlayerData.CurMoveSpdX > 0) ||
        //        (posX <= -Define.WORLD_WIDTH * 1.0f / 2 && mgr.PlayerData.CurMoveSpdX < 0))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //private void Move(PlayerController mgr)
        //{
        //    mgr.PlayerData.UpdateSpeedWhenMove();

        //    Vector3 deltaPos = new Vector3(mgr.PlayerData.CurMoveSpdX * Time.deltaTime, mgr.PlayerData.CurMoveSpdY * Time.deltaTime, 0);
        //    mgr.transform.position += deltaPos;
        //    mgr.PlayerData.CurMoveDistance += deltaPos.magnitude;
        //}

    }
}
