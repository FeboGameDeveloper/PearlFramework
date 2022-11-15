using Pearl.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Pearl
{
    public class ReferenceEqualityComparer : EqualityComparer<System.Object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }
        public override int GetHashCode(object obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }

    public enum MemberEnum { Field, Property, Method, Either }

    public enum GetterEnum { Field, Property, Method }

    [Serializable]
    public struct ReflectionParameter
    {
        public string name;
        public object value;
        public MemberEnum paramterType;
        public FuzzyBoolean staticMember;

        public ReflectionParameter(string name, object value, FuzzyBoolean staticMember)
        {
            this.name = name;
            this.value = value;
            paramterType = MemberEnum.Either;
            this.staticMember = staticMember;
        }

        public ReflectionParameter(string name, object value)
        {
            this.name = name;
            this.value = value;
            paramterType = MemberEnum.Either;
            this.staticMember = FuzzyBoolean.IDoNo;
        }

        public ReflectionParameter(string name, object value, MemberEnum paramterType, FuzzyBoolean staticMember)
        {
            this.name = name;
            this.value = value;
            this.paramterType = paramterType;
            this.staticMember = staticMember;
        }

        public ReflectionParameter(string name, object value, MemberEnum paramterType)
        {
            this.name = name;
            this.value = value;
            this.paramterType = paramterType;
            this.staticMember = FuzzyBoolean.IDoNo;
        }

    }

    [Serializable]
    public struct NameClass
    {
        public string namespaceString;
        public string classString;
        public string assembly;

        public NameClass(string namespaceString, string classString)
        {
            this.namespaceString = namespaceString;
            this.classString = classString;
            this.assembly = "";
        }

        public NameClass(string namespaceString, string classString, string assembly)
        {
            this.namespaceString = namespaceString;
            this.classString = classString;
            this.assembly = assembly;
        }

        public string GetFullNameClass()
        {
            return namespaceString != "" ? namespaceString + "." + classString : classString;
        }

        public Type GetTypeFromName()
        {
            string fullName = GetFullNameClass();
            string result = assembly != null && assembly != "" ? fullName + ", " + assembly : fullName;
            return Type.GetType(result);
        }
    }

    [Serializable]
    public class MethodInfoStruct
    {
        public object container;
        public string methodName;
    }

    public class MemberComplexInfo
    {
        public MemberInfo memberInfo;
        public object container;
        public bool isArray;
        public int index = 0;

        public MemberComplexInfo(MemberInfo memberInfo, object container)
        {
            this.memberInfo = memberInfo;
            this.container = container;
        }

        public MemberComplexInfo(MemberInfo memberInfo, object container, bool isArray, int index)
        {
            this.memberInfo = memberInfo;
            this.container = container;
            this.isArray = isArray;
            this.index = index;
        }
    }

    public class MethodComplexInfo
    {
        public MethodInfo methodInfo;
        public object container;

        public MethodComplexInfo(MethodInfo methodInfo, object container)
        {
            this.methodInfo = methodInfo;
            this.container = container;
        }
    }

    //Metodi di utilità della reflection.
    public static class ReflectionExtend
    {
        public const BindingFlags FLAGS_ALL = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
        public const BindingFlags FLAGS_ALL_DECLARED = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        #region Variables and Properties

        public static T Getter<T>(object container, string memberName, GetterEnum getterEnum)
        {
            try
            {
                if (getterEnum == GetterEnum.Method)
                {
                    return (T)container.GetType().GetMethod(memberName, FLAGS_ALL).Invoke(container, null);
                }
                else if (getterEnum == GetterEnum.Property)
                {
                    return (T)container.GetType().GetProperty(memberName, FLAGS_ALL).GetValue(container);
                }
                else
                {
                    return (T)container.GetType().GetField(memberName, FLAGS_ALL).GetValue(container);
                }
            }
            catch (Exception e)
            {
                Debug.LogManager.Log(e);
                return default;
            }
        }

        public static T Getter<T>(Type type, string memberName, GetterEnum getterEnum)
        {
            try
            {
                if (getterEnum == GetterEnum.Method)
                {
                    return (T)type.GetMethod(memberName, FLAGS_ALL).Invoke(null, null);
                }
                else if (getterEnum == GetterEnum.Property)
                {
                    return (T)type.GetProperty(memberName, FLAGS_ALL).GetValue(null);
                }
                else
                {
                    return (T)type.GetField(memberName, FLAGS_ALL).GetValue(null);
                }
            }
            catch (Exception e)
            {
                Debug.LogManager.Log(e);
                return default;
            }
        }


        public static Type ValueType(this MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fieldInfo)
            {
                return fieldInfo.FieldType;
            }
            if (memberInfo is PropertyInfo propertyInfo)
            {
                return propertyInfo.PropertyType;
            }
            if (memberInfo is MethodInfo methodInfo)
            {
                return methodInfo.ReturnType;
            }
            return null;
        }

        public static void Invoke(this MethodComplexInfo methodComplexInfo, object[] parameters = null)
        {
            if (methodComplexInfo.methodInfo.IsNotNull(out MethodInfo methodInfo))
            {
                methodInfo.Invoke(methodComplexInfo.container, parameters);
            }
        }

        public static T Invoke<T>(this MethodComplexInfo methodComplexInfo, object[] parameters = null)
        {
            if (methodComplexInfo.methodInfo.IsNotNull(out MethodInfo methodInfo))
            {
                return (T)methodInfo.Invoke(methodComplexInfo.container, parameters);
            }

            return default;
        }

        #region GetValue

        public static object GetValue(this MemberComplexInfo memberComplexInfo)
        {
            if (memberComplexInfo != null && memberComplexInfo.memberInfo.IsNotNull(out MemberInfo memberInfo))
            {
                if (memberComplexInfo.isArray)
                {
                    var result = memberInfo.GetValue(memberComplexInfo.container);
                    if (result is IEnumerable enumerable)
                    {
                        int i = 0;
                        foreach (object element in enumerable)
                        {
                            if (i == memberComplexInfo.index)
                            {
                                return element;
                            }
                            i++;
                        }
                    }
                    return null;
                }
                else
                {
                    return memberInfo.GetValue(memberComplexInfo.container);
                }
            }
            return null;
        }

        public static T GetValue<T>(this MemberComplexInfo memberComplexInfo)
        {
            object obj = memberComplexInfo.GetValue();
            if (obj != null && obj is T result)
            {
                return result;
            }

            return default;
        }

        public static object GetValue(Type type, params string[] fieldsName)
        {
            return GetValue<object>(type, fieldsName);
        }

        public static T GetValue<T, F>(params string[] fieldsName)
        {
            return GetValue<T>(typeof(F), fieldsName);
        }

        public static T GetValue<T>(Type type, params string[] fieldsName)
        {
            MemberComplexInfo membrInfo = GetValueInfo(type, fieldsName);
            return membrInfo.GetValue<T>();
        }

        public static object GetValue(this MemberInfo memberInfo, object container, params object[] paramaters)
        {
            try
            {
                if (memberInfo is FieldInfo fieldInfo)
                {
                    return fieldInfo.GetValue(container);
                }
                else if (memberInfo is PropertyInfo propertyInfo)
                {
                    return propertyInfo.GetValue(container);
                }
                else if (memberInfo is MethodInfo methodInfo)
                {
                    return methodInfo.Invoke(container, paramaters);
                }
            }
            catch (ArgumentException e)
            {
                LogManager.LogWarning(e);
                return null;
            }

            return null;
        }

        public static MemberComplexInfo GetValueInfo(Type type, params string[] fieldsName)
        {
            object container = GameObject.FindObjectOfType(type);

            if (container != null && fieldsName.IsAlmostSpecificCount())
            {
                for (int i = 0; i < fieldsName.Length; i++)
                {
                    string fieldName = fieldsName[i];

                    string[] aux = fieldName.Split('[');
                    if (aux != null)
                    {
                        if (aux.Length <= 1)
                        {
                            if (i == fieldsName.Length - 1)
                            {
                                if (GetFieldOrPropertyInfo(container, fieldName, out MemberInfo memberInfo))
                                {
                                    return new MemberComplexInfo(memberInfo, container);
                                }
                                break;
                            }
                            else
                            {
                                if (!GetFieldOrPropertyOrMethod(container, fieldName, out container))
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            string nameArray = aux[0];
                            string indexString = aux[1].Replace("]", "");
                            int index = -1;

                            try
                            {
                                index = Int32.Parse(indexString);
                            }
                            catch (Exception)
                            {
                            }

                            if (index != -1)
                            {
                                if (i == fieldsName.Length - 1)
                                {
                                    GetFieldOrPropertyInfo(container, nameArray, out MemberInfo memberInfo);
                                    return new MemberComplexInfo(memberInfo, container, true, index);

                                }
                                else
                                {
                                    ReflectionExtend.GetCollectionsElement(container, nameArray, index, out container);
                                }
                            }
                        }
                    }
                }
            }

            LogManager.LogWarning("The path is wrong");
            return default;
        }
        #endregion

        public static void SetValue<T, Type>(T value, params string[] fieldsName)
        {
            SetValue<T>(typeof(Type), value, fieldsName);
        }

        public static void SetValue<T>(Type type, T value, params string[] fieldsName)
        {
            if (fieldsName.IsAlmostSpecificCount())
            {
                MemberComplexInfo member = GetValueInfo(type, fieldsName);
                member?.SetValue(value);
            }
        }

        public static void SetStaticField<T>(Type type, T value, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, FLAGS_ALL);
            field?.SetValue(null, value);
        }

        public static void SetStaticField<T, F>(T value, string fieldName)
        {
            SetStaticField<T>(typeof(F), value, fieldName);
        }

        public static MemberComplexInfo GetValueInfo<T>(params string[] fieldsName)
        {
            return GetValueInfo(typeof(T), fieldsName);
        }

        #region GetField

        private static bool GetFieldPrivate(object container, string name, out object result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            result = null;
            if (GetFieldInfo(container, name, out FieldInfo fieldInfo, bindingFlags))
            {
                result = fieldInfo.GetValue(container);
                return true;
            }
            return false;
        }

        public static bool GetField<T>(object container, string name, out T result)
        {
            if (container != null && name != null)
            {
                GetFieldPrivate(container, name, out object var);
                if (var != null && var is T tVar)
                {
                    result = tVar;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool GetField<Type>(string name, out object result)
        {
            object container = GameObject.FindObjectOfType(typeof(Type));

            return GetFieldPrivate(container, name, out result);
        }

        public static bool GetField<Result, Type>(string name, out Result result)
        {
            GetField<Type>(name, out object var);
            if (var != null && var is Result tVar)
            {
                result = tVar;
                return true;
            }
            result = default;
            return false;
        }

        public static bool GetFieldInfo(object container, string name, out FieldInfo result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            result = null;
            if (container != null && name != null)
            {
                Type type = container.GetType();
                while ((result = type.GetField(name, bindingFlags)) == null && (type = type.BaseType) != null) ;

                if (result != null)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region GetProperty
        public static bool GetProperty(object container, string name, out object result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            result = null;
            if (GetPropertyInfo(container, name, out PropertyInfo propertyInfo, bindingFlags))
            {
                result = propertyInfo.GetValue(container);
                return true;
            }
            return false;
        }

        public static bool GetProperty<T>(object container, string name, out T result) where T : UnityEngine.Object
        {
            if (container != null && name != null)
            {
                GetProperty(container, name, out object var);
                if (var != null && var is T tVar)
                {
                    result = tVar;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool GetProperty<Type>(string name, out object result)
        {
            object container = GameObject.FindObjectOfType(typeof(Type));

            return GetProperty(container, name, out result);
        }

        public static bool GetProperty<Result, Type>(string name, out Result result)
        {
            GetProperty<Type>(name, out object var);
            if (var != null && var is Result tVar)
            {
                result = tVar;
                return true;
            }
            result = default;
            return false;
        }


        public static bool GetPropertyInfo(object container, string name, out PropertyInfo result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            result = null;
            if (container != null && name != null)
            {
                Type type = container.GetType();

                while ((result = type.GetProperty(name, bindingFlags)) == null && (type = type.BaseType) != null) ;

                if (result != null && result.CanRead)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        public static void SetStaticProperty<T>(Type type, T value, string propertyName)
        {
            PropertyInfo property = type.GetProperty(propertyName, FLAGS_ALL);
            property?.SetValue(null, value);
        }

        public static void SetStaticProperty<T, F>(T value, string propertyName)
        {
            SetStaticProperty<T>(typeof(F), value, propertyName);
        }

        #region GetFieldOrProperty
        public static bool GetFieldOrPropertyOrMethod(object container, string name, out object result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            bool isGet = GetField(container, name, out result);
            if (!isGet)
            {
                isGet = GetProperty(container, name, out result);
            }
            if (!isGet)
            {
                isGet = UseMethodWithResult<object>(container, name, out result);
            }

            return isGet;
        }

        public static bool GetFieldOrPropertyOrMethod<T>(object container, string name, out T result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            if (container != null && name != null)
            {
                GetFieldOrPropertyOrMethod(container, name, out object var);
                if (var != null && var is T tVar)
                {
                    result = tVar;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool GetFieldOrProperty<Type>(string name, out object result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            object container = GameObject.FindObjectOfType(typeof(Type));

            return GetFieldOrPropertyOrMethod(container, name, out result);
        }

        public static bool GetFieldOrProperty<Result, Type>(object container, string name, out Result result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            GetFieldOrProperty<Type>(name, out object var);
            if (var != null && var is Result tVar)
            {
                result = tVar;
                return true;
            }
            result = default;
            return false;
        }

        public static bool GetFieldOrPropertyInfo(object container, string name, out MemberInfo result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            bool isGet = GetFieldInfo(container, name, out FieldInfo fieldInfo, bindingFlags);
            result = fieldInfo;
            if (!isGet)
            {
                isGet = GetPropertyInfo(container, name, out PropertyInfo propertyInfo, bindingFlags);
                result = propertyInfo;
            }

            return isGet;
        }
        #endregion

        public static void SetStaticFieldOrPropertyOrMethod<T>(Type type, T value, string propertyName, BindingFlags bindingFlags = FLAGS_ALL)
        {
            MemberInfo member = type.GetProperty(propertyName, bindingFlags);
            if (member == null)
            {
                member = type.GetField(propertyName, bindingFlags);
            }
            if (member == null)
            {
                member = type.GetMethod(propertyName, bindingFlags);
            }
            member?.SetValue(null, value);
        }

        public static void SetStaticFieldOrProperty<T, F>(T value, string propertyName)
        {
            SetStaticProperty<T>(typeof(F), value, propertyName);
        }

        public static void GetCollectionsElement(object container, string name, int index, out object result, BindingFlags bindingFlags = FLAGS_ALL)
        {
            if (ReflectionExtend.GetFieldOrPropertyOrMethod(container, name, out result))
            {
                if (result is IEnumerable enumerable)
                {
                    int i = 0;
                    foreach (object element in enumerable)
                    {
                        if (i == index)
                        {
                            result = element;
                            break;
                        }
                        i++;
                    }
                }
            }
        }

        public static bool SetField(object container, string name, object newValue, BindingFlags bindingFlags = FLAGS_ALL)
        {
            if (container != null && name != null && newValue != null)
            {
                Type type = container.GetType();
                FieldInfo fieldInfo;
                while ((fieldInfo = type.GetField(name, bindingFlags)) == null && (type = type.BaseType) != null) ;

                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(container, newValue);
                    return true;
                }
            }

            return false;
        }

        public static void SetValue(this MemberComplexInfo memberComplexInfo, object newValue)
        {
            if (memberComplexInfo != null)
            {
                SetValue(memberComplexInfo.memberInfo, memberComplexInfo.container, newValue);

            }
        }

        public static void SetValue(this MemberInfo memberInfo, object container, object newValue)
        {
            if (memberInfo != null)
            {
                if (memberInfo is FieldInfo fieldInfo)
                {
                    fieldInfo.SetValue(container, newValue);
                }
                else if (memberInfo is PropertyInfo propertyInfo)
                {
                    if (propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(container, newValue);
                    }
                }
            }
        }

        public static bool SetProperty(object container, string name, object newValue, BindingFlags bindingFlags = FLAGS_ALL)
        {
            if (container != null && name != null && newValue != null)
            {
                Type type = container.GetType();
                PropertyInfo fieldInfo;
                while ((fieldInfo = type.GetProperty(name, bindingFlags)) == null && (type = type.BaseType) != null) ;

                if (fieldInfo != null)
                {
                    if (fieldInfo.CanWrite)
                    {
                        fieldInfo.SetValue(container, newValue);
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public static void SetValue(object container, ReflectionParameter parameter, BindingFlags bindingFlags = FLAGS_ALL)
        {
            if (parameter.paramterType == MemberEnum.Property)
            {
                if (parameter.staticMember == FuzzyBoolean.Yes)
                {
                    SetStaticProperty(container.GetType(), parameter.value, parameter.name);
                }
                else
                {
                    SetProperty(container, parameter.name, parameter.value, bindingFlags);
                }
            }
            else if (parameter.paramterType == MemberEnum.Field)
            {
                if (parameter.staticMember == FuzzyBoolean.Yes)
                {
                    SetStaticField(container.GetType(), parameter.value, parameter.name);
                }
                else
                {
                    SetField(container, parameter.name, parameter.value, bindingFlags);
                }
            }
            else if (parameter.paramterType == MemberEnum.Method)
            {
                if (parameter.staticMember == FuzzyBoolean.Yes)
                {
                    UseStaticMethod(container.GetType(), parameter.name, parameter.value);
                }
                else
                {
                    UseMethod(container, parameter.name, parameter.value, bindingFlags);
                }
            }
            else
            {
                if (parameter.staticMember == FuzzyBoolean.Yes)
                {
                    SetStaticFieldOrPropertyOrMethod(container.GetType(), parameter.value, parameter.name);
                }
                else
                {
                    SetFieldOrPropertyOrMethod(container, parameter.name, parameter.value, bindingFlags);
                }
            }
        }

        public static bool SetFieldOrPropertyOrMethod(object container, string name, object newValue, BindingFlags bindingFlags = FLAGS_ALL)
        {
            bool isSet = SetField(container, name, newValue, bindingFlags);
            if (!isSet)
            {
                isSet = SetProperty(container, name, newValue, bindingFlags);
            }
            if (!isSet)
            {
                UseMethod(container, name, newValue);
            }

            return isSet;
        }

        #endregion

        public static Type GetType(string typeString, string assemblyName = null)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type result;
            foreach (var assembly in assemblies)
            {
                if (assemblyName == null || assembly.GetName().Name.Contains(assemblyName, StringComparison.CurrentCultureIgnoreCase))
                {
                    result = assembly.GetType(typeString);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public static MemberInfo[] GetCustomAttribute<T>(this Type type, string memberName, BindingFlags bindings = FLAGS_ALL) where T : Attribute
        {
            MemberInfo[] membersInfo = type.GetMember(memberName, bindings);
            List<MemberInfo> result = new();
            foreach (var memberInfo in membersInfo)
            {
                if (memberInfo.GetCustomAttribute<T>() != null)
                {
                    result.Add(memberInfo);
                }
            }

            if (result.IsAlmostSpecificCount())
            {
                return result.ToArray();
            }
            return null;

        }

        public static MemberInfo[] GetPropertiesAndFields(this Type type, BindingFlags bindingFlags = FLAGS_ALL)
        {
            List<MemberInfo> memberInfos = new();

            if (type == null)
            {
                return null;
            }

            memberInfos.AddRange(type.GetFields(bindingFlags));
            memberInfos.AddRange(type.GetProperties(bindingFlags));

            return memberInfos.ToArray();
        }

        public static MemberInfo[] GetPropertiesAndFieldsWithAttribute<T>(this Type type, bool inherit = false, BindingFlags bindingFlags = FLAGS_ALL) where T : Attribute
        {
            MemberInfo[] memberInfos = GetPropertiesAndFields(type, bindingFlags);
            List<MemberInfo> result = new();
            foreach (MemberInfo member in memberInfos)
            {
                if (member.GetCustomAttribute(typeof(T), inherit) is T)
                {
                    result.Add(member);
                }
            }

            return result.ToArray();
        }


        public static MemberComplexInfo[] GetFieldsOrFieldsWithSpecificAttribute<Attr>(object[] elements, Predicate<Attr> filter = null) where Attr : System.Attribute
        {
            List<MemberComplexInfo> members = new();

            foreach (var element in elements)
            {
                var variables = element.GetType().GetPropertiesAndFields(FLAGS_ALL);

                foreach (var variable in variables)
                {
                    System.Attribute[] attrs = System.Attribute.GetCustomAttributes(variable);

                    foreach (System.Attribute genericAttr in attrs)
                    {
                        if (genericAttr is Attr attr && (filter == null || filter.Invoke(attr)))
                        {
                            members.Add(new MemberComplexInfo(variable, element));
                            break;
                        }
                    }
                }
            }
            return members.ToArray();
        }

        public static MethodComplexInfo[] GetMethodsWithSpecificAttribute<Attr>(object[] elements, Predicate<Attr> filter = null) where Attr : System.Attribute
        {
            List<MethodComplexInfo> members = new();

            foreach (var element in elements)
            {
                var variables = element.GetType().GetMethods(FLAGS_ALL);

                foreach (var variable in variables)
                {
                    System.Attribute[] attrs = System.Attribute.GetCustomAttributes(variable);

                    foreach (System.Attribute genericAttr in attrs)
                    {
                        if (genericAttr is Attr attr && (filter == null || filter.Invoke(attr)))
                        {
                            members.Add(new MethodComplexInfo(variable, element));
                            break;
                        }
                    }
                }
            }

            return members.ToArray();
        }

        public static Result GetValueAttribute<Attr, Result>(MemberInfo member, Func<Attr, Result> func) where Attr : System.Attribute
        {
            if (member != null)
            {
                System.Attribute[] attrs = System.Attribute.GetCustomAttributes(member);

                foreach (System.Attribute attr in attrs)
                {
                    if (attr is Attr specificAttr)
                    {
                        return func(specificAttr);
                    }
                }
            }

            LogManager.LogWarning("Wrong");
            return default;
        }


        #region Methods

        public static bool UseMethod(object container, string nameMethod, params object[] parameters)
        {
            if (container != null && nameMethod != null)
            {
                var specificMethods = GetMethods(container, nameMethod);

                if (specificMethods == null)
                {
                    return false;
                }

                foreach (var methodInfo in specificMethods)
                {
                    try
                    {
                        methodInfo.Invoke(container, parameters);
                        return true;
                    }
                    catch
                    {
                    }
                }
            }
            return false;
        }

        public static bool UseMethodWithResult<T>(object container, string nameMethod, out T result, params object[] parameters)
        {
            result = default;
            if (nameMethod != null)
            {
                var specificMethods = GetMethods(container, nameMethod);

                if (specificMethods == null)
                {
                    return false;
                }

                foreach (var methodInfo in specificMethods)
                {
                    try
                    {
                        result = (T)methodInfo.Invoke(container, parameters);
                        return true;
                    }
                    catch
                    {
                    }
                }
            }
            return false;
        }

        public static bool UseStaticMethod(Type type, string nameMethod, params object[] parameters)
        {
            if (type != null && nameMethod != null)
            {
                var specificMethods = GetMethods(type, nameMethod);

                if (specificMethods == null)
                {
                    return false;
                }

                foreach (var methodInfo in specificMethods)
                {
                    try
                    {
                        methodInfo.Invoke(null, parameters);
                        return true;
                    }
                    catch
                    {
                    }
                }
            }
            return false;
        }

        public static bool UseStaticMethodWithResult<T>(Type type, string nameMethod, out T result, params object[] parameters)
        {
            result = default;
            if (type != null && nameMethod != null)
            {
                var specificMethods = GetMethods(type, nameMethod);

                if (specificMethods == null)
                {
                    return false;
                }

                foreach (var methodInfo in specificMethods)
                {
                    try
                    {
                        result = (T)methodInfo.Invoke(null, parameters);
                        return true;
                    }
                    catch
                    {
                    }
                }
            }
            return false;
        }

        public static bool IsAction(in MethodInfo methodInfo)
        {
            return methodInfo != null && methodInfo.ReturnType.Equals((typeof(void)));
        }

        #endregion

        #region Create Delegate

        public static Delegate CreateAction(in MethodInfoStruct methodInfoStruct, params Type[] parameters)
        {
            return CreateAction(methodInfoStruct.container, methodInfoStruct.methodName, parameters);
        }

        public static Delegate CreateMethod(in object target, in string nameAction, params Type[] parameters)
        {
            if (target != null && nameAction != null)
            {
                Type type = target.GetType();
                MethodInfo methodInfo = null;

                while (methodInfo == null && type != null)
                {
                    methodInfo = type.GetMethod(nameAction, FLAGS_ALL, null, parameters, null);
                    type = type.BaseType;
                }

                return CreateDelegate(methodInfo, target);
            }
            return null;
        }

        public static Delegate CreateMethod(Type type, in string nameAction, params Type[] parameters)
        {
            if (nameAction != null)
            {
                MethodInfo methodInfo = null;

                while (methodInfo == null && type != null)
                {
                    methodInfo = type.GetMethod(nameAction, FLAGS_ALL, null, parameters, null);
                    type = type.BaseType;
                }

                return CreateDelegate(methodInfo);
            }
            return null;
        }


        public static Delegate CreateAction(in object target, in string nameAction, params Type[] parameters)
        {
            if (target != null && nameAction != null)
            {
                bool zeroParameter = false;

                if (parameters == null)
                {
                    parameters = new Type[0];
                }

                if (parameters.Length == 0)
                {
                    zeroParameter = true;
                }


                if (zeroParameter || !parameters.IsOneChildNull())
                {
                    Type type = target.GetType();
                    MethodInfo methodInfo = null;

                    while (methodInfo == null && type != null)
                    {
                        methodInfo = type.GetMethod(nameAction, FLAGS_ALL, null, parameters, null);
                        type = type.BaseType;
                    }

                    return IsAction(methodInfo) ? CreateDelegate(methodInfo, target) : null;
                }
            }
            return null;
        }

        public static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
        {
            if (methodInfo != null && target != null)
            {
                Func<Type[], Type> getType;
                var isAction = methodInfo.ReturnType.Equals((typeof(void)));
                var types = methodInfo.GetParameters().Select(p => p.ParameterType);

                if (isAction)
                {
                    getType = Expression.GetActionType;
                }
                else
                {
                    getType = Expression.GetFuncType;
                    types = types.Concat(new[] { methodInfo.ReturnType });
                }

                if (methodInfo.IsStatic)
                {
                    return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
                }

                return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
            }

            return null;
        }

        public static Delegate CreateDelegate(this MethodInfo methodInfo)
        {
            if (methodInfo != null)
            {
                Func<Type[], Type> getType;
                var isAction = methodInfo.ReturnType.Equals((typeof(void)));
                var types = methodInfo.GetParameters().Select(p => p.ParameterType);

                if (isAction)
                {
                    getType = Expression.GetActionType;
                }
                else
                {
                    getType = Expression.GetFuncType;
                    types = types.Concat(new[] { methodInfo.ReturnType });
                }

                return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
            }

            return null;
        }
        #endregion

        #region ChangeEventHandler
        public static void ChangeEventHandler<T>(object target, string nameEvent, Action<T> action, ActionEvent actionEvent, BindingFlags bindingFlags = FLAGS_ALL)
        {
            ChangeDelegateEventHandler(target, nameEvent, action, actionEvent, bindingFlags);
        }

        public static void ChangeEventHandler(object target, string nameEvent, Action action, ActionEvent actionEvent, BindingFlags bindingFlags = FLAGS_ALL)
        {
            ChangeDelegateEventHandler(target, nameEvent, action, actionEvent, bindingFlags);
        }

        public static void ChangeDelegateEventHandler(object target, string nameEvent, Delegate action, ActionEvent actionEvent, BindingFlags bindingFlags = FLAGS_ALL)
        {
            if (target != null && action != null && nameEvent != null)
            {
                Type t = target.GetType();
                EventInfo eventInfo = t.GetEvent(nameEvent, bindingFlags);
                if (eventInfo != null)
                {
                    switch (actionEvent)
                    {
                        case ActionEvent.Add:
                            eventInfo.AddEventHandler(target, action);
                            break;
                        case ActionEvent.Remove:
                            eventInfo.RemoveEventHandler(target, action);
                            break;
                    }
                }
            }
        }
        #endregion

        #region CreateInstance
        public static T CreateInstance<T>(NameClass nameClass, params object[] vars) where T : class
        {
            return CreateInstance<T>(nameClass.GetTypeFromName(), vars);
        }

        public static T CreateInstance<T>(params object[] vars) where T : class
        {
            return CreateInstance<T>(typeof(T), vars);
        }

        public static T CreateInstance<T>(Type typeClass, params object[] vars) where T : class
        {
            if (typeClass != null)
            {
                object obj = null;
                if (vars == null || vars.Length <= 0)
                {
                    try
                    {
                        obj = Activator.CreateInstance(typeClass, true);
                    }
                    catch (Exception e)
                    {
                        Debug.LogManager.LogWarning(e);
                    }
                }
                else
                {
                    var constructors = (typeof(T)).GetConstructors(FLAGS_ALL);
                    foreach (var cons in constructors)
                    {
                        try
                        {
                            obj = cons.Invoke(vars);
                            break;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (obj == null)
                    {
                        Debug.LogManager.LogWarning("Not Exist the specific constructor");
                    }
                }

                Type otherType = typeof(T);
                if (obj != null && (typeClass == otherType || typeClass.IsSubclassOf(otherType)))
                {
                    return (T)obj;
                }
            }

            return null;
        }

        public static T[] CreateDerivedInstances<T>(params object[] vars) where T : class
        {
            Type[] types = FindAllDerivedTypes<T>();
            T[] result = new T[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                result[i] = CreateInstance<T>(types[i], vars);
            }

            return result;
        }
        #endregion

        #region FindAllDerivedTypes
        public static Type[] FindAllDerivedTypes<T>(bool useSoloThisAssembly = false)
        {
            if (useSoloThisAssembly)
            {
                return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
            }
            else
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                List<Type> result = new();

                foreach (var assembly in assemblies)
                {
                    result.AddRange(FindAllDerivedTypes<T>(assembly));
                }

                return result.ToArray();
            }
        }

        public static Type[] FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly.GetTypes().Where(t => t != derivedType && derivedType.IsAssignableFrom(t)).ToArray();

        }
        #endregion


        #region Private Methods
        private static MethodInfo[] GetMethods(object container, string nameMethod)
        {
            Type type = container.GetType();
            return GetMethods(type, nameMethod);
        }

        private static MethodInfo[] GetMethods(Type type, string nameMethod)
        {
            MethodInfo[] methodsInfo = type.GetMethods(FLAGS_ALL);
            return methodsInfo.FilterArray((x) => x.Name == nameMethod);
        }
        #endregion
    }
}
