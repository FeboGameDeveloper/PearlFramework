using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public class Parallax : MonoBehaviour
    {
        public struct ParallaxData
        {
            public Vector3 initPosition;
            public float bound;
            public float parallaxEffect;
            public SpriteRenderer spiteRenderer;
            public bool isVisibile;
            public float distance;

            public ParallaxData(Vector3 initPosition, float bound, float parallaxEffect, SpriteRenderer renderer)
            {
                this.initPosition = initPosition;
                this.bound = bound;
                this.spiteRenderer = renderer;
                this.parallaxEffect = parallaxEffect;
                isVisibile = false;
                this.distance = 0;
            }
        }

        [SerializeField]
        private GameObject cam = null;
        [SerializeField]
        private float velocity = 2;
        [SerializeField]
        private UpdateModes updateModes = UpdateModes.LateUpdate;
        [SerializeField]
        private TransformFloatDictionary parallaxDictionary = null;
        [SerializeField]
        private bool isLoop = false;
        [SerializeField]
        private bool needVisible = false;
        [SerializeField]
        private Vector3 velocityParallax = Vector3.zero;

        private Dictionary<Transform, ParallaxData> _parallaxDatas = null;


        // Start is called before the first frame update
        protected void Start()
        {
            _parallaxDatas = new Dictionary<Transform, ParallaxData>();
            if (parallaxDictionary != null && _parallaxDatas != null)
            {
                foreach (var element in parallaxDictionary)
                {
                    Transform parent = element.Key;

                    if (parent != null)
                    {
                        var children = parent.GetComponentsInHierarchy<Transform>(true);

                        if (children == null || children.Count == 0)
                        {
                            children = ListExtend.CreateList(parent);
                        }

                        foreach (var tr in children)
                        {
                            if (tr == null)
                            {
                                continue;
                            }

                            var auxSpriteRenderer = tr.GetComponent<SpriteRenderer>();

                            ParallaxData data = new(tr.position, auxSpriteRenderer.bounds.size.x, element.Value, auxSpriteRenderer);
                            data.distance = CalculateDistance(data);
                            _parallaxDatas.Add(tr, data);
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        protected void Update()
        {
            if (updateModes == UpdateModes.Update)
            {
                Setting();
            }
        }

        protected void FixedUpdate()
        {
            if (updateModes == UpdateModes.FixedUpdate)
            {
                Setting();
            }
        }

        protected void LateUpdate()
        {
            if (updateModes == UpdateModes.LateUpdate)
            {
                Setting();
            }
        }

        private void Setting()
        {
            for (int i = 0; i < _parallaxDatas.Count; i++)
            {
                var element = _parallaxDatas.Keys;
                Transform tr = element.Get(i);
                ParallaxData data = _parallaxDatas[tr];

                if (tr == null)
                {
                    continue;
                }

                if (needVisible && !data.isVisibile && data.spiteRenderer != null)
                {
                    if (!data.spiteRenderer.isVisible)
                    {
                        continue;
                    }
                    else
                    {
                        data.isVisibile = true;
                        data.initPosition = tr.position;
                        data.distance = CalculateDistance(data);
                        _parallaxDatas[tr] = data;
                    }
                }

                float dist = CalculateDistance(data);
                float delta = dist - data.distance;

                data.distance = dist;
                _parallaxDatas[tr] = data;

                tr.SetTranslationInUpdate(velocityParallax.normalized * delta, TimeType.Scaled, updateModes);

                if (isLoop)
                {
                    float temp = tr.position.x * (1 - data.parallaxEffect);

                    if (temp > data.initPosition.x + data.bound)
                    {
                        data.initPosition.x += data.bound;
                    }
                    else if (temp < data.initPosition.x - data.bound)
                    {
                        data.initPosition.x -= data.bound;
                    }

                    _parallaxDatas[tr] = data;
                }
            }
        }

        private float CalculateDistance(in ParallaxData data)
        {
            Vector3 position = cam.transform.position;

            //float dist = Vector3.Distance(position, data.initPosition);

            Vector3 dist = position * (velocity * data.parallaxEffect);
            dist += data.initPosition;

            return dist.x;
        }
    }

}