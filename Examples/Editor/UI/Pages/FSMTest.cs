using UnityEngine;

namespace Pearl.Examples.UI
{
    public class FSMTest : MonoBehaviour, IFSM
    {
        string label = "Page1";
        GameObject current = null;

        protected void Start()
        {
            current = GameObject.Find(label);
        }

        public void ChangeLabel(string newLabel)
        {
            label = newLabel;
        }

        public void CheckTransitions(bool forceFinishState)
        {
            current.SetActive(false);

            current = GameObjectExtend.FindInHierarchy("Canvas", label);

            current.SetActive(true);
        }

        public string GetLabel()
        {
            return default;
        }

        public T GetVariable<T>(string nameVar)
        {
            return default;
        }

        public T RemoveVariable<T>(string nameVar)
        {
            return default;
        }

        public void StartFSM()
        {
        }

        public void UpdateVariable<T>(string nameVar, T content)
        {
        }
    }
}
