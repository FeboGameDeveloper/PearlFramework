using TMPro;
using UnityEngine;

namespace Pearl.Debug
{
    public class DebugScreenElement : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text nameVarText;
        [SerializeField]
        private TMP_Text valueVarText;

        private MemberComplexInfo _fieldComplex;

        public string NameField
        {
            get
            {
                if (_fieldComplex != null && _fieldComplex.memberInfo.IsNotNull(out var fieldInfo))
                {
                    return fieldInfo.Name;
                }
                return "";
            }
        }
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
            if (_fieldComplex.memberInfo.IsNotNull(out var memberInfo) && valueVarText != null)
            {
                valueVarText.text = "" + memberInfo.GetValue(_fieldComplex.container);
            }
        }

        public void SetField(MemberComplexInfo fieldComplexInfo, string name)
        {
            _fieldComplex = fieldComplexInfo;
            if (nameVarText != null && _fieldComplex.memberInfo.IsNotNull(out var fieldInfo))
            {
                nameVarText.text = name;
            }
        }
    }
}
