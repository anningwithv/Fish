using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class EntityData
    {
        protected EntityController m_Controller = null;

        public EntityData(EntityController controller)
        {
            m_Controller = controller;
        }
    }
}
