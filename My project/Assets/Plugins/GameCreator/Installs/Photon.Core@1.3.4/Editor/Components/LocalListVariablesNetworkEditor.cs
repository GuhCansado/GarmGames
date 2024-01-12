using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Components
{
    [CustomEditor(typeof(LocalListVariablesNetwork))]
    public class LocalListVariablesNetworkEditor : UnityEditor.Editor
    {
        private static readonly StyleLength DefaultMarginTop = new(5);

        // MEMBERS: -------------------------------------------------------------------------------
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            container.style.marginTop = DefaultMarginTop;
            container.Add(new PropertyField(serializedObject.FindProperty("m_SyncMode")));
            return container;
        }
    }
}