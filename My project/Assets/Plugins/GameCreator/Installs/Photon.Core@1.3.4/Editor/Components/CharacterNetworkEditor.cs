using UnityEditor;
using UnityEditor.UIElements;
using NinjutsuGames.Photon.Runtime.Components;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Components
{
    [CustomEditor(typeof(CharacterNetwork), true)]
    public class CharacterNetworkEditor : UnityEditor.Editor
    {
        protected static readonly StyleLength DefaultMarginTop = new StyleLength(5);
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement container = new VisualElement();
            container.style.marginTop = DefaultMarginTop;

            container.Add(new PropertyField(serializedObject.FindProperty("smoothTime")));
            container.Add(new PropertyField(serializedObject.FindProperty("teleportIfDistance")));
            return container;
        }
    }
}