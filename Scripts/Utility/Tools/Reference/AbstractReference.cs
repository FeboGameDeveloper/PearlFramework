using Pearl.UI;
using UnityEngine;

namespace Pearl
{
    public abstract class AbstractReference<Reference>
    {
        [SerializeField]
        protected bool find;

        [ConditionalField("!@find")]
        [SerializeField]
        protected Reference reference;

        [ConditionalField("@find")]
        [SerializeField]
        protected StringTypeControl typeControl;

        [ConditionalField("@find && @typeControl == Name")]
        [SerializeField]
        protected bool usePrefab;

        [ConditionalField("@find && @typeControl == Name && @usePrefab")]
        [SerializeField]
        protected GameObject prefabName;

        [ConditionalField("@find && @typeControl == Tags")]
        [SerializeField]
        protected bool isEqualCountTags;

        [ConditionalField("@find && (@typeControl != Name || !@usePrefab)")]
        [SerializeField]
        protected string keyForFind;

        [UnityEngine.Header("Root")]

        [SerializeField]
        [ConditionalField("@find")]
        protected bool useRoot;

        [ConditionalField("@find && @useRoot")]
        [SerializeField]
        protected bool includeInactiveChildren;

        [SerializeField]
        [ConditionalField("@find && @useRoot")]
        protected bool findRoot;

        [ConditionalField("@find && @useRoot && !@findRoot")]
        [SerializeField]
        protected Transform rootTransform;

        [ConditionalField("@find && @useRoot && @findRoot")]
        [SerializeField]
        protected bool useCanvas;

        [ConditionalField("@find && @useRoot && @findRoot && !@useCanvas")]
        [SerializeField]
        protected StringTypeControl typeControlRoot;

        [ConditionalField("@useRoot && @findRoot && @typeControlRoot == Tags && !@useCanvas")]
        [SerializeField]
        protected bool isEqualCountTagsRoot;

        [ConditionalField("@find && @useRoot && @findRoot && @useCanvas")]
        [SerializeField]
        protected CanvasTipology canvasTiplogy;

        [ConditionalField("@find && @useRoot && @findRoot && !@useCanvas")]
        [SerializeField]
        protected string keyForFindRoot;

        protected bool _init = false;

        protected GameObject FindObj()
        {
            _init = true;

            if (typeControl == StringTypeControl.Name && usePrefab)
            {
                keyForFind = prefabName.name;
            }


            if (useRoot)
            {
                if (rootTransform == null)
                {
                    if (useCanvas)
                    {
                        rootTransform = CanvasManager.GetChild(canvasTiplogy);
                    }
                    else
                    {
                        rootTransform = GameObjectExtend.Find<Transform>(typeControlRoot, ArrayExtend.CreateArray(keyForFindRoot), isEqualCountTagsRoot);
                    }
                }

                return GameObjectExtend.FindInHierarchy(typeControl, ArrayExtend.CreateArray(keyForFind), rootTransform, includeInactiveChildren, isEqualCountTags);
            }
            else
            {
                return GameObjectExtend.Find(typeControl, ArrayExtend.CreateArray(keyForFind), isEqualCountTags);
            }
        }
    }
}
