using GameCreator.Editor.Common;
using GameCreator.Editor.Variables;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    [CustomPropertyDrawer(typeof(FieldSetStateMachine))]
    public class FieldSetStateMachineDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            var variable = property.FindPropertyRelative("m_Variable");
            var typeID = property.FindPropertyRelative("m_TypeID");

            var fieldVariable = new ObjectField(variable.displayName)
            {
                allowSceneObjects = false,
                objectType = typeof(StateMachineAsset),
                bindingPath = variable.propertyPath
            };
            
            var typeIDStr = typeID.FindPropertyRelative(IdStringDrawer.NAME_STRING);
            var typeIDValue = new IdString(typeIDStr.stringValue);
            
            var toolPickName = new StateMachinePickTool(
                property,
                typeIDValue,
                false
            );
            
            fieldVariable.RegisterValueChangedCallback(_ => toolPickName.OnChangeAsset());

            root.Add(fieldVariable);
            root.Add(toolPickName);
          
            property.serializedObject.Update();
            if(StateMachineAsset.Active != null && variable.objectReferenceValue == null && variable.objectReferenceValue != StateMachineAsset.Active)
            {
                variable.objectReferenceValue = StateMachineAsset.Active;
                SerializationUtils.ApplyUnregisteredSerialization(property.serializedObject);
            }

            return root;
        }
    }
}