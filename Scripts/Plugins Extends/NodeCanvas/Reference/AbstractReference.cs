#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.UI;
using UnityEngine;

namespace Pearl.NodeCanvas
{
    public abstract class AbstractReference<Reference>
    {
        [SerializeField]
        public BBParameter<bool> find;

        [ExposeField]
        [Conditional("find", 0)]
        [SerializeField]
        protected BBParameter<Reference> reference;

        [Conditional("find", 1)]
        public BBParameter<StringTypeControl> typeControl;

        [Conditional("find", 1)]
        [Conditional("typeControl", (int)StringTypeControl.Name)]
        public BBParameter<bool> usePrefab;

        [Conditional("find", 1)]
        [Conditional("typeControl", (int)StringTypeControl.Name)]
        [Conditional("usePrefab", 1)]
        public BBParameter<GameObject> prefabName;

        [Conditional("find", 1)]
        [Conditional("typeControl", (int)StringTypeControl.Tags)]
        public BBParameter<bool> isEqualCountTags;

        [Conditional("find", 1)]
        [Conditional("usePrefab", 0)]
        public BBParameter<string> key;

        [ParadoxNotion.Design.Header("Root")]

        [Conditional("find", 1)]
        public BBParameter<bool> useRoot;

        [Conditional("useRoot", 1)]
        [Conditional("find", 1)]
        public BBParameter<bool> includeInactiveRoot;

        [Conditional("useRoot", 1)]
        [Conditional("find", 1)]
        public BBParameter<bool> findRoot;

        [Conditional("useRoot", 1)]
        [Conditional("findRoot", 0)]
        [Conditional("find", 1)]
        public BBParameter<Transform> rootTransform;

        [Conditional("find", 1)]
        [Conditional("useRoot", 1)]
        [Conditional("findRoot", 1)]
        public BBParameter<bool> useCanvas;

        [Conditional("find", 1)]
        [Conditional("useRoot", 1)]
        [Conditional("findRoot", 1)]
        [Conditional("useCanvas", 0)]
        public BBParameter<StringTypeControl> typeControlRoot;

        [Conditional("find", 1)]
        [Conditional("useRoot", 1)]
        [Conditional("findRoot", 1)]
        [Conditional("typeControlRoot", (int)StringTypeControl.Tags)]
        [Conditional("useCanvas", 0)]
        public BBParameter<bool> isEqualCountTagsRoot;

        [Conditional("find", 1)]
        [Conditional("useRoot", 1)]
        [Conditional("findRoot", 1)]
        [Conditional("useCanvas", 1)]
        public BBParameter<CanvasTipology> canvasTiplogy;

        [Conditional("find", 1)]
        [Conditional("useRoot", 1)]
        [Conditional("findRoot", 1)]
        [Conditional("useCanvas", 0)]
        public BBParameter<string> keyRoot;

        protected bool _init = false;

        protected GameObject FindObj()
        {
            if (Application.isPlaying)
            {
                _init = true;
            }

            if (key != null && typeControl != null && typeControl.value == StringTypeControl.Name && usePrefab != null && usePrefab.value && prefabName != null && prefabName.value != null)
            {
                key.value = prefabName.value.name;
            }

            if (typeControl != null && key != null && includeInactiveRoot != null && isEqualCountTags != null)
            {
                if (useRoot != null && useRoot.value)
                {
                    if ((rootTransform == null || rootTransform.value == null) && typeControlRoot != null && keyRoot != null && isEqualCountTagsRoot != null)
                    {
                        if (useCanvas != null && useCanvas.value && canvasTiplogy != null)
                        {
                            rootTransform = CanvasManager.GetChild(canvasTiplogy.value);
                        }
                        else
                        {
                            rootTransform = GameObjectExtend.Find<Transform>(typeControlRoot.value, ArrayExtend.CreateArray(keyRoot.value), isEqualCountTagsRoot.value);
                        }
                    }

                    return GameObjectExtend.FindInHierarchy(typeControl.value, ArrayExtend.CreateArray(key.value), rootTransform.value, includeInactiveRoot.value, isEqualCountTags.value);
                }
                else
                {
                    return GameObjectExtend.Find(typeControl.value, ArrayExtend.CreateArray(key.value), isEqualCountTags.value);
                }
            }

            return null;
        }
    }
}

#endif
