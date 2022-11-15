#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class CurrentSceneCheck : ConditionTask
    {
        public BBParameter<string> sceneStringContainer;

        protected override string info { get { return "[" + sceneStringContainer.ToString() + "]"; } }

        protected override bool OnCheck()
        {
            if (sceneStringContainer != null && sceneStringContainer.value.IsNotNull(out string sceneString))
            {
                string currentScene = SceneSystemManager.CurrentScene;
                return currentScene != null && currentScene.EqualsIgnoreCase(sceneString);
            }
            return false;
        }
    }
}

#endif