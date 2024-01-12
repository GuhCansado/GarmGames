using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEngine;

namespace NinjutsuGames.Photon.Editor
{
    public class ModelConfigItemTool : TPolymorphicItemTool
    {
        private static readonly IIcon DEFAULT_ICON = new IconCharacter(ColorTheme.Type.Yellow);

        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.VISUAL_SCRIPTING + "Instructions/StyleSheets/Instruction-Head",
            EditorPaths.VISUAL_SCRIPTING + "Instructions/StyleSheets/Instruction-Body"
        };

        protected override object Value => this.m_Property.GetValue<ModelConfig>();

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ModelConfigItemTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }
        
        protected override Texture2D GetIcon() => DEFAULT_ICON.Texture;

        public override string Title => Value.ToString();
    }
}