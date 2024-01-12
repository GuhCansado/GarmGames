using GameCreator.Editor.Common;
using GameCreator.Editor.Variables;
using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor
{
    [CustomPropertyDrawer(typeof(FieldGetPhotonLocalModelsList))]
    public class FieldGetPhotonLocalModelsListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            var propertyVariable = property.FindPropertyRelative("m_ModelsList");
            var propertySelect = property.FindPropertyRelative("m_Select");

            var fieldVariable = new PropertyField(propertyVariable);
            var fieldSelect = new PropertyElement(propertySelect, " ", false);
            
            root.Add(fieldVariable);
            root.Add(fieldSelect);

            return root;
        }
    }
}