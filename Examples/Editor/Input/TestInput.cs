using Pearl.Input;
using UnityEngine;

namespace Pearl.Examples.EventExamples
{
    public class TestInput : MonoBehaviour
    {
        [InspectorButton("ActiveInput")]
        public bool activeInput;

        [InspectorButton("DisactiveInput")]
        public bool disactiveInput;

        public void ReadDown()
        {
            Debug.LogManager.Log("ReadDown");
        }

        public void ReadUp()
        {
            Debug.LogManager.Log("ReadUp");
        }

        public void ReadVector(Vector2 vector)
        {
            Debug.LogManager.Log(vector);
        }

        public void ActiveInput()
        {
            InputManager.ActivePlayer(true);
        }

        public void DisactiveInput()
        {
            InputManager.ActivePlayer(false);
        }

        public void PressEvent()
        {
            Debug.LogManager.Log("press event");
        }

        public void DoublePressEvent()
        {
            Debug.LogManager.Log("double press event");
        }

        public void DetachEvent()
        {
            Debug.LogManager.Log("detach event");
        }

        public void EnterEvent()
        {
            Debug.LogManager.Log("enter event");
        }

        public void ExitEvent()
        {
            Debug.LogManager.Log("exit event");
        }
    }
}
