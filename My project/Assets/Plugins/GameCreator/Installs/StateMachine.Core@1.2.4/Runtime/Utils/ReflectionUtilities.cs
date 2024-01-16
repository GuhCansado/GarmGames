#if UNITY_EDITOR
using System.Reflection;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;

namespace NinjutsuGames.StateMachine.Runtime
{
    public static class ReflectionUtilities
    {
        public static void SetUniqueId(Character character, string id)
        {
            character.GetType().GetField("m_UniqueID", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(character, new UniqueID(id));
        }
        
        public static NameVariableRuntime GetRuntimeVariables(this LocalNameVariables variables)
        {
            return GetPrivateFieldValue<NameVariableRuntime>(variables, "m_Runtime");
        }
        
        public static Event GetTriggerEvent(this Trigger trigger)
        {
            return GetPrivateFieldValue<Event>(trigger, "m_TriggerEvent");
        }
        
        public static InstructionList GetInstructionsList(this Actions actions)
        {
            return GetPrivateFieldValue<InstructionList>(actions, "m_Instructions");
        }
        
        public static T GetPrivateFieldValue<T>(object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field.GetValue(instance);
        }
        
        public static void SetVariablesUniqueId(LocalNameVariables variables, string id)
        {
            variables.GetType().GetField("m_SaveUniqueID", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(variables, new SaveUniqueID(true, id));
        }
    }
}
#endif