using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Managers
{
    [CustomEditor(typeof(PhotonLocalModelsList), true)]
    public class PhotonLocalModelsListEditor : UnityEditor.Editor
    {
        private static readonly StyleLength DEFAULT_MARGIN_TOP = new(5);

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            root.style.marginTop = DEFAULT_MARGIN_TOP;
            var property = serializedObject.FindProperty("models");
            root.Add(new PropertyField(property));
            return root;
        }
        
        [MenuItem("GameObject/Game Creator/Photon/Photon Local Models List", false, 0)]
        public static void CreateInstance(MenuCommand menuCommand)
        {
            var reg = FindObjectOfType<PhotonLocalModelsList>();
            if(reg != null)
            {
                Selection.activeObject = reg;
                return;
            }
            var instance = new GameObject("PhotonLocalModelsList");
            instance.AddComponent<PhotonLocalModelsList>();
            GameObjectUtility.SetParentAndAlign(instance, menuCommand?.context as GameObject);
            Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
            Selection.activeObject = instance;
        }
    }
}