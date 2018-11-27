using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    /// <summary>
    /// 负责Entity的消息处理，以及对Data和View的操作，类似一个Facade
    /// </summary>
    public abstract class EntityController : EventObj
    {
        //protected EntityData m_Data = null;
        //protected EntityView m_View = null;

        protected override void Awake()
        {
            base.Awake();

            Init();
        }

        protected abstract void Init();
    }
}
