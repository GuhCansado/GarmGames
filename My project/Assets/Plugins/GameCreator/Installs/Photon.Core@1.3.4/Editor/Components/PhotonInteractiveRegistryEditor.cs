using UnityEditor;
using UnityEditor.UIElements;
using NinjutsuGames.Photon.Runtime.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Managers
{
    [CustomEditor(typeof(PhotonInteractiveRegistry), true)]
    public class PhotonInteractiveRegistryEditor : UnityEditor.Editor
    {
        protected static readonly StyleLength DEFAULT_MARGIN_TOP = new(5);

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            root.style.marginTop = DEFAULT_MARGIN_TOP;
            var property = serializedObject.FindProperty("interactiveInstances");
            var l = new ListView
            {
                headerTitle = "Interactive Instances",
                reorderMode = ListViewReorderMode.Animated,
                showFoldoutHeader = false,
                reorderable = true,
                showBoundCollectionSize = false,
                style =
                {
                    flexGrow = 1.0f
                },
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                showBorder = true,
                showAddRemoveFooter = true,
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                horizontalScrollingEnabled = false,
            };
            l.BindProperty(property);
            root.Add(l);
            
            var b = new Button
            {
                text = "Find Instances"
            };
            b.clicked += () =>
            {
                serializedObject.Update();
                (serializedObject.targetObject as PhotonInteractiveRegistry).FindInteractiveInstances();
                serializedObject.ApplyModifiedProperties();
            };
            VisualElement but = b;
            but.name = "Find";
            root.Add(but);
            return root;
        }
        
        [MenuItem("GameObject/Game Creator/Photon/Interactive Registry", false, 0)]
        public static void CreateInstance(MenuCommand menuCommand)
        {
            var reg = FindObjectOfType<PhotonInteractiveRegistry>();
            if(reg != null)
            {
                Selection.activeObject = reg;
                return;
            }
            var instance = new GameObject("Photon Interactive Registry");
            var registry = instance.AddComponent<PhotonInteractiveRegistry>();
            registry.FindInteractiveInstances();
            GameObjectUtility.SetParentAndAlign(instance, menuCommand?.context as GameObject);
            Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
            Selection.activeObject = instance;
        }
    }
}