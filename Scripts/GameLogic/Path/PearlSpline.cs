#if SPLINE

using Pearl.Events;
using Pearl.UI;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Pearl.Paths
{
    public class PearlSpline : MonoBehaviour
    {
#region Events
        public event Action OnChangePath;
#endregion

#region Public fields

        [Header("General setting")]

        [SerializeField]
        private SplineContainer splineContainer = default;

        [SerializeField]
        [ConditionalField("!@isUI")]
        private bool isLerp = default;
        [SerializeField]
        [ConditionalField("@isLerp && !@isUI")]
        private float t = 0;
        [SerializeField]
        [ConditionalField("@isLerp && !@isUI")]
        private FunctionEnum functionCurve = default;

        [SerializeField]
        [ConditionalField("!@isLerp")]
        private bool isUI = default;
#pragma warning disable 169, 414
        [SerializeField]
        [ConditionalField("@isUI && !@isLerp")]
        [InspectorButton("OnChangeUIElementForUI")]
        private bool changeUIElement = default;
        [SerializeField]
        [ConditionalField("@isUI && !@isLerp")]
        [InspectorButton("OnChangeKnotForUI")]
        private bool changeKnot = default;
#pragma warning restore 169, 414

#endregion

#region Private fields
        private readonly List<List<BezierKnot>> _knotsForLerp = new();
#endregion

#region Properties
        public bool IsUI
        {
            get { return isUI; }
            set
            {
                if (isUI && !value)
                {
                    PearlEventsManager.RemoveAction(ConstantStrings.ChangeResolution, OnChangeUIElementForUI);
                }
                else if (!isUI && value)
                {
                    PearlEventsManager.AddAction(ConstantStrings.ChangeResolution, OnChangeUIElementForUI);
                    OnChangeUIElementForUI();
                }

                isUI = value;
            }
        }

        public SplineContainer Spline
        {
            set
            {
                splineContainer = value;
                SetMatrixKnotForLerp();
                OnChangeUIElementForUI();
            }
        }

        public float T
        {
            get { return t; }
            set
            {
                t = value;
                SetLerp();
            }
        }
#endregion

#region UnityCallbacks
        // Start is called before the first frame update
        protected void Awake()
        {
            if (isUI)
            {
                PearlEventsManager.AddAction(ConstantStrings.ChangeResolution, OnChangeUIElementForUI);
            }

            if (isLerp)
            {
                SetMatrixKnotForLerp();
                SetLerp();
                float change = splineContainer.CalculateLength();
            }
        }

        protected void Reset()
        {
            splineContainer = GetComponent<SplineContainer>();
        }

        protected void OnValidate()
        {
            if (isLerp)
            {
                SetMatrixKnotForLerp();
            }
        }


        // Update is called once per frame
        protected void OnDestroy()
        {
            if (isUI)
            {
                PearlEventsManager.RemoveAction(ConstantStrings.ChangeResolution, OnChangeUIElementForUI);
            }
        }
#endregion

#region Public Methods
        public bool Evaluate(float t, out float3 position, out float3 tangent, out float3 upVector)
        {
            if (splineContainer == null)
            {
                position = default;
                tangent = default;
                upVector = default;
                return default;
            }

            return splineContainer.Evaluate(t, out position, out tangent, out upVector);
        }

        public float3 EvaluatePosition(float t)
        {
            if (splineContainer == null)
            {
                return default;
            }

            return splineContainer.EvaluatePosition(t);
        }

        public float3 EvaluateTangent(float t)
        {
            if (splineContainer == null)
            {
                return default;
            }

            return splineContainer.EvaluateTangent(t);
        }

        public float3 EvaluateUpVector(float t)
        {
            if (splineContainer == null)
            {
                return default;
            }

            return splineContainer.EvaluateUpVector(t);
        }

        public float3 EvaluateAcceleration(float t)
        {
            if (splineContainer == null)
            {
                return default;
            }

            return splineContainer.EvaluateAcceleration(t);
        }

        public float CalculateLength()
        {
            if (splineContainer == null)
            {
                return default;
            }

            return splineContainer.CalculateLength();
        }
#endregion

#region Private Methods
        private void OnChangeUIElementForUI()
        {
            if (!isUI && splineContainer == null)
            {
                return;
            }

            var spline = splineContainer.Spline;
            spline.Clear();
            for (int j = 0; j < transform.childCount; j += 3)
            {
                BezierKnot knot = new();
                var child = transform.GetChild(j);
                knot.Position = transform.InverseTransformPoint(child.position);
                knot.Rotation = child.rotation;
                knot.TangentIn = (float3)transform.InverseTransformPoint(transform.GetChild(j + 1).position) - knot.Position;
                knot.TangentOut = (float3)transform.InverseTransformPoint(transform.GetChild(j + 2).position) - knot.Position;
                spline.Add(knot);

                OnChangePath?.Invoke();
            }
        }

        private void OnChangeKnotForUI()
        {
            if (!isUI && splineContainer == null)
            {
                return;
            }

            var spline = splineContainer.Spline;
            int j = 0;
            for (int i = 0; i < spline.Count; i++)
            {
                var knot = spline[i];
                Transform child = transform.GetChild(j);
                child.position = transform.TransformPoint(knot.Position);
                child.rotation = knot.Rotation;
                child.AnchorsToCorners();

                child = transform.GetChild(j + 1);

                child.position = transform.TransformPoint(knot.Position + knot.TangentIn);
                child.AnchorsToCorners();

                child = transform.GetChild(j + 2);
                child.position = transform.TransformPoint(knot.Position + knot.TangentOut);
                child.AnchorsToCorners();

                j += 3;
            }

            OnChangeUIElementForUI();
        }

        private void SetMatrixKnotForLerp()
        {
            if (splineContainer == null || !isLerp)
            {
                return;
            }

            Spline spline;
            List<BezierKnot> currentList;
            _knotsForLerp.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<SplineContainer>(out var splineContainrChild))
                {
                    spline = splineContainrChild.Spline;
                    _knotsForLerp.Add(new List<BezierKnot>());
                    currentList = _knotsForLerp[i];
                    for (int j = 0; j < spline.Count; j++)
                    {
                        currentList.Add(spline[j]);
                    }
                }
            }

            SetLerp();
        }

        private void SetLerp()
        {
            if (splineContainer == null || _knotsForLerp == null || !isLerp)
            {
                return;
            }

            var spline = splineContainer.Spline;
            spline.Clear();

            t = Mathf.Clamp01(t);
            float r = t;
            r = FunctionDefinition.GetFunction(functionCurve).Invoke(t);
            r = Mathf.Clamp01(r);

            int keysNum = _knotsForLerp.Count;
            float delta = 1f / (keysNum - 1);
            int piece = r == 1 ? keysNum - 2 : Mathf.FloorToInt(r / delta);

            r = MathfExtend.Percent(t - (delta * piece), delta);

            List<BezierKnot> path1 = _knotsForLerp[piece];
            List<BezierKnot> path2 = _knotsForLerp[piece + 1];
            int length = Mathf.Max(path1.Count, path2.Count);

            for (int i = 0; i < length; i++)
            {
                BezierKnot resultKnot = new();

                BezierKnot knot1;
                BezierKnot knot2;

                if (path1.Count <= i)
                {
                    knot1 = knot2 = path2[i];
                }

                if (path2.Count <= i)
                {
                    knot1 = knot2 = path1[i];
                }
                else
                {
                    knot1 = path1[i];
                    knot2 = path2[i];
                }

                resultKnot.Position = Vector3.Lerp(knot1.Position, knot2.Position, r);
                resultKnot.Rotation = Quaternion.Lerp(knot1.Rotation, knot2.Rotation, r);
                resultKnot.TangentIn = Vector3.Lerp(knot1.TangentIn, knot2.TangentIn, r);
                resultKnot.TangentOut = Vector3.Lerp(knot1.TangentOut, knot2.TangentOut, r);

                spline.Add(resultKnot);
            }

            if (splineContainer != null)
            {
                OnChangePath?.Invoke();
            }
        }
#endregion
    }
}

#endif
