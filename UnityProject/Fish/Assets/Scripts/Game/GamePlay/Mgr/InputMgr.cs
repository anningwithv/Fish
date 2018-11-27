using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;
using HedgehogTeam.EasyTouch;

namespace GameWish.Game
{
    public class InputMgr : TSingleton<InputMgr>
    {
        private List<IInputObserver> m_TouchObservers = new List<IInputObserver>();

        private bool m_IsTouchStartFromUI = false;

        private bool m_IsDragEnabled = true;

        public bool IsDragEnabled
        {
            get { return m_IsDragEnabled; }
            set { m_IsDragEnabled = value; }
        }

        public void Init()
        {
            EasyTouch.On_TouchStart += On_TouchStart;
            EasyTouch.On_TouchDown += On_TouchDown;
            EasyTouch.On_TouchUp += On_TouchUp;
            EasyTouch.On_Drag += On_Drag;
            EasyTouch.On_LongTap += On_LongTap;
            EasyTouch.On_Swipe += On_Swipe;
        }

        public void AddTouchObserver(IInputObserver ob)
        {
            if (!m_TouchObservers.Contains(ob))
            {
                m_TouchObservers.Add(ob);
            }
        }

        public void RemoveTouchObserver(IInputObserver ob)
        {
            if (m_TouchObservers.Contains(ob))
            {
                m_TouchObservers.Remove(ob);
            }
        }

        private void On_TouchStart(Gesture gesture)
        {
            {
                foreach (var ob in m_TouchObservers)
                {
                    ob.On_TouchStart(gesture);
                }
            }
        }

        private void On_TouchDown(Gesture gesture)
        {
            foreach (var ob in m_TouchObservers)
            {
                ob.On_TouchDown(gesture);
            }
        }

        private void On_TouchUp(Gesture gesture)
        {
            foreach (var ob in m_TouchObservers)
            {
                ob.On_TouchUp(gesture);
            }
        }

        private void On_Drag(Gesture gesture)
        {
            if (IsDragEnabled == false)
                return;

            foreach (var ob in m_TouchObservers)
            {
                ob.On_Drag(gesture, m_IsTouchStartFromUI);
            }
        }

        private void On_Swipe(Gesture gesture)
        {
            foreach (var ob in m_TouchObservers)
            {
                ob.On_Swipe(gesture);
            }
        }

        private void On_LongTap(Gesture gesture)
        {
            foreach (var ob in m_TouchObservers)
            {
                ob.On_LongTap(gesture);
            }
        }
    }
}