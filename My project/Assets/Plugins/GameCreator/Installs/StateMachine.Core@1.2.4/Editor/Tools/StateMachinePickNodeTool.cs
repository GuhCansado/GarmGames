using System.Collections.Generic;
using System.Reflection;
using GameCreator.Editor.Common;
using GameCreator.Editor.Variables;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    public class StateMachinePickNodeTool : TNodeNamePickTool
    {
        private TextField m_NameField;
        private VisualElement m_NameDropdown;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StateMachinePickNodeTool(ObjectField asset, SerializedProperty property)
            : base(asset, property, true, false, string.Empty, string.Empty)
        { }
        
        public StateMachinePickNodeTool(ObjectField asset, SerializedProperty property, string nodeName, string nodeId, bool allowCast)
            : base(asset, property, false, allowCast, nodeName, nodeId)
        { }

        protected override void RefreshPickList(Object asset)
        {
            base.RefreshPickList(asset);

            m_NameField = new TextField(string.Empty)
            {
                bindingPath = m_PropertyName.propertyPath
            };
            
            m_NameField.Bind(m_Property.serializedObject);

            m_NameDropdown = new Image
            {
                image = ICON_DROPDOWN.Texture,
                name = NAME_DROPDOWN,
                focusable = true
            };
            
            /*StateMachineAsset stateMachine = asset as StateMachineAsset;
            DropdownMenu dropdownMenu = new DropdownMenu();
            for (int i = 0; i < stateMachine.nodes.Count; i++)
            {
                var node = stateMachine.nodes[i];
                if(node is TriggerNode or StateMachineNode or StartNode) continue;
                dropdownMenu.InsertAction(i, stateMachine.nodes[i].GetCustomName(), a =>
                {
                    
                });
            }*/
            m_NameDropdown.SetEnabled(asset != null);
            m_NameDropdown.AddManipulator(new MouseDropdownManipulator(context =>
            {
                var listNames = GetVariablesList(asset);
                foreach (var entry in listNames)
                {
                    context.menu.AppendAction(entry.Key,
                        menuAction =>
                        {
                            m_PropertyName.serializedObject.Update();
                            m_PropertyName.stringValue = menuAction.name;

                            m_PropertyName.serializedObject.ApplyModifiedProperties();
                            m_PropertyName.serializedObject.Update();
                            
                            m_PropertyId.serializedObject.Update();
                            m_PropertyId.stringValue = menuAction.userData.ToString();
                            
                            m_PropertyId.serializedObject.ApplyModifiedProperties();
                            m_PropertyId.serializedObject.Update();
                        },
                        menuAction => menuAction.name != m_PropertyName.stringValue ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Checked, entry.Value);
                }
            }));
            
            var nameContainer = new VisualElement { name = NAME_ROOT_NAME };
            
            // nameContainer.Add(dropdownMenu);
            nameContainer.Add(new Label(" "));
            nameContainer.Add(m_NameField);
            nameContainer.Add(m_NameDropdown);
            
            AlignLabel.On(nameContainer);

            Add(nameContainer);
        }

        private Dictionary<string, string> GetVariablesList(Object asset)
        {
            var variable = asset as StateMachineAsset;
            
            if (variable == null) return new Dictionary<string, string> {{ string.Empty, string.Empty }};
            return FilterNames(variable.nodes.FindAll(n => n is BaseGameCreatorNode));
        }
    }
}