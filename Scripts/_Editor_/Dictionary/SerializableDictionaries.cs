using Pearl.CustomInspector;
using Pearl.Events;
using Pearl.Input;
using Pearl.UI;
using System;
using UnityEngine;
using static Pearl.Debug.LogManager;
using static Pearl.Editor.ButtonImageForCommandScriptableObject;
using static Pearl.Editor.ButtonTextForCommandScriptableObject;

namespace Pearl
{
    [Serializable]
    public struct Event
    {
        public SimpleEvent ev;
    }

    [Serializable]
    public class FloatGameObjctDictionary : SerializableDictionary<float, GameObject> { }

    [Serializable]
    public class StringIntDictionary : SerializableDictionary<string, int> { }

    [Serializable]
    public class StringGameobjectDictionary : SerializableDictionary<string, GameObject> { }

    [Serializable]
    public class StringBoolDictionary : SerializableDictionary<string, bool> { }

    [Serializable]
    public class StringLogInfoDictionary : SerializableDictionary<string, LogInfo> { }

    [Serializable]
    public class StringInfoTriggerDictionary : SerializableDictionary<string, InfoTrigger> { }

    [Serializable]
    public class IntegerTansformDictionary : SerializableDictionary<int, Transform> { }

    [Serializable]
    public class StringAudioClipDictionary : SerializableDictionary<string, AudioClip> { }

    [Serializable]
    public class InputDeviceEnumImageDictionary : SerializableDictionary<InputDeviceEnum, Sprite> { }

    [Serializable]
    public class StringButtonImageForDeviceDictionary : SerializableDictionary<string, ButtonImageForDevice> { }

    [Serializable]
    public class InputDeviceEnumstringDictionary : SerializableDictionary<InputDeviceEnum, string> { }

    [Serializable]
    public class StringButtonTextForDeviceDictionary : SerializableDictionary<string, ButtonTextForDevice> { }

    [Serializable]
    public class SystemLanguageTextAssetDictionary : SerializableDictionary<SystemLanguage, TextAsset> { }

    [Serializable]
    public class StringSystemLanguageTextAssetDictionaryDictionary : SerializableDictionary<string, SystemLanguageTextAssetDictionary> { }

    [Serializable]
    public class StateLogosFadeInfoDictionary : SerializableDictionary<StateImageFade, FadeInfo> { }

    [Serializable]
    public class IntegerIntegerDictionary : SerializableDictionary<int, int> { }

    [Serializable]
    public class StringSpriteDictionary : SerializableDictionary<string, Sprite> { }

    [Serializable]
    public class IntegerFloatDictionary : SerializableDictionary<int, float> { }

    [Serializable]
    public class IntegerSpriteDictionary : SerializableDictionary<int, Sprite> { }

    [Serializable]
    public class TransformFloatDictionary : SerializableDictionary<Transform, float> { }

    [Serializable]
    public class StringStringDictionary : SerializableDictionary<string, string> { }

    [Serializable]
    public class StringEventDictionary : SerializableDictionary<string, Event> { }
}
