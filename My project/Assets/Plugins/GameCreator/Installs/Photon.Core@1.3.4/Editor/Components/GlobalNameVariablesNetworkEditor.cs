using NinjutsuGames.Photon.Runtime.Components;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Components
{
    [CustomEditor(typeof(GlobalNameVariablesNetwork))]
    public class GlobalNameVariablesNetworkEditor : UnityEditor.Editor
    {
        private static readonly StyleLength DefaultMarginTop = new(5);

        // MEMBERS: -------------------------------------------------------------------------------
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            container.style.marginTop = DefaultMarginTop;
            container.Add(new PropertyField(serializedObject.FindProperty("globalNameVariables")));
            return container;
        }
    }
}