using NinjutsuGames.Photon.Runtime.Components;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Components
{
    [CustomEditor(typeof(VisualScriptingNetwork))]
    public class VisualScriptingNetworkEditor : UnityEditor.Editor
    {
        protected static readonly StyleLength DefaultMarginTop = new(5);
        
        private void OnEnable()
        {
            var t = (VisualScriptingNetwork) target;
            t.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
        }
        
        private void OnValidate()
        {
            var t = (VisualScriptingNetwork) target;
            t.CleanUp();
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement container = new VisualElement();
            container.style.marginTop = DefaultMarginTop;

            var property = serializedObject.FindProperty("actions");
            var field = new PropertyField(property);
            container.Add(field);
            return container;
        }
    }
}