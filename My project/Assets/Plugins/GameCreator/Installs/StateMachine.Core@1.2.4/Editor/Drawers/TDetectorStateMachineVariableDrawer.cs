using System;
using GameCreator.Editor.Common;
using GameCreator.Editor.Variables;
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    public abstract class TDetectorStateMachineVariableDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var head = new VisualElement();
            var body = new VisualElement();

            root.Add(head);
            root.Add(body);

            var variables = property.FindPropertyRelative("m_Variable");
            var when = property.FindPropertyRelative("m_When");
            
            var fieldVariable = new ObjectField(variables.displayName)
            {
                allowSceneObjects = AllowSceneReferences,
                objectType = AssetType,
                bindingPath = variables.propertyPath
            };
            fieldVariable.AddToClassList(AlignLabel.CLASS_UNITY_ALIGN_LABEL);
            
            var fieldWhen = new PropertyField(when);
            
            head.Add(fieldVariable);
            head.Add(fieldWhen);
            
            fieldWhen.RegisterValueChangeCallback(_ =>
            {
                body.Clear();
                if (when.enumValueIndex == 1) // When.Name
                {
                    body.Add(Tool(fieldVariable, property));
                }
            });

            if (when.enumValueIndex == 1) // When.Name
            {
                body.Add(Tool(fieldVariable, property));
            }
            property.serializedObject.Update();
            if(when.enumValueIndex == 0)
            {
                if (AutoSelectReference && StateMachineAsset.Active != null && variables.objectReferenceValue == null && variables.objectReferenceValue != StateMachineAsset.Active)
                {
                    variables.objectReferenceValue = StateMachineAsset.Active;
                    SerializationUtils.ApplyUnregisteredSerialization(property.serializedObject);
                }
            }
            else
            {
                if (AutoSelectReference && StateMachineAsset.Active != null && fieldVariable.value == null && fieldVariable.value != StateMachineAsset.Active)
                {
                    fieldVariable.value = StateMachineAsset.Active;
                    SerializationUtils.ApplyUnregisteredSerialization(property.serializedObject);
                }
            }
            
            return root;
        }

        protected abstract Type AssetType { get; }
        protected abstract bool AllowSceneReferences { get; }
        protected abstract bool AutoSelectReference { get; }
        
        protected abstract TNamePickTool Tool(ObjectField field, SerializedProperty property);

    }
}