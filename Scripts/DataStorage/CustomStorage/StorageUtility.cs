using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pearl.Storage
{
    #region AusiliaryStruct
    [Serializable]
    public struct Vector4Storage
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector4Storage(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4Storage(Vector4 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
            this.w = vector.w;
        }

        public static implicit operator Vector4Storage(Vector4 vector) => new(vector.x, vector.y, vector.z, vector.w);
        public static implicit operator Vector4(Vector4Storage vector) => new(vector.x, vector.y, vector.z, vector.w);
    }

    [Serializable]
    public struct Vector3Storage
    {
        public float x;
        public float y;
        public float z;

        public Vector3Storage(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3Storage(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        public static implicit operator Vector3Storage(Vector3 vector) => new(vector.x, vector.y, vector.z);
        public static implicit operator Vector3(Vector3Storage vector) => new(vector.x, vector.y, vector.z);
    }

    [Serializable]
    public struct Vector2Storage
    {
        public float x;
        public float y;

        public Vector2Storage(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2Storage(Vector2 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
        }

        public static implicit operator Vector2Storage(Vector2 vector) => new(vector.x, vector.y);
        public static implicit operator Vector2(Vector2Storage vector) => new(vector.x, vector.y);
    }
    #endregion


    [Serializable]
    public struct ElementToStorage
    {
        public Type type;
        public object value;

        public ElementToStorage(object value)
        {
            this.type = value.GetType();
            this.value = value;
        }
    }

    public enum UseSameFileEnum { Start, End, Ignore }

    public static class StorageUtility
    {
        private static Dictionary<string, ElementToStorage> _aux = null;
        private static UseSameFileEnum _sameFile = UseSameFileEnum.Ignore;

        #region Public Methods
        public static void Save(string key, object value, string filePath, bool encrypted, UseSameFileEnum sameFile = UseSameFileEnum.Ignore)
        {
            value = ConvertValue(value);

            AssetManager.CreateSubDirectoriesForPath(filePath);

            if (_sameFile == UseSameFileEnum.Ignore && (sameFile == UseSameFileEnum.Start || sameFile == UseSameFileEnum.Ignore))
            {
                _aux = GetElementsStorage(filePath, encrypted);
                _sameFile = sameFile;
            }

            _aux.Update(key, new ElementToStorage(value));

            if (sameFile != UseSameFileEnum.End && _sameFile == UseSameFileEnum.Start)
            {
                return;
            }


            SetElementStorage(filePath, encrypted);
            _aux = null;
            _sameFile = UseSameFileEnum.Ignore;
        }

        public static bool Load(string key, string filePath, object defaultValue, bool encrypted, out object result, UseSameFileEnum sameFile = UseSameFileEnum.Ignore)
        {
            AssetManager.CreateSubDirectoriesForPath(filePath);

            if (_sameFile == UseSameFileEnum.Ignore && (sameFile == UseSameFileEnum.Start || sameFile == UseSameFileEnum.Ignore))
            {
                _aux = GetElementsStorage(filePath, encrypted);
                _sameFile = sameFile;
            }

            if (_aux == null)
            {
                result = default;
                return false;
            }

            bool resultBool = _aux.IsNotNullAndTryGetValue(key, out var element);
            result = resultBool ? ConvetObj(element) : defaultValue;

            if (sameFile == UseSameFileEnum.End || _sameFile == UseSameFileEnum.Ignore)
            {
                _sameFile = UseSameFileEnum.Ignore;
                _aux = null;
            }

            return resultBool;
        }

        public static void Remove(string key, string filePath, bool encrypted, UseSameFileEnum sameFile = UseSameFileEnum.Ignore)
        {
            AssetManager.CreateSubDirectoriesForPath(filePath);

            if (_sameFile == UseSameFileEnum.Ignore && (sameFile == UseSameFileEnum.Start || sameFile == UseSameFileEnum.Ignore))
            {
                _aux = GetElementsStorage(filePath, encrypted);
                _sameFile = sameFile;
            }

            if (_aux == null)
            {
                return;
            }

            _aux.Remove(key);

            if (sameFile != UseSameFileEnum.End && _sameFile == UseSameFileEnum.Start)
            {
                return;
            }

            if (_aux.Count != 0)
            {
                SetElementStorage(filePath, encrypted);
            }
            else
            {
                File.Delete(filePath);
            }

            _aux = null;
            _sameFile = UseSameFileEnum.Ignore;
        }
        #endregion

        #region Private Methods
        private static object ConvetObj(ElementToStorage element)
        {
            Type type = element.type;
            object obj = element.value;
            string objString = obj.ToString();

            type = ConvertType(type);
            if (type == typeof(String))
            {
                return objString;
            }
            else if (type == typeof(bool))
            {
                return bool.Parse(objString);
            }
            else if (type == typeof(Char))
            {
                return Char.Parse(objString);
            }
            else if (type == typeof(Single))
            {
                return float.Parse(objString);
            }
            else if (type == typeof(Double))
            {
                return double.Parse(objString);
            }
            else
            {
                return JsonConvert.DeserializeObject(objString, type);
            }
        }

        private static Dictionary<string, ElementToStorage> GetElementsStorage(string filePath, bool encrypted)
        {
            string JSONstring;
            Dictionary<string, ElementToStorage> result = null;

            try
            {
                if (encrypted)
                {
                    JSONstring = Encrypted.Decrypt(filePath, StorageManager.EncryptedKey);
                }
                else
                {
                    JSONstring = File.ReadAllText(filePath);
                }

                if (JSONstring != null)
                {
                    result = JsonConvert.DeserializeObject<Dictionary<string, ElementToStorage>>(JSONstring);
                }
            }
            catch (Exception e)
            {
                Debug.LogManager.Log(e);
            }

            if (result == null)
            {
                result = new();
            }

            return result;
        }

        private static void SetElementStorage(string filePath, bool encrypted)
        {
            string JSONstring = JsonConvert.SerializeObject(_aux);

            if (encrypted)
            {
                Encrypted.Encrypt(filePath, StorageManager.EncryptedKey, JSONstring, out _);
            }
            else
            {
                File.WriteAllText(filePath, JSONstring);
            }
        }

        private static object ConvertValue(object value)
        {
            if (value is Vector2 vector2)
            {
                return new Vector2Storage(vector2);
            }
            else if (value is Vector3 vector3)
            {
                return new Vector3Storage(vector3);
            }
            if (value is Vector4 vector4)
            {
                return new Vector4Storage(vector4);
            }

            return value;
        }

        private static Type ConvertType(Type type)
        {
            if (type == typeof(Vector2Storage))
            {
                return typeof(Vector2);
            }
            else if (type == typeof(Vector3Storage))
            {
                return typeof(Vector3);
            }
            if (type == typeof(Vector4Storage))
            {
                return typeof(Vector4);
            }

            return type;
        }
        #endregion
    }
}