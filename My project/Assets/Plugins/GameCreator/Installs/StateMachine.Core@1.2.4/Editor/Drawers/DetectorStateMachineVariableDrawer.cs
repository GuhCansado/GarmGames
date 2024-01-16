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
    [CustomPropertyDrawer(typeof(DetectorStateMachine))]

    public class DetectorStateMachineVariableDrawer : TDetectorStateMachineVariableDrawer
    {
        protected override Type AssetType => typeof(StateMachineAsset);
        protected override bool AllowSceneReferences => false;
        protected override bool AutoSelectReference => true;

        protected override TNamePickTool Tool(ObjectField field, SerializedProperty property)
        {
            var namePickTool = new StateMachinePickTool(property);
            field.RegisterValueChangedCallback(_ => namePickTool.OnChangeAsset());
            
            property.serializedObject.Update();
            if(StateMachineAsset.Active != null && field.value == null && field.value != StateMachineAsset.Active)
            {
                field.value = StateMachineAsset.Active;
                SerializationUtils.ApplyUnregisteredSerialization(property.serializedObject);
            }
            return namePickTool;
        }
    }
}