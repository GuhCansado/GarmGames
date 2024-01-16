using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    [CustomPropertyDrawer(typeof(FieldGetNodeStateMachine))]
    public class FieldGetNodeStateMachineDrawer : PropertyDrawer
    {
        private const string Pattern = @"\[([0-9]+)\]";
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            var stateMachine = property.FindPropertyRelative("m_StateMachine");
            var nodeName = property.FindPropertyRelative("m_Name");
            var nodeId = property.FindPropertyRelative("m_GUID");

            var fieldVariable = new ObjectField(stateMachine.displayName)
            {
                allowSceneObjects = false,
                objectType = typeof(StateMachineAsset),
                bindingPath = stateMachine.propertyPath
            };
            
            var toolPickName = new StateMachinePickNodeTool(
                fieldVariable, 
                property,
                nodeName.stringValue,
                nodeId.stringValue,
                true
            );
            
            var matches = Regex.Matches(property.propertyPath, Pattern);
            if (matches.Count > 0)
            {
                var arrayIndices = new List<int>();
                foreach (Match match in matches)
                {
                    arrayIndices.Add(int.Parse(match.Groups[1].Value));
                }
                if(arrayIndices.Count > 0)
                {
                    var arrayIndex = property.serializedObject.targetObject is StateMachineAsset ? arrayIndices[1] : arrayIndices[0];
                    var parentPropertyPath = property.propertyPath.Substring(0, property.propertyPath.LastIndexOf(".data", StringComparison.Ordinal));
                    var arrayProperty = property.serializedObject.FindProperty(parentPropertyPath);
                    if(arrayIndex <= arrayProperty.arraySize)
                    {
                        var arrayElementProperty = arrayProperty?.GetArrayElementAtIndex(arrayIndex);
                        var variableProperty = arrayElementProperty?.FindPropertyRelative("m_StateMachine");
                        if (variableProperty != null)
                        {
                            fieldVariable.RegisterValueChangedCallback(e =>
                            {
                                variableProperty.objectReferenceValue = e.newValue;
                                SerializationUtils.ApplyUnregisteredSerialization(property.serializedObject);
                            });
                            variableProperty.objectReferenceValue = stateMachine.objectReferenceValue;
                            SerializationUtils.ApplyUnregisteredSerialization(property.serializedObject);
                        }
                    }
                }
            }
            
            fieldVariable.RegisterValueChangedCallback(toolPickName.OnChangeAsset);

            root.Add(fieldVariable);
            root.Add(toolPickName);
            
            AlignLabel.On(root);
            
            property.serializedObject.Update();
            if(StateMachineAsset.Active != null && stateMachine.objectReferenceValue == null && stateMachine.objectReferenceValue != StateMachineAsset.Active)
            {
                stateMachine.objectReferenceValue = StateMachineAsset.Active;
                SerializationUtils.ApplyUnregisteredSerialization(property.serializedObject);
            }

            return root;
        }
    }
}