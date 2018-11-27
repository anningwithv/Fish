using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qarth
{
    public class UINodeInvisibleTrigger : ITrigger
    {
        private IUINodeFinder m_Finder;
        private Action<bool, ITrigger> m_Listener;

        public void SetParam(object[] param)
        {
            m_Finder = param[0] as IUINodeFinder;
        }

        public void Start(Action<bool, ITrigger> l)
        {
            m_Listener = l;
            EventSystem.S.Register(EngineEventID.OnPanelUpdate, OnPanelUpdte);
        }

        public void Stop()
        {
            m_Listener = null;
            EventSystem.S.UnRegister(EngineEventID.OnPanelUpdate, OnPanelUpdte);
        }

        //��Ҫ�޸�ready��flag
        public bool isReady
        {
            get
            {
                return m_Finder.FindNode(true) == null;
            }
        }

        private void OnPanelUpdte(int key, params object[] args)
        {
            Log.w("=========guide update========="+isReady);
            if (m_Listener == null)
            {
                return;
            }

            if (isReady)
            {
                m_Listener(true, this);
            }
            else
            {
                //Log.w ("Not m_Finder UINode:" + m_Finder.ToString());
                m_Listener(false, this);
            }
        }
    }
}

