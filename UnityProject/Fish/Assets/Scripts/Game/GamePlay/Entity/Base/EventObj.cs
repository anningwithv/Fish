using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public abstract class EventObj : MonoBehaviour, IEventObserver
    {
        protected int[] m_InteresetEvents = null;

        public int[] InterestEvents
        {
            get
            {
                return m_InteresetEvents;
            }
        }

        protected virtual void Awake()
        {
            SetInterestEvent();
            RegisterEvents();
        }

        protected virtual void OnDestroy()
        {
            UnregisterEvents();
        }

        protected abstract void SetInterestEvent();

        #region Apply interface IEventEntity
        
        public virtual void HandleEvent(int eventId, params object[] param)
        {
            //Log.i("GameObject :" + gameObject.name + " handle event: " + eventId.ToString());
        }

        public void RegisterEvents()
        {
            //Log.i("GameObject :" + gameObject.name + " register events");

            if (InterestEvents != null && InterestEvents.Length > 0)
            {
                for (int i = 0; i < InterestEvents.Length; i++)
                {
                    EventID eventId = (EventID)InterestEvents[i];
                    EventSystem.S.Register<EventID>(eventId, HandleEvent);
                }
            }
        }

        public void UnregisterEvents()
        {
            //Log.i("GameObject :" + gameObject.name + " unregister events");

            if (InterestEvents != null && InterestEvents.Length > 0)
            {
                for (int i = 0; i < InterestEvents.Length; i++)
                {
                    EventID eventId = (EventID)InterestEvents[i];
                    EventSystem.S.UnRegister<EventID>(eventId, HandleEvent);
                }
            }
        }

        public void SendEvent(EventID eventId, params object[] param)
        {
            EventSystem.S.Send<EventID>(eventId, param);
        }

        #endregion
    }
}
