using System;
using UnityEngine;

namespace Pearl
{
    /// <summary>
    /// Conditionally Show/Hide field in inspector, based on some other field value 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public readonly string FieldToCheck;

        /// <param name="fieldToCheck">String name of field to check value</param>
        public ConditionalFieldAttribute(string fieldToCheck)
        {
            FieldToCheck = fieldToCheck;
        }
    }
}