using UnityEngine;

namespace Pearl
{
    [CreateAssetMenu(fileName = "GameManagerCreation", menuName = "Pearl/GameManager", order = 1)]
    public class GameManagerScriptableObject : ScriptableObject
    {
        [SerializeField]
        private GameObject gameManager = null;

        [SerializeField]
        private string[] scenesExclude = null;

        public GameObject GameManager { get { return gameManager; } }
        public string[] ScenesExclude { get { return scenesExclude; } }
    }
}

