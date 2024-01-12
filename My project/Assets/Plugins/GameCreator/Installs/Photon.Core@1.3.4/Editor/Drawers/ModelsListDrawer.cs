using GameCreator.Editor.Common;
using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor
{
    [CustomPropertyDrawer(typeof(ModelsList))]
    public class ModelsListDrawer : PropertyDrawer
    {
        // protected override string PropertyArrayName => "m_Models";
        // protected override float ItemHeight => 122f;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var instructionListTool = new ModelConfigListTool(
                property
            );
            root.Add(instructionListTool);
            return root;
        }
    }
}