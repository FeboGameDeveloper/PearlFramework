using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public class FakeProspect : MonoBehaviour
    {
        [SerializeField]
        private Vector2 addedScaleRange = Vector2.one;
        [SerializeField]
        private float positionForMinScale = 1;
        [SerializeField]
        private float positionForMaxScale = 10;
        [SerializeField]
        private Axis3DEnum axisProspect = Axis3DEnum.Y;


        private Dictionary<Transform, Vector3> _originsScales = new Dictionary<Transform, Vector3>();

        public void RemoveLayer(Transform tr)
        {
            if (_originsScales != null && tr != null)
            {
                _originsScales.Remove(tr);
            }
        }

        public void OnDisableLayer(GameObject obj)
        {
            RemoveLayer(obj.transform);
        }

        public void GetScale(Transform transform, Vector3 position)
        {
            float positionAxis = 0;

            if (axisProspect == Axis3DEnum.X)
            {
                positionAxis = position.x;
            }
            if (axisProspect == Axis3DEnum.Y)
            {
                positionAxis = position.y;
            }
            else
            {
                positionAxis = position.z;
            }

            GetScale(transform, positionAxis);
        }

        public void GetScale(Transform tr, float position)
        {
            if (tr == null)
            {
                return;
            }

            float percent = MathfExtend.Percent(position, positionForMinScale, positionForMaxScale);
            float newScale = Mathf.Lerp(addedScaleRange.x, addedScaleRange.y, percent);

            if (_originsScales != null && !_originsScales.ContainsKey(tr))
            {
                _originsScales.Add(tr, tr.localScale);
                if (tr.TryGetComponent<PearlObject>(out var obj))
                {
                    obj.DisactiveHandler += OnDisableLayer;
                }
            }

            Vector3 oiginScale = _originsScales[tr];
            tr.localScale = new Vector3(oiginScale.x + newScale, oiginScale.y + newScale, oiginScale.z + newScale);
        }

    }
}
