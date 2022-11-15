#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ChangeSceneTask : ActionTask
    {
        [RequiredField]
        public BBParameter<string> sceneStringContainerBB;

        public BBParameter<bool> isAsyncBB;

        [Conditional("isAsyncBB", 1)]
        public BBParameter<bool> useLoadingBarBB = false;

        public BBParameter<bool> saveScenePrevBB = false;
        [Conditional("saveScenePrevBB", 1)]
        public BBParameter<string> nameVarForScenePrevBB = string.Empty;

        protected override string info
        {
            get { return string.Format("Load Scene {0}", sceneStringContainerBB); }
        }

        protected override void OnExecute()
        {
            if (sceneStringContainerBB != null)
            {
                if (saveScenePrevBB != null && saveScenePrevBB.value && nameVarForScenePrevBB != null)
                {
                    blackboard.UpdateVariable(nameVarForScenePrevBB.value, SceneSystemManager.CurrentScene);
                }


                if (isAsyncBB != null && isAsyncBB.value && useLoadingBarBB != null)
                {
                    SceneSystemManager.EnterNewSceneAsync(sceneStringContainerBB.value, OnFinish, useLoadingBarBB.value);
                }
                else
                {
                    SceneSystemManager.EnterNewScene(sceneStringContainerBB.value);
                    EndAction();
                }
            }
        }

        private void OnFinish()
        {
            EndAction();
        }
    }
}

#endif
