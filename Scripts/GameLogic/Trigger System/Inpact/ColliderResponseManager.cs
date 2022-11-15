using Pearl.Debug;
using Pearl.Events;
using Pearl.Multitags;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public class InfoTrigger
    {
        public StringTypeControl stringType;
        public bool once;
        public bool isWait = false;
        public GameObjectEvent action;


        private bool _isDisable = false;

        public InfoTrigger(StringTypeControl stringType, GameObjectEvent action, bool once, bool isWait)
        {
            this.action = action;
            this.stringType = stringType;
            this.once = once;
            this.isWait = isWait;
        }

        public void SetDisable(bool value)
        {
            _isDisable = value;
        }

        public bool IsDisable()
        {
            return _isDisable;
        }
    }

    public class ColliderResponseManager : PearlBehaviour
    {
        public event Action OnDisableResponse;

        #region Inspector Fields
        [Header("TriggerResponse")]
        [SerializeField]
        private StringInfoTriggerDictionary enterActions = null;
        [SerializeField]
        private StringInfoTriggerDictionary exitActions = null;
        #endregion

        #region Private Fields
        private readonly Dictionary<int, int> _listTriggeredActived = new();

        private bool _isEnable = true;
        private int _methodWaitForCount = 0;
        private int _auxIndexForWait = 0;
        #endregion

        #region Property
        public bool IsEnable { get { return _isEnable; } }
        #endregion

        #region Public Methods

        protected override void OnDisable()
        {
            base.OnDisable();

            OnDisableResponse?.Invoke();
        }

        public bool Inpact(in GameObject colliderObj)
        {
            if (!colliderObj || _listTriggeredActived == null)
            {
                return false;
            }

            if (_isEnable)
            {
                int ID = GetID(colliderObj);

                if (_listTriggeredActived.TryGetValue(ID, out int counts))
                {
                    _listTriggeredActived[ID] = ++counts;
                }
                else
                {
                    _listTriggeredActived.Add(ID, 1);
                    Trigger(colliderObj, TriggerType.Enter);
                }
            }
            else
            {
                return ExitInpact(colliderObj, true);
            }

            return true;
        }

        public bool ExitInpact(in GameObject colliderObj, bool ignoreEnable = false)
        {
            if (!colliderObj || _listTriggeredActived == null || !(ignoreEnable || _isEnable))
            {
                return false;
            }

            int ID = GetID(colliderObj);

            if (_listTriggeredActived.TryGetValue(ID, out int counts))
            {
                _listTriggeredActived[ID] = --counts;

                if (counts == 0)
                {
                    _listTriggeredActived.Remove(ID);
                    Trigger(colliderObj, TriggerType.Exit);
                }

                return true;
            }

            return false;
        }
        #endregion

        #region Private Methods
        private int GetID(in GameObject colliderObj)
        {
            if (colliderObj == null)
            {
                return -1;
            }

            return colliderObj.TryGetComponent<TriggerGroup>(out TriggerGroup triggerGroup) ? triggerGroup.CodeID : colliderObj.GetInstanceID();
        }

        private void Trigger(GameObject colliderObj, TriggerType triggerType)
        {
            var actions = triggerType == TriggerType.Enter ? enterActions : exitActions;

            if (actions != null)
            {
                foreach (var pair in actions)
                {
                    var infoTrigger = pair.Value;
                    var conditionTrigger = pair.Key;

                    if (infoTrigger == null || conditionTrigger == null | infoTrigger.IsDisable())
                    {
                        continue;
                    }

                    if (IsRightCollider(infoTrigger.stringType, conditionTrigger, colliderObj))
                    {
                        if (infoTrigger.once)
                        {
                            infoTrigger.SetDisable(true);
                        }


                        if (infoTrigger.isWait)
                        {
                            var list = infoTrigger.action?.GetListComponents();
                            _methodWaitForCount = list.Count;
                            if (list.IsAlmostSpecificCount())
                            {
                                _isEnable = false;

                                foreach (var element in list)
                                {
                                    WaitManager.Wait(element, OnFinishWait);
                                }

                                infoTrigger.action?.Invoke(colliderObj);

                                if (!enabled)
                                {
                                    OnDisableResponse?.Invoke();
                                }
                            }
                            else
                            {
                                LogManager.LogWarning("There isn't one action with IWaitAction interface");
                            }

                        }
                        infoTrigger.action?.Invoke(colliderObj);
                    }
                }
            }
        }

        private void OnFinishWait()
        {
            _auxIndexForWait++;
            if (_auxIndexForWait == _methodWaitForCount)
            {
                _auxIndexForWait = 0;
                _isEnable = true;
            }
        }

        private bool IsRightCollider(in StringTypeControl stringType, in string key, in GameObject colliderObj)
        {
            return colliderObj != null && ((stringType == StringTypeControl.Layer && colliderObj.layer == LayerMask.NameToLayer(key)) ||
                      (stringType == StringTypeControl.Tag && colliderObj.CompareTag(key)) ||
                      (stringType == StringTypeControl.Name && colliderObj.name == key) ||
                      (stringType == StringTypeControl.Tags && colliderObj.HasTags(false, key)));
        }
        #endregion
    }
}
