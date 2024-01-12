using NinjutsuGames.Photon.Runtime.Comparers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Common
{
    [CustomPropertyDrawer(typeof(CompareClientState))]
    public class CompareClientStateDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            var comparison = property.FindPropertyRelative("m_Comparison");
            var compareTo = property.FindPropertyRelative("m_CompareTo");
            
            var fieldComparison = new PropertyField(comparison);
            var fieldCompareTo = new PropertyField(compareTo, property.displayName);

            root.Add(fieldComparison);
            root.Add(fieldCompareTo);
            
            return root;
        }
    }
}