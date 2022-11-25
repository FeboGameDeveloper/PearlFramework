using Pearl.Events;
using Pearl.Input;
using Pearl.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pearl
{
    /// <summary>
    /// A class that manages the scene change
    /// </summary>
    public class SceneSystemManager : PearlBehaviour, ISingleton
    {
        [SerializeField]
        private string loadingScene = "loading";

        [SerializeField]
        private GameObject loadingBarPrefab = null;

        /// <summary>
        /// The current scene in the game
        /// </summary>
        private string _currentScene;

        /// <summary>
        /// The version of the game
        /// </summary>
        public static string CurrentScene
        {
            get
            {
                if (Singleton<SceneSystemManager>.GetIstance(out var sceneManager))
                {
                    if (sceneManager)
                    {
                        return sceneManager._currentScene;
                    }
                }

                return string.Empty;
            }
        }

        #region Unity Callbacks
        protected override void Start()
        {
            base.Start();

            ReceiveNewScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            SceneManager.sceneLoaded += ReceiveNewScene;
        }

        /// <summary>
        /// A method called when the manager is destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            SceneManager.sceneLoaded -= ReceiveNewScene;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// The method accesses a new scene through the name of scene
        /// </summary>
        /// <param name = "newScene">The new scene in string</param>
        public static void EnterNewScene(in string newScene)
        {
            if (newScene != null)
            {
                SceneManager.LoadScene(newScene);
            }
        }

        public static void EnterNewSceneAsync(in string newScene, Action callback, bool useLoadingBar)
        {
            if (Singleton<SceneSystemManager>.GetIstance(out var sceneManager))
            {
                sceneManager.EnterSceneAsync(newScene, callback, useLoadingBar);
            }
        }
        #endregion

        #region Private Methods
        private void EnterSceneAsync(in string newScene, Action callback, bool useLoadingBar)
        {
            StartCoroutine(EnterNewSceneAsync(newScene, callback, useLoadingBar));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newScene"></param>
        /// <param name="callback"></param>
        /// <param name="useLoadingBar"></param>
        /// <returns></returns>
        private IEnumerator EnterNewSceneAsync(string newScene, Action callback, bool useLoadingBar)
        {
            if (newScene == null)
            {
                callback.Invoke();
                yield return null;
            }

            InputManager.ActiveAllInput(false);
            BlackPageManager.AppearBlackPage();

            SceneManager.LoadScene(loadingScene);
            GameObject loadingIstance = null;

            if (useLoadingBar)
            {
                BlackPageManager.DisappearBlackPage();
                GameObjectExtend.CreateUIlement(loadingBarPrefab, out loadingIstance, canvasTipology: CanvasTipology.HUD);
            }

            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newScene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                PearlEventsManager.CallEvent(ConstantStrings.loadingLevel, PearlEventType.Normal, asyncLoad.progress);
                yield return null;
            }

            if (useLoadingBar)
            {
                GameObjectExtend.DestroyExtend(loadingIstance);
            }
            else
            {
                BlackPageManager.DisappearBlackPage();
            }

            callback?.Invoke();
            InputManager.ActiveAllInput(true);
        }

        /// <summary>
        /// the method is activated at the event "SceneManager.sceneLoaded": 
        /// the method starts to the event that activates when there is a new scene.
        /// </summary>
        private void ReceiveNewScene(Scene scene, LoadSceneMode load)
        {
            if (scene != null)
            {
                _currentScene = scene.name;
            }

            PearlEventsManager.CallEvent(ConstantStrings.newSceneEvent, PearlEventType.Normal, scene.name);
        }
        #endregion
    }
}
