using UnityEngine;

namespace Pearl.Editor
{
    [CreateAssetMenu(fileName = "BuildOptions", menuName = "Pearl/BuildOptions", order = 1)]

    public class BuildOptionsScriptableObject : ScriptableObject
    {
        [SerializeField]
        private bool augmentPreBuild = true;

        //Add button in inspector
        [InspectorButton("Augment1Level")]
        public bool augment1Level;

        //Add button in inspector
        [InspectorButton("Augment2Level")]
        public bool augment2Level;

        //Add button in inspector
        [InspectorButton("Augment3Level")]
        public bool augment3Level;

        public bool AugmentPreBuild { get { return augmentPreBuild; } }

        public void Augment1Level()
        {
            BuildVersionManager.AugumentVersion(0);
        }

        public void Augment2Level()
        {
            BuildVersionManager.AugumentVersion(1);
        }

        public void Augment3Level()
        {
            BuildVersionManager.AugumentVersion(2);
        }
    }
}

