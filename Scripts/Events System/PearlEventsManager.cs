using Pearl.Testing;
using System;
using System.Collections.Generic;

namespace Pearl.Events
{
    public enum PearlEventType { Normal, Trigger }


    /// <summary>
    /// This static class manages the communication between different gameobjects in a centralized way. 
    /// For example, the gameobject "A" must receive instructions from the gameobject "B": 
    /// A subscribes to an event "eV" of the "EventsManager" class. When "B" wants to call "A", it 
    /// invokes the "eV" event of the "EventsManager" class with the necessary parameters.
    /// Event "ev" invokes object "A" and all other objects subscribed to "e".
    /// </summary>
    public static class PearlEventsManager
    {
        #region Readonly fields
        private static readonly Dictionary<string, Delegate> _dictionaryEvent = new();

        private static readonly Dictionary<string, System.Object[]> _dictionaryTrigger = new();
        #endregion

        #region Change Events

        #region Change method in event
        /// <summary>
        /// For add or delete action with delegate
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "ev">Does the action have to be added or removed?</param>
        /// <param name = "action">the action that the event must possess/ delete</param>
        private static bool ChangeDelegate(in string eventString, Delegate action, in ActionEvent ev, bool solo = false)
        {
            if (ev == ActionEvent.Add)
            {
                return AddDelegate(eventString, action, solo);
            }
            else
            {
                return RemoveDelegate(eventString, action);
            }
        }

        /// <summary>
        /// For add or delete action with action with 0 parameter
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "ev">Does the action have to be added or removed?</param>
        /// <param name = "action">the action that the event must possess/ delete</param>
        public static bool ChangeAction(in string eventString, Action action, in ActionEvent ev, bool solo = false)
        {
            return ChangeDelegate(eventString, action, ev, solo);
        }

        /// <summary>
        /// For add or delete action with action with 1 parameter
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "ev">Does the action have to be added or removed?</param>
        /// <param name = "action">the action that the event must possess/ delete</param>
        public static bool ChangeAction<T>(in string eventString, Action<T> action, in ActionEvent ev, bool solo = false)
        {
            return ChangeDelegate(eventString, action, ev, solo);
        }

        /// <summary>
        /// For add or delete action with action with 2 parameters
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "ev">Does the action have to be added or removed?</param>
        /// <param name = "action">the action that the event must possess/ delete</param>
        public static bool ChangeAction<T, F>(in string eventString, Action<T, F> action, in ActionEvent ev, bool solo = false)
        {
            return ChangeDelegate(eventString, action, ev, solo);
        }

        /// <summary>
        /// For add or delete action with action with 3 parameters
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "ev">Does the action have to be added or removed?</param>
        /// <param name = "action">the action that the event must possess/ delete</param>
        public static bool ChangeAction<T, F, Z>(in string eventString, Action<T, F, Z> action, in ActionEvent ev, bool solo = false)
        {
            return ChangeDelegate(eventString, action, ev, solo);
        }
        #endregion

        #region Add method in event
        /// <summary>
        /// For add action with delegate
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that the event must possess</param>
        private static bool AddDelegate(string eventString, Delegate action, bool solo = false)
        {
            if (action == null || eventString == null || _dictionaryEvent == null)
            {
                return false;
            }

            eventString = eventString.ToLower();

            //if the action is a trigger, if the trigger has already occurred, the action fires
            if (_dictionaryTrigger.TryGetValue(eventString, out object[] parameters))
            {
                action.DynamicInvoke(parameters);
                return true;
            }

            _dictionaryEvent.TryGetValue(eventString, out Delegate pastValue);

            if (solo && pastValue != null)
            {
                LogManager.LogWarning("this event was overwritten");
                return false;
            }

            if (pastValue != null)
            {
                Delegate[] list = pastValue.GetInvocationList();
                foreach (Delegate d in list)
                {
                    if (d == action)
                    {
                        LogManager.LogWarning("It already exists");
                        return false;
                    }
                }
            }

            try
            {
                _dictionaryEvent.Update(eventString, Delegate.Combine(pastValue, action));
            }
            catch (ArgumentException e)
            {
                LogManager.LogWarning(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// For add action with action with 0 parameter
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that the event must possess</param>
        public static bool AddAction(in string eventString, Action action, bool solo = false)
        {
            return AddDelegate(eventString, action, solo);
        }

        /// <summary>
        /// For add action with action with 1 parameter
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that the event must possess</param>
        public static bool AddAction<T>(in string eventString, Action<T> action, bool solo = false)
        {
            return AddDelegate(eventString, action, solo);
        }

        /// <summary>
        /// For add action with action with 2 parameters
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that the event must possess</param>
        public static bool AddAction<T, F>(in string eventString, Action<T, F> action, bool solo = false)
        {
            return AddDelegate(eventString, action, solo);
        }

        /// <summary>
        /// For add action with action with 3 parameters
        /// </summary>
        /// <param name = "solo"> Only this action must be in this event</param>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that the event must possess</param>
        public static bool AddAction<T, F, Z>(in string eventString, Action<T, F, Z> action, bool solo = false)
        {
            return AddDelegate(eventString, action, solo);
        }
        #endregion

        #region Remove method in event
        /// <summary>
        /// For remove action with delegate
        /// </summary>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that is to remove the event</param>
        public static bool RemoveDelegate(string eventString, Delegate action)
        {
            if (action == null || eventString == null || _dictionaryEvent == null)
            {
                return false;
            }

            eventString = eventString.ToLower();

            if (_dictionaryEvent.TryGetValue(eventString, out Delegate pastValue))
            {
                Delegate newDelegate;

                try
                {
                    newDelegate = Delegate.Remove(pastValue, action);
                }
                catch (ArgumentException e)
                {
                    LogManager.LogWarning(e.Message);
                    return false;
                }

                if (newDelegate != null && newDelegate.GetInvocationList().Length > 0)
                {
                    _dictionaryEvent.Update(eventString, newDelegate);
                }
                else
                {
                    _dictionaryEvent.Remove(eventString);
                }
            }
            return true;
        }

        /// <summary>
        /// For remove action with action with 0 parameter
        /// </summary>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that is to remove the event</param>
        public static bool RemoveAction(in string eventString, Action action)
        {
            return RemoveDelegate(eventString, action);
        }

        /// <summary>
        /// For remove action with action with 1 parameter
        /// </summary>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that is to remove the event</param>
        public static bool RemoveAction<T>(in string eventString, Action<T> action)
        {
            return RemoveDelegate(eventString, action);
        }

        /// <summary>
        /// For remove action with action with 2 parameters
        /// </summary>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that is to remove the event</param>
        public static bool RemoveAction<T, F>(in string eventString, Action<T, F> action)
        {
            return RemoveDelegate(eventString, action);
        }

        /// <summary>
        /// For remove action with action with 3 parameters
        /// </summary>
        /// <param name = "eventString">The ID of event</param>
        /// <param name = "action">the action that is to remove the event</param>
        public static bool RemoveAction<T, F, Z>(in string eventString, Action<T, F, Z> action)
        {
            return RemoveAction(eventString, action);
        }
        #endregion

        #endregion

        #region Call events
        public static bool CallEvent(string eventString, params System.Object[] objects)
        {
            return CallEvent(eventString, PearlEventType.Normal, objects);
        }

        public static bool CallTrigger(string eventString, params System.Object[] objects)
        {
            return CallEvent(eventString, PearlEventType.Trigger, objects);
        }

        /// <summary>
        /// This function calls the specific fast event to activate all its subscribed methods.
        /// Beware of fast events, since they do not check if their MonoBehaviour has been destroyed, 
        /// the user must remove the method manually (using the RemoveMethod function).
        /// <param name = "eventAction">The dictionary key associated with the event</param>
        /// <param name = "eventType">The events is normal or trigger? </param>
        /// <param name = "objects">The parameters for invoke</param>
        /// </summary>
        public static bool CallEvent(string eventString, PearlEventType eventType, params System.Object[] objects)
        {
            if (eventString == null || _dictionaryEvent == null)
            {
                return false;
            }

            eventString = eventString.ToLower();

            bool isTrigger = eventType == PearlEventType.Trigger;
            if (isTrigger)
            {
                if (_dictionaryTrigger.ContainsKey(eventString))
                {
                    return true;
                }
                else
                {
                    _dictionaryTrigger.Add(eventString, objects);
                }
            }

            _dictionaryEvent.TryGetValue(eventString, out Delegate currentDelegate);

            if (currentDelegate == null)
            {
                return false;
            }

            try
            {
                currentDelegate.DynamicInvoke(objects);
            }
            catch (Exception e)
            {
                LogManager.LogWarning(e.Message);
                return false;
            }

            if (isTrigger)
            {
                ClearEvent(eventString);
            }

            return true;
        }

        public static bool CallEventWithReturn(string eventString, Action actionOnWait, params System.Object[] objects)
        {
            return CallEventWithReturn(eventString, PearlEventType.Normal, actionOnWait, objects);
        }

        public static bool CallTriggerWithReturn(string eventString, Action actionOnWait, params System.Object[] objects)
        {
            return CallEventWithReturn(eventString, PearlEventType.Trigger, actionOnWait, objects);
        }

        /// <summary>
        /// the function is similar to the CallEvent with one difference: when all the actions are executed, 
        /// the actionOnWait action will be executed. 
        /// Each integer action will have to execute at the end: WaitManager.Call(this).
        /// <param name = "eventAction">The dictionary key associated with the event</param>
        /// <param name = "eventType">The events is normal or trigger? </param>
        /// <param name = "actionOnWait">The action to be performed at the end of everything</param>
        /// <param name = "objects">The parameters for invoke</param>
        /// </summary>
        public static bool CallEventWithReturn(string eventString, PearlEventType eventType, Action actionOnWait, params System.Object[] objects)
        {
            eventString = eventString.ToLower();
            if (_dictionaryEvent.TryGetValue(eventString, out Delegate currentDelegate))
            {
                var list = currentDelegate.GetInvocationList();

                foreach (var element in list)
                {
                    WaitManager.Wait(element.Target, actionOnWait);
                }
            }

            return CallEvent(eventString, eventType, objects);
        }

        public static bool CallEventWithReturn<T>(string eventString, Action<T> actionOnWait, params System.Object[] objects)
        {
            return CallEventWithReturn<T>(eventString, PearlEventType.Normal, actionOnWait, objects);
        }

        public static bool CallTriggerWithReturn<T>(string eventString, Action<T> actionOnWait, params System.Object[] objects)
        {
            return CallEventWithReturn<T>(eventString, PearlEventType.Trigger, actionOnWait, objects);
        }

        /// <summary>
        /// the function is similar to the CallEvent with one difference: when all the actions are executed, 
        /// the actionOnWait action will be executed. 
        /// Each integer action will have to execute at the end: WaitManager.Call(this).
        /// <param name = "eventAction">The dictionary key associated with the event</param>
        /// <param name = "eventType">The events is normal or trigger? </param>
        /// <param name = "actionOnWait">The action to be performed at the end of everything</param>
        /// <param name = "objects">The parameters for invoke</param>
        /// </summary>
        public static bool CallEventWithReturn<T>(string eventString, PearlEventType eventType, Action<T> actionOnWait, params System.Object[] objects)
        {
            eventString = eventString.ToLower();
            if (_dictionaryEvent.TryGetValue(eventString, out Delegate currentDelegate))
            {
                var list = currentDelegate.GetInvocationList();

                foreach (var element in list)
                {
                    WaitManager.Wait<T>(element.Target, actionOnWait);
                }
            }

            return CallEvent(eventString, eventType, actionOnWait, objects);
        }
        #endregion

        #region Clear Events
        /// <summary>
        /// Clear all event
        /// </summary>
        public static void ClearAllEvent()
        {
            if (_dictionaryEvent != null)
            {
                _dictionaryEvent.Clear();
            }
        }

        /// <summary>
        /// Clear specific event
        /// </summary>
        public static void ClearEvent(in string eventString)
        {
            if (_dictionaryEvent != null)
            {
                _dictionaryEvent.Remove(eventString);
            }
        }

        /// <summary>
        /// Clear All trigger event
        /// </summary>
        public static void ClearAllTrigger()
        {
            if (_dictionaryTrigger != null)
            {
                _dictionaryTrigger.Clear();
            }
        }

        /// <summary>
        /// Clear specific trigger event
        /// </summary>
        public static void ClearTrigger(in string eventString)
        {
            if (_dictionaryTrigger != null)
            {
                _dictionaryTrigger.Remove(eventString);
            }
        }
        #endregion
    }
}
