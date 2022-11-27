using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Pearl.UI;

namespace Pearl.Editor
{
    public class UIMenu : MonoBehaviour
    {
        [MenuItem("UI/AnchorsToCorners")]
        static void Init()
        {
            if (SelectionExtend.TryActiveComponent<RectTransform>(out var rect))
            {
                RectTransformExtend.AnchorsToCorners(rect);
            }
        }
    }

}