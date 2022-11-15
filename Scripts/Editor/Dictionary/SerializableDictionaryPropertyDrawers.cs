using Pearl.CustomInspector;
using UnityEditor;

namespace Pearl
{
    [CustomPropertyDrawer(typeof(InputDeviceEnumImageDictionary))]
    [CustomPropertyDrawer(typeof(StringButtonImageForDeviceDictionary))]
    [CustomPropertyDrawer(typeof(StringGameobjectDictionary))]
    [CustomPropertyDrawer(typeof(StringBoolDictionary))]
    [CustomPropertyDrawer(typeof(StringInfoTriggerDictionary))]
    [CustomPropertyDrawer(typeof(IntegerTansformDictionary))]
    [CustomPropertyDrawer(typeof(StringAudioClipDictionary))]
    [CustomPropertyDrawer(typeof(InputDeviceEnumstringDictionary))]
    [CustomPropertyDrawer(typeof(StringButtonTextForDeviceDictionary))]
    [CustomPropertyDrawer(typeof(SystemLanguageTextAssetDictionary))]
    [CustomPropertyDrawer(typeof(StringSystemLanguageTextAssetDictionaryDictionary))]
    [CustomPropertyDrawer(typeof(StateLogosFadeInfoDictionary))]
    [CustomPropertyDrawer(typeof(IntegerIntegerDictionary))]
    [CustomPropertyDrawer(typeof(StringSpriteDictionary))]
    [CustomPropertyDrawer(typeof(IntegerFloatDictionary))]
    [CustomPropertyDrawer(typeof(StringLogInfoDictionary))]
    [CustomPropertyDrawer(typeof(IntegerSpriteDictionary))]
    [CustomPropertyDrawer(typeof(TransformFloatDictionary))]
    [CustomPropertyDrawer(typeof(StringStringDictionary))]
    [CustomPropertyDrawer(typeof(StringEventDictionary))]
    [CustomPropertyDrawer(typeof(StringIntDictionary))]
    [CustomPropertyDrawer(typeof(FloatGameObjctDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }


    public class AnySerializableDictionaryStoragePropertyDrawer : SerializableDictionaryStoragePropertyDrawer { }
}