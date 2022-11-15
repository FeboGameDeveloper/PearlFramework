using Pearl.Debug;
using TypeReferences;
using UnityEngine;

namespace Pearl.Examples.InspectorExamples
{
    public interface ITest
    {
        void Run();
    }

    public class InspectorTest : MonoBehaviour, ITest
    {
        //In Pearl there are the dictionaries in inspectors (Look in the contributors folder: SerializableDictionary)
        public IntegerIntegerDictionary dictionary;

        //In Pearl there are the types in inspectors (Look in the contributors folder: rotorz-classtypereference)
        [ClassImplements(typeof(PearlBehaviour))]
        public ClassTypeReference type;

        [InterfaceType(typeof(ITest))]
        public Component iTest;

        //read only
        [ReadOnly]
        public string s = "Hi";

        public bool b;
        public bool c;
        public int a;
        public Axis2DEnum axisEnum;

        //the field can be seen if the logical expression is satisfied(@ = reference)
        [ConditionalField("!@b && @b == @c && @a == 5 && @axisEnum == X")]
        public string str = "Ciao";

        //Add button in inspector
        [InspectorButton("Method1")]
        public bool testButton;

        private void Method1()
        {
            LogManager.Log("Method");
        }

        public void Run()
        {
            UnityEngine.Debug.Log("Hola");
        }

        void Start()
        {
            ((ITest)iTest).Run();
        }
    }
}
