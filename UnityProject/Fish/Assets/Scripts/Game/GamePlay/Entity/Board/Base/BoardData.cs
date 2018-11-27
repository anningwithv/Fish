using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class BoardData : EntityData
    {
        private BoardBaseController m_BoardController = null;

        public BoardData(BoardBaseController controller) : base(controller)
        {
            m_BoardController = controller;
        }
    }
}
