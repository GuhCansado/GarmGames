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
    public class StateMachinePickTool : TNamePickTool
    {
        private TextField m_NameField;
        private VisualElement m_NameDropdown;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StateMachinePickTool(SerializedProperty property)
            : base(property, true, false, ValueNull.TYPE_ID)
        { }
        
        public StateMachinePickTool(SerializedProperty property, IdString typeID, bool allowCast)
            : base(property, false, allowCast, typeID)
        { }

        protected override Object Asset => m_PropertyVariable.objectReferenceValue;

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
            
            m_NameDropdown.SetEnabled(asset != null);
            m_NameDropdown.AddManipulator(new MouseDropdownManipulator(context =>
            {
                var listNames = GetVariablesList(asset);
                foreach (var entry in listNames)
                {
                    context.menu.AppendAction(
                        entry.Key,
                        menuAction =>
                        {
                            m_PropertyName.serializedObject.Update();
                            m_PropertyName.stringValue = menuAction.name;
            
                            m_PropertyName.serializedObject.ApplyModifiedProperties();
                            m_PropertyName.serializedObject.Update();
                        },
                        menuAction =>
                        {
                            if (menuAction.name != m_PropertyName.stringValue)
                            {
                                return entry.Value
                                    ? DropdownMenuAction.Status.Normal
                                    : DropdownMenuAction.Status.Disabled;
                            }
                            
                            return DropdownMenuAction.Status.Checked;
                        }
                    );
                }
            }));
            
            var nameContainer = new VisualElement { name = NAME_ROOT_NAME };
            
            nameContainer.Add(new Label(" "));
            nameContainer.Add(m_NameField);
            nameContainer.Add(m_NameDropdown);
            
            AlignLabel.On(nameContainer);

            Add(nameContainer);
        }

        private Dictionary<string, bool> GetVariablesList(Object asset)
        {
            var variable = asset as StateMachineAsset;
            
            if (variable == null) return new Dictionary<string, bool> {{ string.Empty, false }};

            var names = variable.GetType()
                .GetField("m_NameList", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(variable) as NameList;

            return FilterNames(names);
        }
    }
}