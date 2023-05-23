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
            Pearl.Testing.LogManager.Log("ReadDown");
        }

        public void ReadUp()
        {
            Pearl.Testing.LogManager.Log("ReadUp");
        }

        public void ReadVector(Vector2 vector)
        {
            Pearl.Testing.LogManager.Log(vector);
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
            Pearl.Testing.LogManager.Log("press event");
        }

        public void DoublePressEvent()
        {
            Pearl.Testing.LogManager.Log("double press event");
        }

        public void DetachEvent()
        {
            Pearl.Testing.LogManager.Log("detach event");
        }

        public void EnterEvent()
        {
            Pearl.Testing.LogManager.Log("enter event");
        }

        public void ExitEvent()
        {
            Pearl.Testing.LogManager.Log("exit event");
        }
    }
}
