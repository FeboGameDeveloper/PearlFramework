using Pearl.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Debug
{
    public struct DebugVar
    {
        public MemberComplexInfo member;
        public string name;

        public DebugVar(string name, MemberComplexInfo member)
        {
            this.name = name;
            this.member = member;
        }
    }

    public class DebugScreenManager : PearlBehaviour, ISingleton
    {
        [SerializeField]
        private StringBoolDictionary debugView = null;

        [SerializeField]
        private GameObject debugScreenPrefab = null;

        [SerializeField]
        private GameObject debugScreenElementPrefab = null;

        private Transform _debugScreenParent;


        private static bool _isDebugScreen = false;

        public static bool IsDebugScreen
        {
            get { return _isDebugScreen; }
        }

        public static bool GetIstance(out DebugScreenManager result)
        {
            return Singleton<DebugScreenManager>.GetIstance(out result);
        }

        protected void OnValidate()
        {
            if (Application.isPlaying && _isDebugScreen)
            {
                CreateDebugScreen();
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            if (DebugManager.GetActiveDebug("debugScreen"))
            {
                GameObjectExtend.CreateUIlement(debugScreenPrefab, out _debugScreenParent, CanvasTipology.Debug);
                CreateDebugScreen();
                _isDebugScreen = true;
            }
        }

        public static void AddContainer(object container)
        {
            if (GetIstance(out var debugScreenManager))
            {
                debugScreenManager.AddContainerIntern(container);
            }
        }

        public static void RemoveContainer(object container)
        {
            if (GetIstance(out var debugScreenManager))
            {
                debugScreenManager.RemoveContainerIntern(container);
            }
        }

        public void AddContainerIntern(object container)
        {
            object[] containers = ArrayExtend.CreateArray(container);
            CreateNewElements(containers);
        }

        public void RemoveContainerIntern(object container)
        {
            object[] containers = ArrayExtend.CreateArray(container);
            RemoveElements(containers);
        }

        private void CreateDebugScreen()
        {
            _debugScreenParent.DestroyAllChild();
            object[] containers = GameObject.FindObjectsOfType<MonoBehaviour>();
            CreateNewElements(containers);
        }

        public void CreateNewElements(object[] containers)
        {
            DebugVar[] fields = GetMembers(containers);

            if (fields == null || _debugScreenParent == null)
            {
                return;
            }

            foreach (var field in fields)
            {
                bool isExist = false;

                foreach (Transform child in _debugScreenParent)
                {
                    if (child.TryGetComponent<DebugScreenElement>(out var e))
                    {
                        if (e.NameField == field.name)
                        {
                            isExist = true;
                            break;
                        }
                    }
                }

                if (!isExist && GameObjectExtend.CreateGameObject<DebugScreenElement>(debugScreenElementPrefab, out var element, _debugScreenParent))
                {
                    element.SetField(field.member, field.name);
                }
            }
        }

        public void RemoveElements(object[] containers)
        {
            DebugVar[] fields = GetMembers(containers);

            if (fields == null)
            {
                return;
            }

            foreach (var field in fields)
            {
                for (int i = _debugScreenParent.childCount - 1; i != -1; i--)
                {
                    Transform child = _debugScreenParent.GetChild(i);

                    if (child != null)
                    {
                        if (child.TryGetComponent<DebugScreenElement>(out var element))
                        {
                            if (field.member.memberInfo.IsNotNull(out var memberInfo) && element.NameField == memberInfo.Name)
                            {
                                _debugScreenParent.DestroyChild(i);
                            }
                        }
                    }
                }
            }
        }

        private DebugVar[] GetMembers(object[] containers)
        {
            return GetMembers(debugView.GetSpecificKeys(
                (string value) =>
            {
                return debugView.IsNotNullAndTryGetValue(value, out bool result) && result;
            })
                );
        }

        public DebugVar[] GetMembers(string[] categories)
        {
            List<string> aux = new(categories);
            List<DebugVar> result = new();


            var debugPages = ReflectionExtend.CreateDerivedInstances<DebugScreenVarsNative>();

            foreach (var debugPage in debugPages)
            {
                var methods = ReflectionExtend.GetMethodsWithSpecificAttribute<DebugScreenAttribute>(ArrayExtend.CreateArray(debugPage), (DebugScreenAttribute attr) =>
                {
                    return aux != null && aux.Contains(attr.DebugCategory);
                });

                foreach (var method in methods)
                {
                    var member = method.Invoke<MemberComplexInfo>();
                    string name = "";
                    if (member != null)
                    {
                        name = ReflectionExtend.GetValueAttribute(method.methodInfo, (DebugScreenAttribute attr) =>
                        {
                            return attr.DebugName;
                        });


                        result.Add(new DebugVar(name, member));
                    }
                }
            }

            return result.ToArray();
        }
    }
}
