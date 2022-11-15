
#if INK
using Pearl.Ink;
#endif


using Pearl.Debug;
using Pearl.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Pearl
{
    public class ConnectionWeb : PearlBehaviour, ISingleton
    {
        [SerializeField]
        private string URLDomain = "https://game.febogame.dev/";
        [SerializeField]
        private ContainerString[] errorDescription = null;


        private List<IMultipartFormSection> _auxData;
        private UnityAction<string>[] _resultCallbackWithResult;
        private UnityAction[] _resultCallback;
        private UnityAction[] _errorCallback;
        private bool _useResult;
        private bool _useInfoError;

        public static ConnectionWeb Instance
        {
            get { return Singleton<ConnectionWeb>.GetIstance(); }
        }

        #region CreateRequest

        public static void CreateRequestWithResult(in UnityAction<string> resultCallbackWithResult, in string URL, bool useInfoError, bool isAsyinc, params ContainerString[] containers)
        {
            UnityAction<string>[] resultActions = new UnityAction<string>[] { resultCallbackWithResult };

            CreateRequestWithResult(resultActions, null, URL, useInfoError, isAsyinc, containers);
        }

        public static void CreateRequestWithResult(in UnityAction<string>[] resultCallbacksWithResult, in string URL, bool useInfoError, bool isAsyinc, params ContainerString[] containers)
        {
            CreateRequestWithResult(resultCallbacksWithResult, null, URL, useInfoError, isAsyinc, containers);
        }

        public static void CreateRequestWithResult(in UnityAction<string> resultCallbackWithResult, in UnityAction errorCallback, in string URL, bool useInfoError, bool isAsyinc, params ContainerString[] containers)
        {
            UnityAction<string>[] resultActions = new UnityAction<string>[] { resultCallbackWithResult };
            UnityAction[] errorActions = new UnityAction[] { errorCallback };

            CreateRequestWithResult(resultActions, errorActions, URL, useInfoError, isAsyinc, containers);

        }

        public static void CreateRequestWithResult(in UnityAction<string>[] resultCallbackWithResult, in UnityAction[] errorCallback, in string URL, bool useInfoError, bool isAsyinc, params ContainerString[] containers)
        {
            ConnectionWeb instance = Instance;
            if (instance != null)
            {
                instance._useResult = true;
                instance.CreateRequestInternal(null, resultCallbackWithResult, errorCallback, URL, useInfoError, isAsyinc, containers);
            }
            else
            {
                LogManager.LogWarning("There isn't instance");
            }
        }

        public static void CreateRequest(in UnityAction resultCallback, in string URL, bool useInfoError, bool isAsyinc, params ContainerString[] containers)
        {
            UnityAction[] resultActions = new UnityAction[] { resultCallback };

            CreateRequest(resultActions, null, URL, useInfoError, isAsyinc, containers);
        }

        public static void CreateRequest(in UnityAction[] resultCallback, in string URL, bool useInfoError, bool isAsyinc, params ContainerString[] containers)
        {
            CreateRequest(resultCallback, null, URL, useInfoError, isAsyinc, containers);
        }

        public static void CreateRequest(in UnityAction resultCallback, in UnityAction errorCallback, in string URL, bool useInfoError, bool isAsyinc, params ContainerString[] containers)
        {
            UnityAction[] resultActions = new UnityAction[] { resultCallback };
            UnityAction[] errorActions = new UnityAction[] { errorCallback };

            CreateRequest(resultActions, errorActions, URL, useInfoError, isAsyinc, containers);
        }

        public static void CreateRequest(in UnityAction[] resultCallback, in UnityAction[] errorCallback, in string URL, bool useInfoError, bool isAsyinc, params ContainerString[] containers)
        {
            ConnectionWeb instance = Instance;
            if (instance != null)
            {
                instance._useResult = false;
                instance.CreateRequestInternal(resultCallback, null, errorCallback, URL, useInfoError, isAsyinc, containers);
            }
            else
            {
                LogManager.LogWarning("There isn't instance");
            }
        }
        #endregion

        private void CreateRequestInternal(in UnityAction[] resultCallback, in UnityAction<string>[] resultCallbackWithResult, in UnityAction[] errorCallback, in string URL, bool useInfoError, bool isAsyinc, params ContainerString[] containers)
        {
            _auxData = new List<IMultipartFormSection>();

            _resultCallbackWithResult = resultCallbackWithResult;
            _resultCallback = resultCallback;
            _errorCallback = errorCallback;
            _useInfoError = useInfoError;

            if (containers != null)
            {
                foreach (var container in containers)
                {
                    try
                    {
                        _auxData.Add(new MultipartFormDataSection(container.title, container.value));
                    }
                    catch (ArgumentException e)
                    {
                        LogManager.LogWarning(e);
                        ExecuteError("ErrorCode: 03");
                        return;
                    }
                }
            }

            string completeURL = URLDomain + URL;
            UnityEngine.Debug.Log(completeURL);

            if (isAsyinc)
            {
                StartCoroutine(CreateRequestAsyinc(_auxData, _resultCallbackWithResult, completeURL));
            }
            else
            {
                CreateRequestSyinc(_auxData, _resultCallbackWithResult, completeURL);
            }
        }

        private IEnumerator CreateRequestAsyinc(List<IMultipartFormSection> formData, UnityAction<string>[] resultCallback, string URL)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(URL, formData))
            {
                www.certificateHandler = new AcceptAllCertificates();

                UnityWebRequestAsyncOperation request = null;
                try
                {
                    request = www.SendWebRequest();
                }
                catch (Exception e)
                {
                    ExecuteError(e.ToString());
                }

                yield return request;
                FinishRequest(www);
            }
        }

        private void CreateRequestSyinc(List<IMultipartFormSection> formData, UnityAction<string>[] resultCallback, string URL)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(URL, formData))
            {
                www.certificateHandler = new AcceptAllCertificates();
                try
                {
                    www.SendWebRequest();
                }
                catch (Exception e)
                {
                    ExecuteError(e.ToString());
                }

                while (!www.isDone)
                {
                }

                FinishRequest(www);
            }
        }

        private void FinishRequest(UnityWebRequest www)
        {
            string answer = www.downloadHandler != null ? www.downloadHandler.text : string.Empty;
            bool error = answer.ContainsIgnoreCamelCase("error");

            if (www.result == UnityWebRequest.Result.ProtocolError ||
                www.result == UnityWebRequest.Result.DataProcessingError ||
                www.result == UnityWebRequest.Result.ConnectionError)
            {
                string result = www.result.ToString();
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    result = "ErrorCode: 02";
                }

                UnityEngine.Debug.Log(www.error);
                ExecuteError(result);
            }
            else if (error)
            {
                ExecuteError(answer);
            }
            else
            {
                ExecuteRightAction(answer);
            }
        }

        private void ExecuteRightAction(in string answer)
        {
            if (_useResult)
            {
                foreach (var action in _resultCallbackWithResult)
                {
                    action?.Invoke(answer);
                }
            }
            else
            {
                foreach (var action in _resultCallback)
                {
                    action?.Invoke();
                }
            }
        }

        private void ExecuteError(in string textError)
        {
            LogManager.Log(textError);

            if (_useInfoError)
            {
                string description = GetDescrption(textError.ToString());

                if (_errorCallback != null && _errorCallback.ThereIsntNullInChilds())
                {
                    ButtonInfo buttonInfo = new("confirm", _errorCallback, true);
                    GenerateQuestionarie.GenerateQuestionarieUI(description, true, "", buttonInfo);
                }
                else
                {
                    GenerateQuestionarie.GenerateQuestionarieUI(description, true, "confirm");
                }
            }
            else if (_errorCallback != null)
            {
                foreach (var action in _errorCallback)
                {
                    action?.Invoke();
                }
            }
        }

        private string GetDescrption(string text)
        {
            if (text != null)
            {
                string desciption = null;
                foreach (var container in errorDescription)
                {
                    if (text.ContainsIgnoreCamelCase(container.title))
                    {
                        if (container.useReference)
                        {
#if INK
                            desciption = StoryExtend.GetSingleText(container.storyIndex);
#endif
                            break;
                        }
                        else
                        {
                            desciption = container.value;
                            break;
                        }
                    }
                }

                if (desciption == null)
                {
#if INK
                    desciption = StoryExtend.GetSingleText(new StoryIndex("variousText", "OtherOnlineProblem"));
#endif
                }

                return desciption;

            }
            return null;
        }
    }
}
