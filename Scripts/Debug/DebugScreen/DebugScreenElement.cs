using System;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace Pearl.Testing.ScreenVars
{
    public class DebugScreenElement : MonoBehaviour
    {
        #region Inspector fields
        [SerializeField]
        private TMP_Text nameVarText;
        [SerializeField]
        private TMP_Text valueVarText;
        #endregion

        #region Private fields
        private MemberComplexInfo _fieldComplex;
        private MemberInfo _memberInfo;
        #endregion

        #region Property
        public Type IndexType { get; private set; }
        #endregion

        #region Unity Callbacks
        protected void Reset()
        {
            if (transform.childCount >= 2)
            {
                nameVarText = transform.GetChild(0).GetComponent<TMP_Text>();
                valueVarText = transform.GetChild(1).GetComponent<TMP_Text>();
            }
        }

        protected void Update()
        {
            if (_memberInfo != null && valueVarText != null)
            {
                valueVarText.text = "" + _memberInfo.GetValue(_fieldComplex.container);
            }
        }
        #endregion

        #region Public Methods
        public void SetField(MemberComplexInfo fieldComplexInfo, Type indexType, string name)
        {
            _fieldComplex = fieldComplexInfo;
            IndexType = indexType;
            if (nameVarText != null && _fieldComplex != null)
            {
                _memberInfo = _fieldComplex.memberInfo;
                nameVarText.text = name;
            }
        }
        #endregion
    }
}
