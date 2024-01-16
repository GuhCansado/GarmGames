using System.Linq;
using System.Reflection;
using GameCreator.Editor.Common;
using GameCreator.Editor.Variables;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    public class RunnerNameVariableTool : TPolymorphicItemTool
    {
        private const string PROP_NAME = "m_Name";
        private const string PROP_VALUE = "m_Value";
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => this.Variable?.Title;

        private TVariable Variable => (this.ParentTool as RunnerNameListTool)?.NameList.Get(this.Index);
        
        protected override object Value => this.m_Property.GetValue<NameVariable>();

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public RunnerNameVariableTool(IPolymorphicListTool parentTool)
            : base(parentTool, 0)
        { }

        // OVERRIDERS: ----------------------------------------------------------------------------

        protected override void SetupBody()
        {
            this.m_Property.serializedObject.Update();

            var field = new PropertyField(this.m_Property);
            field.Bind(this.m_Property.serializedObject);
            
            field.RegisterValueChangeCallback(_ =>
            {
                this.m_Property.serializedObject.Update();
                this.UpdateHead();
            });

            this.m_Body.Add(field);
            this.UpdateBody(false);
        }

        protected override Texture2D GetIcon()
        {
            this.m_Property.serializedObject.Update();
            
            if(Variable == null) return Texture2D.whiteTexture;
            
            var instance = this.Variable.GetType()
                .GetField(PROP_VALUE, BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(this.Variable) as TValue;

            var iconAttrs = instance?.GetType()
                .GetCustomAttributes<ImageAttribute>()
                .FirstOrDefault();
            
            var icon = iconAttrs?.Image;
            return icon != null ? icon : Texture2D.whiteTexture;
        }

        protected override void OnChangePlayMode(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    this.m_Body?.SetEnabled(false);
                    break;
                
                default:
                    this.m_Body?.SetEnabled(true);
                    break;
            }
        }
    }
    public class RunnerNameListTool : NameListTool
    {
        protected const string USS_PATH = EditorPaths.VARIABLES + "StyleSheets/NameList";
        public override bool AllowReordering => true;
        public override bool AllowDuplicating => true;
        public override bool AllowDeleting  => true;
        public override bool AllowContextMenu => false;
        public override bool AllowCopyPaste => true;
        public override bool AllowInsertion => true;
        public override bool AllowBreakpoint => false;
        public override bool AllowDisable => false;
        public override bool AllowDocumentation => false;

        public RunnerNameListTool(SerializedProperty propertyRoot) : base(propertyRoot)
        {
            var sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (var styleSheet in sheets) this.styleSheets.Add(styleSheet);
        }
    }

    public class BlackboardNameListTool : RunnerNameListTool
    {
        public override bool AllowReordering => false;
        public BlackboardNameListTool(SerializedProperty propertyRoot) : base(propertyRoot)
        {
            var sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (var styleSheet in sheets) this.styleSheets.Add(styleSheet);
        }
    }
}