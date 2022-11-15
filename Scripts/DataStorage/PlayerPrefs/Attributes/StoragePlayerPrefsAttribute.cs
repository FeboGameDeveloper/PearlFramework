using System;

namespace Pearl.Storage
{
    [System.AttributeUsage(System.AttributeTargets.Field |
                       System.AttributeTargets.Property,
                       AllowMultiple = false)]
    public class StoragePlayerPrefsAttribute : Attribute
    {
        private readonly string nameStoredVar;

        public string NameStoredVar { get { return nameStoredVar; } }

        public StoragePlayerPrefsAttribute(string nameStoredVar)
        {
            this.nameStoredVar = nameStoredVar;
        }

        public StoragePlayerPrefsAttribute()
        {
            this.nameStoredVar = null;
        }
    }
}
