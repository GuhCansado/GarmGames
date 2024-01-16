using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    public abstract class TNodeNamePickTool : VisualElement
    {
        private const string USS_PATH = EditorPaths.VARIABLES + "StyleSheets/NamePickTool";

        protected const string NAME_ROOT_NAME = "GC-NamePickTool-Name";
        protected const string NAME_DROPDOWN = "GC-NamePickTool-Dropdown";

        protected static readonly IIcon ICON_DROPDOWN = new IconArrowDropDown(ColorTheme.Type.TextLight);
        
        // MEMBERS: -------------------------------------------------------------------------------

        protected readonly SerializedProperty m_Property;
        
        private readonly SerializedProperty m_PropertyVariable;
        protected readonly SerializedProperty m_PropertyName;
        protected readonly SerializedProperty m_PropertyId;

        private readonly ObjectField m_Asset;
        
        protected readonly bool m_AllowAny;
        protected readonly bool m_AllowCast;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected string NodeName { get; }
        protected string NodeId { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TNodeNamePickTool(ObjectField asset, SerializedProperty property, 
            bool allowAny, bool allowCast, string nodeName, string nodeId)
        {
            var sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (var styleSheet in sheets)
            {
                styleSheets.Add(styleSheet);
            }
            
            m_Property = property;
            m_Property.serializedObject.Update();
            
            m_PropertyVariable = property.FindPropertyRelative("m_StateMachine");
            m_PropertyName = property.FindPropertyRelative($"m_Name");
            m_PropertyId = property.FindPropertyRelative($"m_GUID");

            m_AllowAny = allowAny;
            m_AllowCast = allowCast;
            
            NodeName = nodeName;
            NodeId = nodeId;
            m_Asset = asset;
            
            asset.UnregisterValueChangedCallback(OnChangeAsset);
            asset.RegisterValueChangedCallback(OnChangeAsset);
            
            RefreshPickList(m_PropertyVariable.objectReferenceValue);
        }

        public void OnChangeAsset(ChangeEvent<Object> changeEvent)
        {
            RefreshPickList(changeEvent.newValue);
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected virtual void RefreshPickList(Object asset)
        {
            Clear();
        }

        protected Dictionary<string, string> FilterNames(List<BaseNode> names)
        {
            var list = new Dictionary<string, string> {{ string.Empty, string.Empty }};
            
            for (var i = 0; i < names?.Count; ++i)
            {
                var baseNode = names[i];
                list[baseNode.GetCustomName()] = baseNode.GUID;

                if(baseNode.GetType() != typeof(BaseGameCreatorNode))
                {
                    list[baseNode.GetCustomName()] = baseNode.GUID;
                    continue;
                }
                /*if(baseNode is TriggerNode or StartNode or StateMachineNode)
                {
                    list[baseNode.GetCustomName()] = baseNode.GUID;
                    continue;
                }*/
                
                /*if (this.m_AllowAny)
                {
                    bool isNull = nameVariable.TypeID.Hash == ValueNull.TYPE_ID.Hash;
                    list[nameVariable.Name] = !isNull;
                    continue;
                }

                if (nameVariable.TypeID.Hash == this.TypeID.Hash)
                {
                    list[nameVariable.Name] = true;
                    continue;
                } 
                
                if (this.m_AllowCast && this.TypeID.Hash == ValueString.TYPE_ID.Hash)
                {
                    bool isNull = nameVariable.TypeID.Hash == ValueNull.TYPE_ID.Hash;
                    list[nameVariable.Name] = !isNull;
                    continue;
                }*/

                list[baseNode.GetCustomName()] = baseNode.GUID;
            }

            return list;
        }
    }
}