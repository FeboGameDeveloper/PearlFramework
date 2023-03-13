using Pearl.Debug;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pearl
{
    public static class EnumExtend
    {
        public static T GetRandom<T>() where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                int auxInteger = UnityEngine.Random.Range(0, Length<T>());
                return (T)Enum.ToObject(typeof(T), auxInteger);
            }

            return default;
        }

        public static T GetRandom<T>(params T[] exceptions) where T : struct, IConvertible
        {
            if (exceptions != null && typeof(T).IsEnum)
            {
                exceptions = exceptions.Distinct().ToArray();

                int[] exceptionNumbers = new int[exceptions.Length];
                for (int i = 0; i < exceptionNumbers.Length; i++)
                {
                    exceptionNumbers[i] = Convert.ToInt32(exceptions[i]);
                }

                int length = Length<T>();

                int[] numbers = new int[length - exceptionNumbers.Length];
                int j = 0;
                for (int i = 0; i < length; i++)
                {
                    if (!Array.Exists<int>(exceptionNumbers, x => x == i))
                    {
                        numbers[j] = i;
                        j++;
                    }
                }

                int auxInteger = RandomExtend.GetRandomElement(numbers);
                return (T)Enum.ToObject(typeof(T), auxInteger);
            }

            return default;
        }

        public static int ParseEnum(string value, Type type, bool ignoreCase = false)
        {
            if (type.IsEnum && value != null)
            {
                string[] array = GetArray(type);

                for (int i = 0; i < array.Length; i++)
                {
                    bool isEqual = ignoreCase ? array[i].EqualsIgnoreCase(value) : array[i].Equals(value);

                    if (isEqual)
                    {
                        return i;
                    }
                }
                LogManager.LogWarning("The enum doesn't exist");
            }

            return default;
        }

        public static T ParseEnum<T>(string value, bool ignoreCase = false) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum && value != null)
            {
                if (ignoreCase)
                {
                    T[] array = GetArray<T>();

                    foreach (T element in array)
                    {
                        if (element.ToString().EqualsIgnoreCase(value))
                        {
                            return element;
                        }
                    }
                    LogManager.LogWarning("The enum doesn't exist");
                }
                else
                {
                    try
                    {
                        return (T)Enum.Parse(typeof(T), value, true);
                    }
                    catch (ArgumentException e)
                    {
                        LogManager.LogWarning(e.Message);
                    }
                }
            }

            return default;
        }

        public static int Length(Type type)
        {
            if (type.IsEnum)
            {
                return Enum.GetValues(type).Length;
            }

            return default;
        }

        public static int Length<T>() where T : struct, IConvertible
        {
            return Length(typeof(T));
        }

        public static T Next<T>(T value) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                int auxInteger = Convert.ToInt32(value);
                auxInteger = MathfExtend.ChangeInCircle(auxInteger, 1, Length<T>());
                return (T)Enum.ToObject(typeof(T), auxInteger);
            }

            return default;
        }

        public static string[] GetArray(Type type)
        {
            if (type.IsEnum)
            {
                string[] enums = new string[Length(type)];
                for (int i = 0; i < enums.Length; i++)
                {
                    enums[i] = Enum.ToObject(type, i).ToString();
                }
                return enums;
            }

            return default;
        }

        public static T[] GetArray<T>() where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                T[] enums = new T[Length<T>()];
                for (int i = 0; i < enums.Length; i++)
                {
                    enums[i] = (T)Enum.ToObject(typeof(T), i);
                }
                return enums;
            }

            return default;
        }


        public static List<string> ConvertInListString<T>(T[] listEnum) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                List<string> stringList = new();
                foreach (var en in listEnum)
                {
                    stringList.Add(en.ToString());
                }

                return stringList;
            }

            return default;
        }

        public static bool IsNotEqual<T>(T element, params T[] members) where T : struct, IConvertible
        {
            foreach (T member in members)
            {
                if (element.Equals(member))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsEqual<T>(T element, T member) where T : struct, IConvertible
        {
            return element.Equals(member);
        }
    }

}