using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class PlayerView : EntityView
    {

        public void Init()
        {

        }

        private void RotateToDir(Vector3 dir, Transform body)
        {
            Vector3 forward = body.up;
            Vector3 toTargetDir = dir;
            Quaternion toTargetDirQuaternion = Quaternion.FromToRotation(forward, toTargetDir);
            body.rotation *= toTargetDirQuaternion;
        }
    }
}
