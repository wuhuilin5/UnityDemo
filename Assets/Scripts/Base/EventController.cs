using System;
using System.Collections.Generic;

namespace UnityDemo.Event
{
    public class EventController
    {
        #region Fields
        private Dictionary<string, Delegate> mEventListeners = new Dictionary<string, Delegate>();
        #endregion

        #region Methods
        public void AddEventListener<T>(string eventType, Action<T> listener)
        {
            OnListenerAdding(eventType, listener);
            mEventListeners[eventType] = (Action<T>)Delegate.Combine((Action<T>)mEventListeners[eventType], listener);
        }

        private void OnListenerAdding(string eventType, Delegate listener)
        {
            if (!mEventListeners.ContainsKey(eventType))
            {
                mEventListeners.Add(eventType, null);
            }

            Delegate tmpDelegate = mEventListeners[eventType];
            if (tmpDelegate != null && tmpDelegate.GetType() != listener.GetType())
            {
                throw new Exception(string.Format("Try to add not correct event {0}. Current type is {1}, adding type is {2}.", eventType, tmpDelegate.GetType().Name, listener.GetType().Name));
            }
        }

        #endregion
    }
}
