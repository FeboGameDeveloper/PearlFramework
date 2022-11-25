using Pearl.Debug;
using Pearl.Multitags;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public struct ParentStruct
    {
        public Transform parent;
        public WorldReference reference;

        public ParentStruct(Transform parent, WorldReference reference)
        {
            this.parent = parent;
            this.reference = reference;
        }

        public ParentStruct(Transform parent)
        {
            this.parent = parent;
            this.reference = WorldReference.World;
        }
    }

    /// <summary>
    /// A class that extends the Transform class
    /// </summary>
    public static class TransformExtend
    {
        #region Public Methods

        #region Translate

        #region SetTranslationInUpdate
        public static void SetTranslationInUpdate(this Transform @this, Vector3 translate, TimeType timeType = TimeType.Scaled, UpdateModes updateMode = UpdateModes.Update)
        {
            @this.Translate(GetTranslatonInUpdate(translate, timeType, updateMode));
        }

        public static void SetTranslationInUpdate(this Transform @this, out bool isFinish, Vector3 translate, Transform finalTransform, float neighbourhood = 0, TimeType timeType = TimeType.Scaled, UpdateModes updateMode = UpdateModes.Update)
        {
            translate = GetTranslatonInUpdate(translate, timeType, updateMode);
            @this.SetTranslation(out isFinish, translate, finalTransform, neighbourhood);
        }

        public static void SetTranslationInUpdate(this Transform @this, out bool isFinish, Vector3 translate, Vector3 finalPosition, float neighbourhood = 0, TimeType timeType = TimeType.Scaled, UpdateModes updateMode = UpdateModes.Update)
        {
            translate = GetTranslatonInUpdate(translate, timeType, updateMode);
            @this.SetTranslation(out isFinish, translate, finalPosition, neighbourhood);
        }

        public static void SetTranslationInUpdate(this Transform @this, out bool isFinish, out Vector3 intersection, Vector3 translate, Line line, float neighbourhood = 0, TimeType timeType = TimeType.Scaled, UpdateModes updateMode = UpdateModes.Update)
        {
            translate = GetTranslatonInUpdate(translate, timeType, updateMode);
            @this.SetTranslation(out isFinish, out intersection, translate, line, neighbourhood);
        }

        public static void SetTranslationInUpdate(this Transform @this, out bool isFinish, out Vector3 intersection, Vector3 translate, Plane plane, float neighbourhood = 0, TimeType timeType = TimeType.Scaled, UpdateModes updateMode = UpdateModes.Update)
        {
            translate = GetTranslatonInUpdate(translate, timeType, updateMode);
            @this.SetTranslation(out isFinish, out intersection, translate, plane, neighbourhood);
        }

        private static Vector3 GetTranslatonInUpdate(Vector3 translate, TimeType timeType = TimeType.Scaled, UpdateModes updateMode = UpdateModes.Update)
        {
            return translate * TimeExtend.GetDeltaTime(timeType, updateMode);
        }
        #endregion

        #region SetTranslation
        public static void SetTranslation(this Transform @this, out bool isFinish, Vector3 translate, Transform finalTransform, float neighbourhood = 0)
        {
            if (finalTransform != null)
            {
                @this.SetTranslation(out isFinish, translate, finalTransform.position, neighbourhood);
            }
            else
            {
                isFinish = false;
            }
        }

        public static void SetTranslation(this Transform @this, out bool isFinish, out Vector3 intersection, Vector3 translate, Line line, float neighbourhood = 0)
        {
            if (@this != null)
            {
                Line lineTranslation = new(@this.position, translate);
                Vector3 result = GeometryUtils.LineLineIntersection(out intersection, lineTranslation, line) ? intersection : line.point;
                @this.SetTranslation(out isFinish, translate, result, neighbourhood);
            }
            else
            {
                isFinish = false;
                intersection = Vector3.zero;
            }
        }

        public static void SetTranslation(this Transform @this, out bool isFinish, out Vector3 intersection, Vector3 translate, Plane plane, float neighbourhood = 0)
        {
            if (@this != null)
            {
                Line lineTranslation = new(@this.position, translate);
                Vector3 result = GeometryUtils.LinePlaneIntersection(out intersection, lineTranslation, plane) ? intersection : plane.planePoint;
                @this.SetTranslation(out isFinish, translate, result, neighbourhood);
            }
            else
            {
                isFinish = false;
                intersection = Vector3.zero;
            }
        }

        public static void SetTranslation(this Transform @this, out bool isFinish, Vector3 translate, Vector3 finalPosition, float neighbourhood = 0)
        {
            isFinish = false;
            if (@this != null)
            {
                Vector3 currentPoition = @this.position;
                Vector3 delta = finalPosition - currentPoition;

                if (delta.ApproxZero() || (delta.normalized == translate.normalized && delta.magnitude < translate.magnitude))
                {
                    isFinish = true;
                }
                else if (neighbourhood != 0)
                {
                    neighbourhood = Mathf.Abs(neighbourhood);
                    Vector3 futureDestination = currentPoition + translate;
                    if (Vector3.Distance(finalPosition, futureDestination) <= neighbourhood)
                    {
                        isFinish = true;
                    }
                }

                if (isFinish)
                {
                    @this.position = finalPosition;
                }
                else
                {
                    @this.Translate(translate);
                }
            }
        }
        #endregion

        #endregion

        #region EulerAngles

        public static void SetEulerAngles(this Transform @this, Vector3 newValue, ChangeTypeEnum changeTypeTransform, AxisCombined axisCombined = AxisCombined.XYZ)
        {
            if (@this)
            {
                Vector3 eulerAngles = @this.eulerAngles;
                @this.eulerAngles = Vector3Extend.ChangeVector(eulerAngles, newValue, changeTypeTransform, axisCombined);
            }
        }

        public static void SetXEulerAngles(this Transform @this, float xValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            if (@this)
            {
                Vector3 eulerAngles = @this.eulerAngles;
                eulerAngles = Vector3Extend.ChangeVector(eulerAngles, AxisCombined.X, xValue, changeTypeTransform, range);
                @this.eulerAngles = eulerAngles;
            }
        }

        public static void SetYEulerAngles(this Transform @this, float yValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            if (@this)
            {
                Vector3 eulerAngles = @this.eulerAngles;
                eulerAngles = Vector3Extend.ChangeVector(eulerAngles, AxisCombined.Y, yValue, changeTypeTransform, range);
                @this.eulerAngles = eulerAngles;
            }
        }

        public static void SetZEulerAngles(this Transform @this, float zValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            if (@this)
            {
                Vector3 eulerAngles = @this.eulerAngles;
                eulerAngles = Vector3Extend.ChangeVector(eulerAngles, AxisCombined.Z, zValue, changeTypeTransform, range);
                @this.eulerAngles = eulerAngles;
            }
        }

        #endregion

        public static bool IsAlmostOneElement(this Transform @this)
        {
            return @this != null && @this.childCount > 0;
        }

        public static void SetSibilling(this Transform @this, TypeSibilling typeSibilling, int position = 0)
        {
            if (@this == null)
            {
                return;
            }

            if (typeSibilling == TypeSibilling.First)
            {
                @this.SetAsFirstSibling();
            }
            else if (typeSibilling == TypeSibilling.Last)
            {
                @this.SetAsLastSibling();
            }
            else
            {
                position = Math.Max(0, position);
                @this.SetSiblingIndex(position);
            }
        }

        public static void SetParent(this Transform @this, ParentStruct parentStruct)
        {
            SetParent(@this, parentStruct.parent, parentStruct.reference);
        }

        public static void SetParent(this Transform @this, Transform parent, WorldReference reference)
        {
            @this.SetParent(parent);

            if (reference == WorldReference.Local)
            {
                @this.localPosition = @this.position;
            }
        }

        public static TypeSibilling GetTypSybilling(this Transform @this, int index)
        {
            if (@this == null)
            {
                LogManager.LogWarning("The transform is null");
                return TypeSibilling.SpecificIndex;
            }

            if (index < 0 || index >= @this.childCount)
            {
                LogManager.LogWarning("The index is invalid");
                return TypeSibilling.SpecificIndex;
            }

            if (index == 0)
            {
                return TypeSibilling.First;
            }
            else if (index == @this.childCount - 1)
            {
                return TypeSibilling.Last;
            }
            else
            {
                return TypeSibilling.SpecificIndex;
            }
        }

        public static void DestroyAllChild(this Transform @this, bool immediate = false)
        {
            if (@this != null)
            {
                for (int i = @this.childCount - 1; i >= 0; i--)
                {
                    @this.DestroyChild(i, immediate);
                }
            }
        }

        public static void DestroyChild(this Transform @this, int index, bool immediate = false)
        {
            if (@this != null && @this.childCount > index)
            {
                GameObjectExtend.DestroyExtend(@this.GetChild(index).gameObject, immediate);
            }
        }

        public static Transform[] GetChildren(this Transform @this, bool includeYourSelf, params string[] tags)
        {
            return @this?.GetChildren((x) => x.HasTags(false, tags), includeYourSelf);
        }

        public static Transform GetChild(this Transform @this, bool includeYourSelf, params string[] tags)
        {
            var result = @this?.GetChildren((x) => x.HasTags(false, tags), includeYourSelf);

            return result.IsAlmostSpecificCount() ? result[0] : null;
        }

        public static T GetChild<T>(this Transform @this, bool includeYourSelf, params string[] tags)
        {
            var result = @this?.GetChildren((x) => x.HasTags(false, tags), includeYourSelf);

            return result.IsAlmostSpecificCount() ? result[0].GetComponent<T>() : default;
        }

        public static Transform[] GetChildren(this Transform @this, bool includeYourSelf = false)
        {
            List<Transform> result = new();

            if (includeYourSelf)
            {
                result.Add(@this);
            }

            if (@this != null)
            {
                foreach (Transform tr in @this)
                {
                    result.Add(tr);
                }
            }

            return result.ToArray();
        }

        public static Transform[] GetChildren(this Transform @this, Func<Transform, bool> filler, bool includeYourSelf = false)
        {
            List<Transform> result = new();
            Transform[] children = @this.GetChildren(includeYourSelf);


            if (children != null && filler != null)
            {
                foreach (Transform tr in children)
                {
                    if (filler.Invoke(tr))
                    {
                        result.Add(tr);
                    }
                }
            }
            return result.ToArray();
        }

        public static Transform GetChild(this Transform @this, Func<Transform, bool> filler, bool includeYourSelf = false)
        {
            var array = GetChildren(@this, filler, includeYourSelf);
            return array.IsAlmostSpecificCount() ? array[0] : null;
        }


        /// <summary>
        /// Returns the number that identifies the specific transform-child (if he is not a son of anyone, he returns -1)
        /// </summary>
        /// <param name = "this"> The specific component transform</param>
        public static int GetNumberChild(this Transform @this)
        {
            if (@this == null)
            {
                return -1;
            }

            Transform parent = @this.parent;
            if (@this && parent)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    if (parent.GetChild(i).Equals(@this))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public static void SetRotation2D(this Transform @this, Vector2 direction, float initAngle = 0)
        {
            if (@this == null)
            {
                return;
            }

            float aux = QuaternionExtend.CalculateAngle2D(direction.normalized, initAngle);
            Vector3 angles = @this.eulerAngles;
            angles.z = aux;
            @this.eulerAngles = angles;
        }

        public static int GetNumberChild(this GameObject @this)
        {
            if (@this != null)
            {
                return GetNumberChild(@this.transform);
            }
            return -1;
        }

        public static bool GetChild(this Transform @this, int index, out Transform result)
        {
            result = null;
            if (@this && index < @this.childCount)
            {
                result = @this.GetChild(index);
                return result != null;
            }
            return false;
        }

        public static Transform[] GetActiveChildren(this Transform @this)
        {
            if (@this == null)
            {
                return null;
            }

            return @this.GetChildren((x) => x.gameObject.activeSelf);
        }

        #region Scale 
        public static void SetScale(this Transform @this, Vector3 newValue, ChangeTypeEnum changeTypeTransform, AxisCombined axisCombined = AxisCombined.XYZ)
        {
            if (@this)
            {
                Vector3 position = @this.localScale;
                @this.localScale = Vector3Extend.ChangeVector(position, newValue, changeTypeTransform, axisCombined);
            }
        }

        public static void SetXScale(this Transform @this, float xScale, ChangeTypeEnum changeTypeTransform = ChangeTypeEnum.Setting)
        {
            if (@this)
            {
                Vector3 scale = @this.localScale;
                scale = Vector3Extend.ChangeVector(scale, AxisCombined.X, xScale, changeTypeTransform);
                @this.localScale = scale;
            }
        }

        public static void SetYScale(this Transform @this, float yScale, ChangeTypeEnum changeTypeTransform)
        {
            if (@this)
            {
                Vector3 scale = @this.localScale;
                scale = Vector3Extend.ChangeVector(scale, AxisCombined.Y, yScale, changeTypeTransform);
                @this.localScale = scale;
            }
        }

        public static void SetZScale(this Transform @this, float zScale, ChangeTypeEnum changeTypeTransform)
        {
            if (@this)
            {
                Vector3 scale = @this.localScale;
                scale = Vector3Extend.ChangeVector(scale, AxisCombined.Z, zScale, changeTypeTransform);
                @this.localScale = scale;
            }
        }
        #endregion

        #region Position
        public static void SetPosition(this Transform @this, Vector3 newValue, ChangeTypeEnum changeTypeTransform, AxisCombined axisCombined = AxisCombined.XYZ)
        {
            if (@this)
            {
                Vector3 position = @this.position;
                @this.position = Vector3Extend.ChangeVector(position, newValue, changeTypeTransform, axisCombined);
            }
        }

        public static void SetXPosition(this Transform @this, float xValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            if (@this)
            {
                Vector3 position = @this.position;
                position = Vector3Extend.ChangeVector(position, AxisCombined.X, xValue, changeTypeTransform, range);
                @this.position = position;
            }
        }

        public static void SetYPosition(this Transform @this, float yValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            if (@this)
            {
                Vector3 position = @this.position;
                position = Vector3Extend.ChangeVector(position, AxisCombined.Y, yValue, changeTypeTransform, range);
                @this.position = position;
            }
        }

        public static void SetZPosition(this Transform @this, float zValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            if (@this)
            {
                Vector3 position = @this.position;
                position = Vector3Extend.ChangeVector(position, AxisCombined.Z, zValue, changeTypeTransform, range);
                @this.position = position;
            }
        }

        #endregion

        #region Rotation
        public static void SetXRotation(this Transform @this, float angleX, ChangeTypeEnum changeTypeTransform)
        {
            if (@this)
            {
                float oldAngle = 0;
                if (changeTypeTransform == ChangeTypeEnum.Setting)
                {
                    oldAngle = @this.localEulerAngles.z;
                }
                @this.Rotate(Vector3.right * (angleX - oldAngle));
            }
        }

        public static void SetYRotation(this Transform @this, float angleY, ChangeTypeEnum changeTypeTransform)
        {
            if (@this)
            {
                float oldAngle = 0;
                if (changeTypeTransform == ChangeTypeEnum.Setting)
                {
                    oldAngle = @this.localEulerAngles.z;
                }
                @this.Rotate(Vector3.up * (angleY - oldAngle));
            }
        }

        public static void SetZRotation(this Transform @this, float angleZ, ChangeTypeEnum changeTypeTransform)
        {
            if (@this)
            {
                float oldAngle = 0;
                if (changeTypeTransform == ChangeTypeEnum.Setting)
                {
                    oldAngle = @this.localEulerAngles.z;
                }
                @this.Rotate(Vector3.forward * (angleZ - oldAngle));
            }
        }
        #endregion

        public static T GetComponentInBorthers<T>(this Transform @this) where T : Component
        {
            if (@this)
            {
                Transform parent = @this.parent;
                if (parent)
                {
                    return parent.GetComponentInChildren<T>();
                }
            }
            return default;
        }

        public static T GetChild<T>(this Transform @this, int index) where T : Component
        {
            if (@this && @this.childCount > index)
            {
                var child = @this.GetChild(index);
                if (child && child.TryGetComponent<T>(out T component))
                {
                    return component;
                }
            }
            return null;
        }

        #region Hierarchy
        public static T GetChildInHierarchy<T>(this GameObject @this, in string name, bool onlyChildren = false)
        {
            var result = GetChildInHierarchy(@this, name, onlyChildren);
            if (result != null)
            {
                return result.GetComponent<T>();
            }
            return default;
        }

        public static GameObject GetChildInHierarchy(this GameObject @this, in string name, bool onlyChildren = false)
        {
            if (@this == null)
            {
                return null;
            }

            Transform tr = GetChildInHierarchy(@this.transform, name, onlyChildren);

            if (tr == null)
            {
                return null;
            }
            return tr.gameObject;
        }

        public static Transform GetChildInHierarchy(this Transform @this, string name, bool onlyChildren = false)
        {
            if (@this == null)
            {
                return null;
            }

            List<Transform> result = @this.GetComponentsInHierarchy<Transform>(onlyChildren);
            return result.Find(x => x.name.EqualsIgnoreCase(name));
        }

        public static T GetChildInHierarchy<T>(this GameObject @this, bool onlyChildren = false)
        {
            if (@this == null)
            {
                return default;
            }

            return GetChildInHierarchy<T>(@this.transform, onlyChildren);
        }

        public static T GetChildInHierarchy<T>(this Transform @this, bool onlyChildren = false)
        {
            List<T> result = @this.GetComponentsInHierarchy<T>(onlyChildren);
            if (result.IsAlmostSpecificCount())
            {
                return result[0];
            }
            return default;
        }

        public static Component GetChildInHierarchy(this GameObject @this, Type type, bool onlyChildren = false)
        {
            if (@this != null)
            {
                return @this.transform.GetChildInHierarchy(type, onlyChildren);
            }
            return null;
        }

        public static Component GetChildInHierarchy(this Transform @this, Type type, bool onlyChildren = false, bool includeInactive = true)
        {
            if (type != null)
            {
                Transform[] result = @this.GetComponentsInChildren<Transform>(onlyChildren, includeInactive);

                if (result.IsAlmostSpecificCount())
                {
                    foreach (var tr in result)
                    {
                        Component component = tr.GetComponent(type);
                        if (component != null)
                        {
                            return component;
                        }
                    }
                }
            }

            return default;
        }

        public static void GetChildrenInHierarchy<T>(this GameObject @this, out T[] result, bool onlyChildren = false, bool includeInactive = true)
        {
            result = null;

            List<T> list = @this.GetChildrenInHierarchy<T>(onlyChildren, includeInactive);

            if (list != null)
            {
                result = list.ToArray();
            }
        }

        public static List<T> GetChildrenInHierarchy<T>(this GameObject @this, bool onlyChildren = false, bool includeInactive = true)
        {
            if (@this == null)
            {
                return null;
            }

            return @this.transform.GetComponentsInHierarchy<T>(onlyChildren, includeInactive);
        }

        public static List<T> GetComponentsInHierarchy<T>(this Transform @this, bool onlyChildren = false, bool includeInactive = true)
        {
            var transforms = @this.GetComponentsInChildren<Transform>(onlyChildren, includeInactive);

            List<T> result = new List<T>();

            bool isGameObject = typeof(T) == typeof(GameObject);

            foreach (var tr in transforms)
            {
                if (isGameObject)
                {
                    result.Add((T)(object)tr.gameObject);
                }
                else
                {
                    if (tr.TryGetComponent<T>(out T component))
                    {
                        result.Add(component);
                    }
                }
            }

            return result;
        }
        #endregion

        public static GameObject[] GetChildrenObj(this Transform transform)
        {
            GameObject[] result = null;

            if (transform)
            {
                result = new GameObject[transform.childCount];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = transform.GetChild(i).gameObject;
                }
            }

            return result;
        }

        public static List<string> ReturnTags(this Transform value)
        {
            if (value != null)
            {
                return MultiTagsManager.GetTags(value.gameObject);
            }

            return null;
        }

        public static bool HasTags(this Transform value, bool only, bool includeChildren, params string[] tagsParameter)
        {
            Transform[] result = FindTransformsWithTags(value, only, includeChildren, tagsParameter);
            return result.IsAlmostSpecificCount() ? true : false;
        }

        public static Transform FindTransformWithTags(this Transform value, bool only, bool includeChildren, params string[] tagsParameter)
        {
            Transform[] result = FindTransformsWithTags(value, only, includeChildren, tagsParameter);
            return result.IsAlmostSpecificCount() ? result[0] : null;

        }

        public static Transform[] FindTransformsWithTags(this Transform value, bool only, bool includeChildren, params string[] tagsParameter)
        {
            List<Transform> result = new List<Transform>();

            if (includeChildren)
            {
                List<MultitagsComponent> values = value.GetComponentsInHierarchy<MultitagsComponent>(false);

                foreach (var obj in values)
                {
                    if (obj.HasTags(only, tagsParameter))
                    {
                        result.Add(obj.transform);
                    }
                }
            }
            else if (value && value.gameObject.HasTags(only, tagsParameter))
            {
                result.Add(value);
            }

            return result.ToArray();
        }

        public static bool IsChildOfDirectly(this Transform value, Transform parent)
        {
            if (value && parent)
            {
                return value.parent == parent;
            }

            return false;
        }

        public static Transform GetChildDirectlyForParent(Transform value, Transform parent)
        {
            if (value && parent)
            {
                for (Transform aux = value; aux != null; aux = aux.parent)
                {
                    if (aux.IsChildOfDirectly(parent))
                    {
                        return aux;
                    }
                }
            }

            return null;
        }

        public static Transform Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            return transform;
        }
        #endregion
    }
}
