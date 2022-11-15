using System;
using System.IO;

namespace Pearl.Storage
{
    [System.AttributeUsage(System.AttributeTargets.Field |
                   System.AttributeTargets.Property,
                   AllowMultiple = false)]
    public class StorageAttribute : Attribute
    {
        private readonly string _relativePath = string.Empty;
        private readonly string _absolutePath = string.Empty;
        private readonly bool _isSlot = false;
        private readonly GetterEnum? _getterEnum;

        public string RelativePath { get { return _relativePath; } }
        public string AbsolutePath { get { return _absolutePath; } }
        public bool IsSlot { get { return _isSlot; } }
        public GetterEnum? Getter { get { return _getterEnum; } }


        public StorageAttribute(string relativePath, bool isSlot = false, bool isCompany = false)
        {
            _absolutePath = StorageManager.PersistentDataPath;
            if (isCompany)
            {
                _absolutePath = Directory.GetParent(_absolutePath).FullName;
            }

            _isSlot = isSlot;
            _relativePath = relativePath;
            _getterEnum = null;
        }


        public StorageAttribute(string relativePath, GetterEnum getterEnum, bool isSlot = false, bool isCompany = false) : this(relativePath, isSlot, isCompany)
        {
            _getterEnum = getterEnum;
        }
    }
}