using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Editor.VisualScripting;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEditor;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Tools
{
    public class GameObjectListTool : TPolymorphicListTool
    {
        private const string NAME_BUTTON_ADD = "GC-Instruction-List-Foot-Add";
        
        private const string CLASS_INSTRUCTION_RUNNING = "gc-list-item-head-running";

        private static readonly IIcon ICON_PASTE = new IconPaste(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_PLAY = new IconPlay(ColorTheme.Type.TextNormal);

        // MEMBERS: -------------------------------------------------------------------------------

        protected Button m_ButtonAdd;
        protected Button m_ButtonPaste;
        protected Button m_ButtonPlay;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string ElementNameHead => "GC-Instruction-List-Head";
        protected override string ElementNameBody => "GC-Instruction-List-Body";
        protected override string ElementNameFoot => "GC-Instruction-List-Foot";

        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.VISUAL_SCRIPTING + "Instructions/StyleSheets/Instructions-List"
        };

        public override bool AllowReordering => true;
        public override bool AllowDuplicating => true;
        public override bool AllowDeleting  => true;
        public override bool AllowContextMenu => true;
        public override bool AllowCopyPaste => true;
        public override bool AllowInsertion => true;
        public override bool AllowBreakpoint => true;
        public override bool AllowDisable => true;
        public override bool AllowDocumentation => true;
        
        private BaseActions Actions => this.SerializedObject?.targetObject as BaseActions;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public GameObjectListTool(SerializedProperty property)
            : base(property, InstructionListDrawer.NAME_INSTRUCTIONS)
        {
            this.SerializedObject.Update();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override VisualElement MakeItemTool(int index)
        {
            return new InstructionItemTool(this, index);
        }

        protected override void SetupHead()
        { }

        protected override void SetupFoot()
        {
            base.SetupFoot();
            
            /*this.m_ButtonAdd = new TypeSelectorElementInstruction(this.PropertyList, this)
            {
                name = NAME_BUTTON_ADD
            };*/
            
            /*this.m_ButtonPaste = new Button(() =>
            {
                if (!CopyPasteUtils.CanSoftPaste(typeof(Instruction))) return;
                
                int pasteIndex = this.PropertyList.arraySize;
                this.InsertItem(pasteIndex, CopyPasteUtils.SourceObjectCopy);
            })
            
            {
                name = "GC-Instruction-List-Foot-Button"
            };*/
            
            this.m_ButtonPaste.Add(new Image
            {
                image = ICON_PASTE.Texture
            });
            
            /*this.m_ButtonPlay = new Button(this.RunInstructions)
            {
                name = "GC-Instruction-List-Foot-Button"
            };*/
            
            this.m_ButtonPlay.Add(new Image
            {
                image = ICON_PLAY.Texture
            });
            
            // this.m_Foot.Add(this.m_ButtonAdd);
            // this.m_Foot.Add(this.m_ButtonPaste);
            // this.m_Foot.Add(this.m_ButtonPlay);
            
            this.m_ButtonPlay.style.display = this.Actions != null
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
    }
}