using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pearl
{
    public class InformationAttribute : PropertyAttribute
    {

#if UNITY_EDITOR
        public string Message;
        public MessageType Type;
        public bool MessageAfterProperty;

        public InformationAttribute(string message, InformationTypeEnum type, bool messageAfterProperty)
        {
            this.Message = message;
            if (type == InformationTypeEnum.Error) { this.Type = UnityEditor.MessageType.Error; }
            if (type == InformationTypeEnum.Info) { this.Type = UnityEditor.MessageType.Info; }
            if (type == InformationTypeEnum.Warning) { this.Type = UnityEditor.MessageType.Warning; }
            if (type == InformationTypeEnum.None) { this.Type = UnityEditor.MessageType.None; }
            this.MessageAfterProperty = messageAfterProperty;
        }
#else
		public InformationAttribute(string message, InformationTypeEnum type, bool messageAfterProperty)
		{

		}
#endif
    }
}