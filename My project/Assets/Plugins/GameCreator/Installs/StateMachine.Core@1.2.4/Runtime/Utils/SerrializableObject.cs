using System;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
#endif

namespace NinjutsuGames.StateMachine.Runtime
{
    // Warning: this class only support the serialization of UnityObject and primitive
    [Serializable]
    public class SerializableObject
    {
        [Serializable]
        class ObjectWrapper
        {
            public Object value;
        }

        public string serializedType;
        public string serializedName;
        public string serializedValue;

        public object value;

        public SerializableObject(object value, Type type, string name = null)
        {
            this.value = value;
            serializedName = name;
            serializedType = type.AssemblyQualifiedName;
        }

        public void Deserialize()
        {
            if (String.IsNullOrEmpty(serializedType))
            {
                Debug.LogError("Can't deserialize the object from null type");
                return;
            }

            Type type = Type.GetType(serializedType);

            if (type.IsPrimitive)
            {
                if (string.IsNullOrEmpty(serializedValue))
                    value = Activator.CreateInstance(type);
                else
                    value = Convert.ChangeType(serializedValue, type, CultureInfo.InvariantCulture);
            }
            else if (typeof(Object).IsAssignableFrom(type))
            {
                ObjectWrapper obj = new ObjectWrapper();
                JsonUtility.FromJsonOverwrite(serializedValue, obj);
                value = obj.value;
            }
            else if (type == typeof(string))
                value = serializedValue.Length > 1 ? serializedValue.Substring(1, serializedValue.Length - 2).Replace("\\\"", "\"") : "";
            else
            {
                try
                {
                    value = Activator.CreateInstance(type);
                    JsonUtility.FromJsonOverwrite(serializedValue, value);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    Debug.LogError("Can't serialize type " + serializedType);
                }
            }
        }

        public void Serialize()
        {
            if (value == null)
                return;

            serializedType = value.GetType().AssemblyQualifiedName;

            if (value.GetType().IsPrimitive)
                serializedValue = Convert.ToString(value, CultureInfo.InvariantCulture);
            else if (value is Object) //type is a unity object
            {
                if ((value as Object) == null)
                    return;

                ObjectWrapper wrapper = new ObjectWrapper {value = value as Object};
                serializedValue = JsonUtility.ToJson(wrapper);
            }
            else if (value is string)
                serializedValue = "\"" + ((string) value).Replace("\"", "\\\"") + "\"";
            else
            {
                try
                {
                    serializedValue = JsonUtility.ToJson(value);
                    if (String.IsNullOrEmpty(serializedValue))
                        throw new Exception();
                }
                catch
                {
                    Debug.LogError("Can't serialize type " + serializedType);
                }
            }
        }
    }
}