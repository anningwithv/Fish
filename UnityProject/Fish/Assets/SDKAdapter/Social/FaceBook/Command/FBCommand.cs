using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;
using Facebook.Unity;

namespace Qarth
{
    public class FBCommand
    {
        protected enum ExecuteState
        {
            Idle,
            Execute,
        }

        protected int m_ExecuteTime;
        protected ExecuteState m_State = ExecuteState.Idle;

        public bool isExecuteAble
        {
            get
            {
                if (!FB.IsInitialized)
                {
                    return false;
                }
                return m_State == ExecuteState.Idle;
            }
        }

        protected void OnExecuteBegin()
        {
            ++m_ExecuteTime;
            m_State = ExecuteState.Execute;
        }

        protected void OnExecuteFinish()
        {
            m_State = ExecuteState.Idle;
        }
    }
}