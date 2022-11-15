using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Editor
{
    [CreateAssetMenu(fileName = "VarDefaultTextManager", menuName = "Pearl/VarDefaultTextManager", order = 1)]
    public class VarDefaultTextManagerScriptableObject : ScriptableObject
    {
        [SerializeField]
        private Dictionary<string, string> dictionary = null;

        public Dictionary<string, string> Dict { get { return dictionary; } }
    }
}
