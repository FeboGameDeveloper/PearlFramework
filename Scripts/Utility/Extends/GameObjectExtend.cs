using Pearl.Multitags;
using Pearl.Pools;
using Pearl.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pearl
{
    /// <summary>
    /// A class that extends the gameobject class
    /// </summary>
    public static class GameObjectExtend
    {
        #region Get
        public static T[] GetComponentsInParent<T>(this GameObject @this, bool excludeMySelf, bool includeInactive) where T : Component
        {
            if (@this == null)
            {
                return null;
            }

            GameObject container = excludeMySelf ? @this.transform.parent.gameObject : @this;

            if (container == null)
            {
                return null;
            }

            return container.GetComponentsInParent<T>(includeInactive);
        }
        #endregion


        #region Equals
        public static bool IsEqual(this GameObject @this, GameObject aux, StringTypeControl tagType)
        {
            if (@this != null && aux != null)
            {
                if (tagType == StringTypeControl.Name)
                {
                    return aux.name == @this.name;
                }
                else if (tagType == StringTypeControl.Layer)
                {
                    return aux.layer == @this.layer;
                }
                else if (tagType == StringTypeControl.Tag)
                {
                    return @this.CompareTag(aux.tag);
                }
                else if (tagType == StringTypeControl.Tags)
                {
                    return @this.GetTags().SequenceEqual(aux.GetTags());
                }
            }

            return false;
        }

        #endregion

        #region Find
        public static T FindGameObjectWithTag<T>(string tag) where T : Component
        {
            var obj = GameObject.FindGameObjectWithTag(tag);
            return obj != null ? obj.GetComponent<T>() : null;
        }

        public static T[] FindOtherObjectsOfType<T>(this GameObject @this) where T : Component
        {
            if (@this != null)
            {
                List<T> list = new(GameObject.FindObjectsOfType<T>());
                if (list != null)
                {
                    list.Remove(list.Find(x => x.gameObject.GetInstanceID() == @this.GetInstanceID()));
                    return list.ToArray();
                }
            }
            return null;
        }

        public static T Find<T>(in string name) where T : Component
        {
            GameObject obj = GameObject.Find(name);
            return obj != null ? obj.GetComponent<T>() : null;
        }

        public static GameObject Find(StringTypeControl tagType, string key)
        {
            return Find(tagType, ArrayExtend.CreateArray(key), true);
        }

        public static GameObject Find(StringTypeControl tagType, string[] keys, bool onlyThoseTags = false)
        {
            GameObject result = null;
            if (tagType == StringTypeControl.Tag)
            {
                foreach (var key in keys)
                {
                    result = GameObject.FindGameObjectWithTag(key);
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            else if (tagType == StringTypeControl.Name)
            {
                foreach (var key in keys)
                {
                    result = GameObject.Find(key);
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            else if (tagType == StringTypeControl.Tags)
            {
                result = MultiTagsManager.FindGameObjectWithMultiTags(onlyThoseTags, keys);
            }
            else if (tagType == StringTypeControl.Layer)
            {
                var auxes = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (var aux in auxes)
                {
                    string layer = LayerMask.LayerToName(aux.layer);
                    foreach (var key in keys)
                    {
                        if (layer.EqualsIgnoreCase(key))
                        {
                            result = aux;
                            break;
                        }
                    }

                    if (result != null)
                    {
                        break;
                    }
                }


            }
            return result;
        }

        public static T Find<T>(StringTypeControl tagType, string key) where T : Component
        {
            GameObject obj = Find(tagType, key);
            return obj != null ? obj.GetComponent<T>() : default;
        }

        public static T Find<T>(StringTypeControl tagType, string[] keys, bool onlyThoseTags = false) where T : Component
        {
            GameObject obj = Find(tagType, keys, onlyThoseTags);
            return obj != null ? obj.GetComponent<T>() : default;
        }

        public static GameObject FindInHierarchy(StringTypeControl tagType, string[] keys, Transform root, bool includeInactive, bool onlyThoseTags = false)
        {
            if (root == null)
            {
                return null;
            }

            Transform[] childrenRoot = root.GetComponentsInChildren<Transform>(includeInactive);

            foreach (Transform tr in childrenRoot)
            {
                if (tagType == StringTypeControl.Tag)
                {
                    string tag = tr.tag;
                    foreach (var key in keys)
                    {
                        if (tag.EqualsIgnoreCase(key))
                        {
                            return tr.gameObject;
                        }
                    }
                }
                else if (tagType == StringTypeControl.Tags)
                {
                    if (tr.HasTags(onlyThoseTags, keys))
                    {
                        return tr.gameObject;
                    }
                }
                else if (tagType == StringTypeControl.Name)
                {
                    foreach (var key in keys)
                    {
                        if (tr.name.EqualsIgnoreCase(key))
                        {
                            return tr.gameObject;
                        }
                    }
                }
                else if (tagType == StringTypeControl.Layer)
                {
                    string layer = LayerMask.LayerToName(tr.gameObject.layer);
                    foreach (var key in keys)
                    {
                        if (layer.EqualsIgnoreCase(key))
                        {
                            return tr.gameObject;
                        }
                    }
                }
            }

            return null;
        }

        public static GameObject FindInHierarchy(StringTypeControl tagType, string key, Transform root, bool includeInactive)
        {
            return FindInHierarchy(tagType, ArrayExtend.CreateArray(key), root, includeInactive, true);
        }

        public static T FindInHierarchy<T>(StringTypeControl tagType, string[] keys, Transform root, bool includeInactive, bool onlyThoseTags = false) where T : Component
        {
            GameObject obj = FindInHierarchy(tagType, keys, root, includeInactive, onlyThoseTags);
            return obj != null ? obj.GetComponent<T>() : default;
        }

        public static T FindInHierarchy<T>(StringTypeControl tagType, string key, Transform root, bool includeInactive) where T : Component
        {
            GameObject obj = FindInHierarchy(tagType, key, root, includeInactive);
            return obj != null ? obj.GetComponent<T>() : default;
        }

        public static GameObject FindInHierarchy(in string rootString, in string gameObjectString)
        {
            GameObject rootObj = GameObject.Find(rootString);
            return TransformExtend.GetChildInHierarchy(rootObj, gameObjectString);
        }

        public static T FindInHierarchy<T>(in string rootString, in string gameObjectString) where T : Component
        {
            GameObject obj = FindInHierarchy(rootString, gameObjectString);
            return obj != null ? obj.GetComponent<T>() : null;
        }

        public static T FindInHierarchy<T>(in string rootString) where T : Component
        {
            GameObject rootObj = GameObject.Find(rootString);
            if (rootObj)
            {
                return TransformExtend.GetChildInHierarchy<T>(rootObj);
            }
            return null;
        }

        public static T[] FindAllInterfaces<T>(in bool includeInactive = false)
        {
            List<T> interfaces = new();
            GameObject[] rootGameObjects = GameObject.FindObjectsOfType<GameObject>(includeInactive);

            if (rootGameObjects != null)
            {
                foreach (var rootGameObject in rootGameObjects)
                {
                    T[] childrenInterfaces = rootGameObject.GetComponents<T>();
                    foreach (var childInterface in childrenInterfaces)
                    {
                        interfaces.Add(childInterface);
                    }
                }
            }
            return interfaces.ToArray();
        }
        #endregion

        #region Destroy
        public static void DestroyOnLoad(GameObject obj)
        {
            if (obj != null && obj.transform.parent == null)
            {
                SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
            }
        }

        /// <summary>
        /// Destroy gameobject (if it's a pool manager, it behaves accordingly)
        /// </summary>
        /// <param name = "this"> The object that will destroyed</param>
        public static void DestroyExtend(this UnityEngine.Object @this, bool immediate = false)
        {
            if (@this != null)
            {
                if (@this is GameObject obj && obj.HasTags(false, PoolManager.poolElement))
                {
                    PoolManager.Remove(obj);
                }
                else
                {
                    if (immediate || !Application.isPlaying)
                    {
                        GameObject.DestroyImmediate(@this);
                    }
                    else
                    {
                        GameObject.Destroy(@this);
                    }
                }
            }
        }
        #endregion

        #region Create GameObjct

        #region Create Without prefab
        public static GameObject CreateGameObject(string name, Transform parent = null, WorldReference parentReference = WorldReference.World, bool dontDestroyAtLoad = false, params Type[] components)
        {
            var list = components.FilterArray((x) => x != typeof(Transform));

            GameObject obj = new(name, list);
            if (obj)
            {
                if (dontDestroyAtLoad)
                {
                    GameObject.DontDestroyOnLoad(obj);
                }

                ParentStruct parentStruct = new(parent, parentReference);
                obj.transform.SetParent(parentStruct);
                return obj;
            }
            return null;
        }

        public static T CreateGameObject<T>(string name, Transform parent = null, WorldReference parentReference = WorldReference.World, bool dontDestroyAtLoad = false, params Type[] components) where T : Component
        {
            GameObject obj = CreateGameObject(name, parent, parentReference, dontDestroyAtLoad, components);

            return obj != null ? obj.AddOnlyOneComponent<T>() : default;
        }
        #endregion

        #region Create GameObject With prefab
        public static bool CreateGameObject<T>(GameObject prefab, out T reference, string name = null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, WorldReference parentReference = WorldReference.World, bool onlyInTheScene = false, bool dontDestroyAtLoad = false) where T : Component
        {
            CreateGameObject(prefab, out GameObject obj, name, position, rotation, parent, parentReference, onlyInTheScene);
            reference = null;
            if (obj != null)
            {
                reference = obj.AddOnlyOneComponent<T>();
                return reference != null;
            }
            return false;
        }

        public static bool CreateGameObject(GameObject prefab, out GameObject reference, string name = null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, WorldReference parentReference = WorldReference.World, bool onlyInTheScene = false, bool dontDestroyAtLoad = false)
        {
            if (prefab)
            {
                if (onlyInTheScene)
                {
                    GameObject aux = GameObject.Find(prefab.name);
                    if (aux != null)
                    {
                        reference = null;
                        return false;
                    }
                }

                reference = (GameObject)GameObject.Instantiate(prefab, position, rotation);
                if (reference)
                {
                    reference.name = name ?? reference.name.Split('(')[0];
                    ParentStruct parentStruct = new(parent, parentReference);
                    reference.transform.SetParent(parentStruct);
                    if (dontDestroyAtLoad)
                    {
                        GameObject.DontDestroyOnLoad(reference);
                    }
                }
            }
            else
            {
                reference = null;
            }

            return reference != null;
        }
        #endregion

        #region Create With Resources
        public static bool CreateGameObjectFromBindings<T>(in string path, out T reference, string name = null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, WorldReference parentReference = WorldReference.World, bool onlyInTheScene = false, bool dontDestroyAtLoad = false) where T : Component
        {
            CreateGameObjectFromBindings(path, out GameObject obj, name, position, rotation, parent, parentReference, onlyInTheScene);
            reference = null;
            if (obj != null)
            {
                reference = obj.AddOnlyOneComponent<T>();
                return reference != null;
            }
            return false;
        }

        public static bool CreateGameObjectFromBindings(in string path, out GameObject reference, string name = null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, WorldReference parentReference = WorldReference.World, bool onlyInTheScene = false, bool dontDestroyAtLoad = false)
        {
            GameObject prefab = AssetManager.LoadPrefab(path);
            return CreateGameObject(prefab, out reference, name, position, rotation, parent, parentReference, onlyInTheScene);
        }

        public static bool CreateUIEElementsFromBindings(in string path, out GameObject reference, string name = null, CanvasTipology canvasTipology = CanvasTipology.Null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, WorldReference parentReference = WorldReference.World, bool onlyInTheScene = false, bool dontDestroyAtLoad = false)
        {
            CreateGameObjectFromBindings(path, out reference, name, position, rotation, parent, parentReference, onlyInTheScene);
            if (reference != null)
            {
                CanvasPaste(reference.transform, canvasTipology);
                return true;
            }
            return false;
        }

        public static bool CreateUIEElementsFromBindings<T>(in string path, out T reference, string name = null, CanvasTipology canvasTipology = CanvasTipology.Null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, WorldReference parentReference = WorldReference.World, bool onlyInTheScene = false, bool dontDestroyAtLoad = false) where T : Component
        {
            CreateGameObjectFromBindings<T>(path, out reference, name, position, rotation, parent, parentReference, onlyInTheScene);
            if (reference != null)
            {
                CanvasPaste(reference.transform, canvasTipology);
                return true;
            }
            return false;
        }
        #endregion

        #region Create UI Element
        public static bool CreateUIlement(in GameObject prefab, out GameObject reference, string name = null, CanvasTipology canvasTipology = CanvasTipology.Null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, WorldReference parentReference = WorldReference.World, bool onlyInTheScene = false, bool dontDestroyAtLoad = false)
        {
            CreateGameObject(prefab, out reference, name, position, rotation, parent, parentReference, onlyInTheScene);
            if (reference != null)
            {
                CanvasPaste(reference.transform, canvasTipology);
                return true;
            }
            return false;
        }

        public static bool CreateUIlement<T>(in GameObject prefab, out T reference, string name = null, CanvasTipology canvasTipology = CanvasTipology.Null, Vector3 position = default, Quaternion rotation = default, Transform parent = null, WorldReference parentReference = WorldReference.World, bool onlyInTheScene = false, bool dontDestroyAtLoad = false) where T : Component
        {
            CreateGameObject<T>(prefab, out reference, name, position, rotation, parent, parentReference, onlyInTheScene);
            if (reference != null)
            {
                CanvasPaste(reference.transform, canvasTipology);
                return true;
            }
            return false;
        }
        #endregion

        #endregion

        #region Copy
        public static GameObject Copy(this GameObject @this, params Type[] excludes)
        {
            GameObject copied = new(@this.name);
            Copy(copied, @this, excludes);
            return copied;
        }


        public static void Copy(this GameObject @this, GameObject toCopy, params Type[] excludes)
        {
            foreach (var component in toCopy.GetAllComponents())
            {
                if (excludes != null && excludes.Contains(component.GetType()))
                {
                    continue;
                }

                Type currentType = component.GetType();
                Component newComponent;


                newComponent = currentType == typeof(Transform) ? @this.GetComponent<Transform>() : @this.AddComponent(currentType);
                newComponent.GetCopyOf(component);
            }
        }
        #endregion

        #region Private Methods
        private static void CanvasPaste(Transform transform, CanvasTipology canvasTipology = CanvasTipology.Null)
        {
            CanvasManager.Paste(transform, canvasTipology);

            if (transform != null)
            {
                RectTransform rectTransform = transform.GetComponent<RectTransform>();
                if (rectTransform)
                {
                    rectTransform.anchoredPosition = Vector2.zero;
                }
            }
        }
        #endregion
    }
}
