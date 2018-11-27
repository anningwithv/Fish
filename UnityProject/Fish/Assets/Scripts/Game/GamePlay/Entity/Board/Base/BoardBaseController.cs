using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class BoardBaseController : EntityController, IBoard
    {
        private BoardData m_BoardData = null;
        private BoardView m_BoardView = null;
  
        protected override void Init()
        {
            m_BoardView = gameObject.AddComponent<BoardView>();
            m_BoardData = new BoardData(this);
        }

        protected override void SetInterestEvent()
        {
        }
    }
}
