using Pearl.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl.UI
{
    #region Enums
    public enum CanvasTipology
    {
        HUD,
        UI,
        Debug,
        Null,
    }
    #endregion

    public class CanvasManager : PearlBehaviour, ISingleton
    {
        #region Static
        public static float MatchWidthOrHeight
        {
            get
            {
                if (Singleton<CanvasManager>.GetIstance(out var canvasManager))
                {
                    if (canvasManager.TryGetComponent<CanvasScaler>(out var canvas))
                    {
                        return canvas.matchWidthOrHeight;
                    }
                }
                return 0;
            }
            set
            {
                if (Singleton<CanvasManager>.GetIstance(out var canvasManager))
                {
                    if (canvasManager.TryGetComponent<CanvasScaler>(out var canvas))
                    {
                        canvas.matchWidthOrHeight = value;
                    }
                }
            }
        }
        #endregion

        #region UnityCallbacks
        protected override void Awake()
        {
            base.Awake();

            PearlEventsManager.AddAction(ConstantStrings.ChangeResolution, FixMatch);
            FixMatch();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            PearlEventsManager.RemoveAction(ConstantStrings.ChangeResolution, FixMatch);
        }
        #endregion

        #region Static Public Methods
        public static void FixMatch()
        {
            var ratio = ScreenExtend.AspectRatio;
            MatchWidthOrHeight = ratio > 1 ? 0 : 1;
        }

        public static Transform GetChild(CanvasTipology tipology = CanvasTipology.Null)
        {
            if (Singleton<CanvasManager>.GetIstance(out var canvasManager))
            {
                string tag = tipology.ToString();
                Transform parent = canvasManager.transform;

                foreach (Transform child in canvasManager.transform)
                {
                    if (tag.Equals(child.name))
                    {
                        parent = child;
                        break;
                    }
                }

                return parent;
            }
            return null;
        }

        public static Transform GetWidget(string widgetName, CanvasTipology tipology = CanvasTipology.Null)
        {
            Transform child = GetChild(tipology);
            if (child != null)
            {
                return child.GetChildInHierarchy(widgetName);
            }
            return null;
        }

        public static GameObject GetWidgetObj(string widgetName, CanvasTipology tipology = CanvasTipology.Null)
        {
            Transform widget = GetWidget(widgetName, tipology);
            if (widget)
            {
                return widget.gameObject;
            }
            return null;
        }

        public static void Paste(Transform newChild, CanvasTipology tipology = CanvasTipology.Null, TypeSibilling typeSibilling = TypeSibilling.First, int pos = 0)
        {
            if (newChild && Singleton<CanvasManager>.GetIstance(out var canvasManager))
            {
                Transform parent = GetChild(tipology);
                if (!parent)
                {
                    parent = canvasManager.transform;
                }


                newChild.SetParent(parent, false);
                newChild.SetSibilling(typeSibilling, pos);
            }
        }
        #endregion
    }
}
