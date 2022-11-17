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
            Pearl.Debug.LogManager.Log("ReadDown");
        }

        public void ReadUp()
        {
            Pearl.Debug.LogManager.Log("ReadUp");
        }

        public void ReadVector(Vector2 vector)
        {
            Pearl.Debug.LogManager.Log(vector);
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
            Pearl.Debug.LogManager.Log("press event");
        }

        public void DoublePressEvent()
        {
            Pearl.Debug.LogManager.Log("double press event");
        }

        public void DetachEvent()
        {
            Pearl.Debug.LogManager.Log("detach event");
        }

        public void EnterEvent()
        {
            Pearl.Debug.LogManager.Log("enter event");
        }

        public void ExitEvent()
        {
            Pearl.Debug.LogManager.Log("exit event");
        }
    }
}
