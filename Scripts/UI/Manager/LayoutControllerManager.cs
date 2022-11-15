using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl.UI
{
    public class LayoutControllerManager : MonoBehaviour
    {
        [SerializeField]
        private bool isAutomatic = true;

        [SerializeField]
        private bool initAtStart = true;
        [SerializeField]
        [ConditionalField("@initAtStart")]
        private float waitTime = 0.1f;
        [SerializeField]
        [ConditionalField("!@isAutomatic")]
        private List<RectTransform> containers = null;


        // Start is called before the first frame update
        private void Start()
        {
            if (isAutomatic)
            {
                UpdateContainer();
            }

            if (initAtStart)
            {
                PearlInvoke.WaitForMethod(waitTime, UpdateLayout, TimeType.Unscaled);
            }
        }

        public void UpdateContainer()
        {
            containers = transform.GetComponentsInHierarchy<RectTransform>();
        }

        public void UpdateLayout()
        {
            foreach (var container in containers)
            {
                if (container != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(container);
                }
            }
        }
    }

}