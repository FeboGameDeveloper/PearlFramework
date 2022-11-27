using Pearl.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pearl.Editor
{
    public static class SelectionExtend
    {

        public static T ActiveComponent<T>()
        {
            GameObject aux = Selection.activeGameObject;
            if (aux != null)
            {
                return aux.GetComponent<T>();
            }

            return default;
        }

        public static bool TryActiveComponent<T>(out T result)
        {
            result = ActiveComponent<T>();
            return result != null;
        }
    }
}
