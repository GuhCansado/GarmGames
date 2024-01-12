using System.Reflection;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;

namespace NinjutsuGames.Photon.Runtime
{
    public static class ReflectionUtilities
    {
        public static NameVariableRuntime GetRuntimeVariables(this LocalNameVariables variables)
        {
            return GetPrivateFieldValue<NameVariableRuntime>(variables, "m_Runtime");
        }
        
        public static Event GetTriggerEvent(this Trigger trigger)
        {
            return GetPrivateFieldValue<Event>(trigger, "m_TriggerEvent");
        }
        
        public static T GetPrivateFieldValue<T>(object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field.GetValue(instance);
        }
        
        public static void SetPrivateFieldValue<T>(object instance, string fieldName, T value)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(instance, value);
        }
    }
}