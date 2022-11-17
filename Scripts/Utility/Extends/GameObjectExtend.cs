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

        public static T[] GetComponentsInParent<T>(this GameObject @this, bool excludeMySelf, bool includeInactive)
        {
            if (@this == null)
            {
                return null;
            }

            var components = @this.GetComponentsInParent<T>(includeInactive);

            if (components == null)
            {
                return null;
            }

            var list = components.ToList();
            var ownerComponents = @this.GetComponents<T>();

            if (excludeMySelf && ownerComponents != null)
            {
                list.Remove(ownerComponents);
            }

            return list.ToArray();
        }

        public static T FindGameObjectWithTag<T>(string tag)
        {
            var obj = GameObject.FindGameObjectWithTag(tag);
            if (obj != null)
            {
                return obj.GetComponent<T>();
            }
            return default;
        }

        public static void DestroyOnLoad(GameObject obj)
        {
            if (obj != null && obj.transform.parent == null)
            {
                SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
            }
        }


        #region Equals
        public static bool IsEqual(GameObject aux1, GameObject aux2, StringTypeControl tagType)
        {
            if (aux1 != null && aux2 != null)
            {
                if (tagType == StringTypeControl.Name)
                {
                    return aux2.name == aux1.name;
                }
                if (tagType == StringTypeControl.Layer)
                {
                    return aux2.layer == aux1.layer;
                }
                if (tagType == StringTypeControl.Tag)
                {
                    return aux1.CompareTag(aux2.tag);
                }
                if (tagType == StringTypeControl.Tags)
                {
                    return aux1.GetTags().SequenceEqual(aux2.GetTags());
                }

                return aux2.layer == aux1.layer;
            }

            return false;
        }

        #endregion

        #region Find
        public static T Find<T>(in string name) where T : Component
        {
            GameObject obj = GameObject.Find(name);
            return obj != null ? obj.GetComponent<T>() : null;
        }

        public static T Find<T>(in string name, in Type type) where T : Component
        {
            GameObject obj = GameObject.Find(name);
            return obj != null ? (T)obj.GetComponent(type) : null;
        }

        public static GameObject Find(StringTypeControl tagType, string[] keys, bool isEquals = false)
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
                result = MultiTagsManager.FindGameObjectWithMultiTags(isEquals, keys);
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

        public static T Find<T>(StringTypeControl tagType, string[] keys, bool isEquals = false) where T : Component
        {
            GameObject obj = Find(tagType, keys, isEquals);
            return obj != null ? obj.GetComponent<T>() : default;
        }

        public static T Find<T>(StringTypeControl tagType, Type type, string[] keys) where T : Component
        {
            GameObject obj = Find(tagType, keys);
            return obj != null ? (T)obj.GetComponent(type) : null;
        }

        public static T FindWithType<T>(string typeString, string assembly = null) where T : Component
        {
            Type type = ReflectionExtend.GetType(typeString, assembly);
            return (T)GameObject.FindObjectOfType(type);
        }

        public static T Find<T>(StringTypeControl tagType, string typeString, string[] keys, string assembly = null) where T : Component
        {
            Type type = ReflectionExtend.GetType(typeString, assembly);
            GameObject obj = Find(tagType, keys);
            return obj != null && type != null ? (T)obj.GetComponent(type) : null;
        }

        public static GameObject FindInHierarchy(StringTypeControl tagType, string[] keys, Transform root, bool includeInactive, bool isEquals = false)
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
                    if (tr.HasTags(isEquals, keys))
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

        public static T FindInHierarchy<T>(in string rootString, in string gameobjectString) where T : Component
        {
            GameObject obj = FindInHierarchy(rootString, gameobjectString);
            return obj != null ? obj.GetComponent<T>() : null;
        }

        public static GameObject FindInHierarchy(in string rootString, in string gameObjectString)
        {
            GameObject rootObj = GameObject.Find(rootString);
            if (rootObj)
            {
                return TransformExtend.GetChildInHierarchy(rootObj, gameObjectString);
            }
            return null;
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

        public static T[] FindInterfaces<T>()
        {
            List<T> interfaces = new();
            GameObject[] rootGameObjects = GameObject.FindObjectsOfType<GameObject>();

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
        /// <summary>
        /// Destroy gameobject (if it's a pool manager, it behaves accordingly)
        /// </summary>
        /// <param name = "obj"> The object that will destroyed</param>
        public static void DestroyGameObject(this GameObject obj, bool immediate = false)
        {
            if (obj != null)
            {
                if (obj.HasTags(false, PoolManager.elementPool))
                {
                    PoolManager.Remove(obj);
                }
                else
                {
                    if (immediate || !Application.isPlaying)
                    {
                        GameObject.DestroyImmediate(obj);
                    }
                    else
                    {
                        GameObject.Destroy(obj);
                    }
                }
            }
        }

        public static void DestroyComponent<T>(T component) where T : Component
        {
            GameObject.Destroy(component);
        }

        public static void DestroyComponents<T>(ICollection<T> components) where T : Component
        {
            foreach (var component in components)
            {
                DestroyComponent<T>(component);
            }
        }

        public static void DestroyGameObject<T>(T component) where T : Component
        {
            DestroyGameObject(component.gameObject);
        }

        public static void DestroyGameObject<T>(ICollection<T> components) where T : Component
        {
            foreach (var component in components)
            {
                DestroyGameObject<T>(component);
            }
        }

        public static void DestroyGameObject(ICollection<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                DestroyGameObject(gameObject);
            }
        }
        #endregion

        #region Extend Methods
        public static void CreateGameObjectFromFolder(in string name, out GameObject reference, Transform parent = null)
        {
            GameObject prefab = AssetManager.LoadGameObjectFromPath(name);
            CreateGameObject(prefab, out reference, parent);
        }

        public static void CreateOnceFromBindings(string tagObject, in string name, out GameObject reference)
        {
            GameObject newObject = GameObjectExtend.FindGameObjectWithMultiTags(true, tagObject);
            if (newObject == null)
            {
                CreateGameObjectFromBindings(name, out reference);
            }
            else
            {
                reference = newObject;
            }
        }

        public static void CreateOnceFromBindings<T>(in string name, out GameObject reference) where T : Component
        {
            T componentInGame = GameObject.FindObjectOfType<T>();
            if (componentInGame == null)
            {
                CreateGameObjectFromBindings(name, out reference);
            }
            else
            {
                reference = componentInGame.gameObject;
            }
        }

        public static void CreateGameObjectFromBindings<T>(in string path, out T reference, Transform parent = null) where T : Component
        {
            GameObject prefab = AssetManager.LoadPrefab(path);
            CreateGameObject<T>(prefab, out reference, parent);
        }

        public static bool CreateGameObjectFromBindings<T>(in string path, out T reference, string name, bool dontDestroyAtLoad, Transform parent = null) where T : Component
        {
            GameObject prefab = AssetManager.LoadPrefab(path);
            prefab.name = name;
            bool result = CreateGameObject<T>(prefab, out reference, parent);
            if (reference != null && dontDestroyAtLoad)
            {
                GameObject.DontDestroyOnLoad(reference);
            }
            return result;
        }

        public static bool CreateGameObjectFromBindings(in string path, out GameObject reference, string name, Transform parent = null)
        {
            GameObject prefab = AssetManager.LoadPrefab(path);
            prefab.name = name;
            return CreateGameObject(prefab, out reference, parent);
        }

        public static void CreateGameObjectFromBindings(in string name, out GameObject reference, Transform parent = null, ChangeTypeEnum changeEnum = ChangeTypeEnum.Modify, bool solo = false)
        {
            GameObject prefab = AssetManager.LoadPrefab(name);
            CreateGameObject(prefab, out reference, parent, changeEnum, solo);
        }

        public static void CreateUIEElementsFromBindings(in string name, out GameObject reference, CanvasTipology tipology = CanvasTipology.Null)
        {
            CreateGameObjectFromBindings(name, out reference);

            CanvasManager.Paste(reference.transform, tipology);

            if (reference)
            {
                RectTransform rectTransform = reference.GetComponent<RectTransform>();
                if (rectTransform)
                {
                    rectTransform.anchoredPosition = Vector2.zero;
                }
            }
        }

        public static void CreateUIEElementsFromBindings<T>(in string name, out T reference, CanvasTipology tipology = CanvasTipology.Null) where T : Component
        {
            reference = null;
            CreateUIEElementsFromBindings(name, out GameObject newElement, tipology);
            if (newElement)
            {
                reference = newElement.GetComponentInChildren<T>();
            }
        }

        public static void CreateUIEElementsFromBindings(in string name, out GameObject reference, Transform parent)
        {
            CreateGameObjectFromBindings(name, out reference, parent);
            if (reference)
            {
                RectTransform rectTransform = reference.GetComponent<RectTransform>();
                if (rectTransform)
                {
                    rectTransform.anchoredPosition = Vector2.zero;
                }
            }
        }

        public static void CreateUIEElementsFromBindings<T>(in string name, out T reference, Transform parent) where T : Component
        {
            CreateUIEElementsFromBindings(name, out GameObject newElement, parent);
            reference = null;
            if (newElement)
            {
                reference = newElement.GetComponentInChildren<T>();
            }
        }

        #region Create GameObjct

        #region Create Without prefab
        public static T CreateGameObject<T>(in string name, bool dontDestoryAtLoad = false, params Type[] components) where T : Component
        {
            return CreateGameObject<T>(name, dontDestoryAtLoad, null, components);
        }

        public static T CreateGameObject<T>(in string name, bool dontDestoryAtLoad, Transform parent, params Type[] components) where T : Component
        {
            GameObject obj = new(name, components);
            obj.transform.SetParent(parent);

            if (dontDestoryAtLoad)
            {
                GameObject.DontDestroyOnLoad(obj);
            }

            return obj != null ? obj.AddOnlyOneComponent<T>() : default;
        }

        public static GameObject CreateGameObject(string name, Transform parent, bool dontDestoryAtLoad = false, params Type[] components)
        {
            ParentStruct parentStruct = new(parent);
            return CreateGameObject(name, parentStruct, dontDestoryAtLoad, components);
        }

        public static GameObject CreateGameObject<T>(string name, Transform parent, bool dontDestoryAtLoad = false)
        {
            ParentStruct parentStruct = new(parent);
            return CreateGameObject(name, parentStruct, dontDestoryAtLoad, typeof(T));
        }

        public static GameObject CreateGameObject<T>(string name, ParentStruct parentStruct, bool dontDestoryAtLoad = false)
        {
            return CreateGameObject(name, parentStruct, dontDestoryAtLoad, typeof(T));
        }

        public static GameObject CreateGameObject<T, F>(string name, Transform parent, bool dontDestoryAtLoad = false)
        {
            ParentStruct parentStruct = new(parent);
            return CreateGameObject(name, parentStruct, dontDestoryAtLoad, typeof(T), typeof(F));
        }

        public static GameObject CreateGameObject<T, F>(string name, ParentStruct parentStruct, bool dontDestoryAtLoad = false)
        {
            return CreateGameObject(name, parentStruct, dontDestoryAtLoad, typeof(T), typeof(F));
        }

        public static GameObject CreateGameObject(string name, ParentStruct parent, bool dontDestoryAtLoad = false, params Type[] components)
        {
            var list = components.FilterArray((x) => x != typeof(Transform));

            GameObject obj = new(name, list);
            if (obj)
            {
                if (dontDestoryAtLoad)
                {
                    GameObject.DontDestroyOnLoad(obj);
                }
                obj.transform.SetParent(parent);
                return obj;
            }
            return null;
        }


        public static T CreateGameObjectWithComponent<T>(string name, Transform parent, bool dontDestoryAtLoad = false, params Type[] components) where T : Component
        {
            GameObject obj = CreateGameObject(name, parent, dontDestoryAtLoad, components);
            return obj != null ? obj.GetComponent<T>() : null;
        }

        public static T CreateGameObjectWithComponent<T>(string name, ParentStruct parent, bool dontDestoryAtLoad = false) where T : Component
        {
            GameObject obj = CreateGameObject(name, parent, dontDestoryAtLoad, typeof(T));
            return obj != null ? obj.GetComponent<T>() : null;
        }

        public static T CreateGameObjectWithComponent<T>(string name, Transform parent = null, bool dontDestoryAtLoad = false) where T : Component
        {
            GameObject obj = CreateGameObject(name, parent, dontDestoryAtLoad, typeof(T));
            return obj != null ? obj.GetComponent<T>() : null;
        }

        #endregion


        public static GameObject CreateGameObject(GameObject prefab)
        {
            CreateGameObject(prefab, out GameObject result);
            return result;
        }

        public static GameObject CreateGameObject(GameObject prefab, Transform parent)
        {
            CreateGameObject(prefab, out GameObject result, parent);
            return result;
        }

        public static bool CreateGameObject<T>(GameObject prefab, out T reference, Transform parent, bool solo = false) where T : Component
        {
            return CreateGameObject<T>(prefab, out reference, null, Vector3.zero, Quaternion.identity, parent, ChangeTypeEnum.Modify, solo);
        }

        public static bool CreateGameObject<T>(GameObject prefab, out T reference, bool solo = false) where T : Component
        {
            return CreateGameObject<T>(prefab, out reference, null, Vector3.zero, Quaternion.identity, null, ChangeTypeEnum.Modify, solo);
        }

        public static bool CreateGameObject<T>(GameObject prefab, out T reference, Vector3 position, bool solo = false) where T : Component
        {
            return CreateGameObject<T>(prefab, out reference, null, position, Quaternion.identity, null, ChangeTypeEnum.Modify, solo);
        }

        public static bool CreateGameObject<T>(GameObject prefab, out T reference, string name, bool solo = false) where T : Component
        {
            return CreateGameObject<T>(prefab, out reference, name, Vector3.zero, Quaternion.identity, null, ChangeTypeEnum.Modify, solo);
        }

        public static bool CreateGameObject<T>(GameObject prefab, out T reference, Vector3 position, Transform parent, bool solo = false) where T : Component
        {
            return CreateGameObject<T>(prefab, out reference, null, position, Quaternion.identity, parent, ChangeTypeEnum.Setting, solo);
        }

        public static bool CreateGameObject(GameObject prefab, out GameObject reference, bool solo = false)
        {
            return CreateGameObject(prefab, out reference, null, Vector3.zero, Quaternion.identity, null, ChangeTypeEnum.Modify, solo);
        }

        public static bool CreateGameObject(GameObject prefab, out GameObject reference, Transform parent, bool solo = false)
        {
            CreateGameObject(prefab, out reference, null, Vector3.zero, Quaternion.identity, parent, ChangeTypeEnum.Modify, solo);
            return reference != null;
        }

        public static bool CreateGameObject(GameObject prefab, out GameObject reference, Vector3 position, bool solo = false)
        {
            return CreateGameObject(prefab, out reference, null, position, Quaternion.identity, null, ChangeTypeEnum.Modify, solo);
        }

        public static bool CreateGameObject(GameObject prefab, out GameObject reference, Vector3 position, Transform parent, bool solo = false)
        {
            return CreateGameObject(prefab, out reference, null, position, Quaternion.identity, parent, ChangeTypeEnum.Setting, solo);
        }

        public static bool CreateGameObject(GameObject prefab, out GameObject reference, string name, bool solo = false)
        {
            return CreateGameObject(prefab, out reference, name, Vector3.zero, Quaternion.identity, null, ChangeTypeEnum.Modify, solo);
        }

        public static bool CreateGameObject<T>(GameObject prefab, out T reference, Transform parent, ChangeTypeEnum changeEnum, bool solo = false) where T : Component
        {
            return CreateGameObject<T>(prefab, out reference, null, Vector3.zero, Quaternion.identity, parent, changeEnum, solo);
        }

        public static bool CreateGameObject<T>(GameObject prefab, out T reference, Vector3 position, Transform parent, ChangeTypeEnum changeEnum, bool solo = false) where T : Component
        {
            return CreateGameObject<T>(prefab, out reference, null, position, Quaternion.identity, parent, changeEnum, solo);
        }

        public static bool CreateGameObject(GameObject prefab, out GameObject reference, Transform parent, ChangeTypeEnum changeEnum, bool solo = false)
        {
            return CreateGameObject(prefab, out reference, null, Vector3.zero, Quaternion.identity, parent, changeEnum, solo);
        }

        public static bool CreateGameObject(GameObject prefab, out GameObject reference, Vector3 position, Transform parent, ChangeTypeEnum changeEnum, bool solo = false)
        {
            return CreateGameObject(prefab, out reference, null, position, Quaternion.identity, parent, changeEnum, solo);
        }

        public static bool CreateGameObject<T>(GameObject prefab, out T reference, string name, Vector3 position, Quaternion rotation, Transform parent = null, ChangeTypeEnum changeEnum = ChangeTypeEnum.Modify, bool solo = false) where T : Component
        {
            reference = null;
            if (CreateGameObject(prefab, out GameObject newElement, name, position, rotation, parent, changeEnum, solo))
            {
                reference = newElement.GetComponentInChildren<T>();
            }

            return reference != null;
        }

        public static bool CreateGameObject(GameObject prefab, out GameObject reference, string name, Vector3 position, Quaternion rotation, Transform parent = null, ChangeTypeEnum changeEnum = ChangeTypeEnum.Modify, bool solo = false)
        {
            if (prefab)
            {
                if (solo)
                {
                    GameObject aux = GameObject.Find(prefab.name);
                    if (aux != null)
                    {
                        reference = null;
                        return false;
                    }
                }

                if (parent != null && changeEnum == ChangeTypeEnum.Modify)
                {
                    position += parent.position;
                }

                reference = (GameObject)GameObject.Instantiate(prefab, position, rotation, parent);
                reference.name = name ?? reference.name.Split('(')[0];
            }
            else
            {
                reference = null;
            }

            return reference != null;
        }

        #endregion

        public static void CreateUIlement(GameObject prefab, out GameObject reference, CanvasTipology tipology = CanvasTipology.Null, ChangeTypeEnum changeEnum = ChangeTypeEnum.Modify, bool solo = false)
        {
            CreateGameObject(prefab, out reference, null, changeEnum, solo);
            if (reference)
            {
                CanvasManager.Paste(reference.transform, tipology);

                RectTransform rectTransform = reference.GetComponent<RectTransform>();
                if (rectTransform)
                {
                    rectTransform.anchoredPosition = Vector2.zero;
                }
            }
        }

        public static void CreateUIlement<T>(GameObject prefab, out T reference, CanvasTipology tipology = CanvasTipology.Null, ChangeTypeEnum changeEnum = ChangeTypeEnum.Modify, bool solo = false) where T : Component
        {
            reference = null;
            CreateUIlement(prefab, out GameObject newElement, tipology, changeEnum, solo);
            if (newElement)
            {
                reference = newElement.GetComponentInChildren<T>();
            }
        }

        public static void CreateUIlement(GameObject prefab, out GameObject reference, Transform parent = null, ChangeTypeEnum changeEnum = ChangeTypeEnum.Modify, bool solo = false)
        {
            CreateGameObject(prefab, out reference, parent, changeEnum, solo);

            if (reference)
            {
                RectTransform rectTransform = reference.GetComponent<RectTransform>();
                if (rectTransform)
                {
                    rectTransform.anchoredPosition = Vector2.zero;
                }
            }
        }

        public static void CreateUIlement<T>(GameObject prefab, out T reference, Transform parent = null, ChangeTypeEnum changeEnum = ChangeTypeEnum.Modify, bool solo = false) where T : Component
        {
            reference = null;
            CreateUIlement(prefab, out GameObject newElement, parent, changeEnum, solo);
            if (newElement)
            {
                reference = newElement.GetComponentInChildren<T>();
            }
        }

        public static GameObject FindGameObjectWithMultiTags(bool only, params string[] tagsParameter)
        {
            return MultiTagsManager.FindGameObjectWithMultiTags(only, tagsParameter);
        }

        public static GameObject[] FindGameObjectsWithMultiTags(bool only, params string[] tagsParameter)
        {
            return MultiTagsManager.FindGameObjectsWithMultiTags(only, tagsParameter);
        }


        /// <summary>
        /// returns all the specific components in the scene except the the caller
        /// </summary>
        /// <param name = "obj">The object that have the components</param>
        public static T[] FindOtherObjectsOfType<T>(this GameObject obj) where T : MonoBehaviour
        {
            if (obj != null)
            {
                List<T> list = new(GameObject.FindObjectsOfType<T>());
                if (list != null)
                {
                    list.Remove(list.Find(x => x.gameObject.GetInstanceID() == obj.GetInstanceID()));
                    return list.ToArray();
                }
            }
            return null;
        }

        public static void CopyAllComponents(this GameObject obj, GameObject toCopy, params Type[] excludes)
        {
            foreach (var component in toCopy.GetAllComponents())
            {
                if (excludes != null && excludes.Contains(component.GetType()))
                {
                    continue;
                }

                Type currentType = component.GetType();
                Component newComponent;
                if (currentType == typeof(Transform))
                {
                    newComponent = obj.GetComponent<Transform>();
                }
                else
                {
                    newComponent = obj.AddComponent(currentType);
                }

                newComponent.GetCopyOf(component);
            }
        }
        #endregion
    }
}
