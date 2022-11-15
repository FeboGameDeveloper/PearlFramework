#if NODE_CANVAS


using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.UI
{
    public struct WidgetInfoWithPosition
    {
        public GameObject widget;
        public CanvasTipology canvasTipology;
        public TypeSibilling typeSibilling;
        [Conditional("typeSibilling", (int)TypeSibilling.SpecificIndex)]
        public int positionChild;
        public bool isDisabled;
    }

    public struct WidgetInfo
    {
        public GameObject widget;
        public CanvasTipology canvasTipology;
        public bool isDisabled;
    }
}
#endif

