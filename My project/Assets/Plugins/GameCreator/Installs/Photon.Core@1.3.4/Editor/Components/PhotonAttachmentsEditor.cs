using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using NinjutsuGames.Photon.Runtime.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Managers
{
    [CustomEditor(typeof(PhotonAttachments), true)]
    public class PhotonAttachmentsEditor : UnityEditor.Editor
    {
        protected static readonly StyleLength DEFAULT_MARGIN_TOP = new(5);

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            root.style.marginTop = DEFAULT_MARGIN_TOP;
            var property = serializedObject.FindProperty("attachments");
            var l = new ListView
            {
                headerTitle = "Attachments",
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
            return root;
        }
        
        [MenuItem("GameObject/Game Creator/Photon/Photon Attachments", false, 0)]
        public static void CreateInstance(MenuCommand menuCommand)
        {
            var reg = FindObjectOfType<PhotonAttachments>();
            if(reg != null)
            {
                Selection.activeObject = reg;
                return;
            }
            var instance = new GameObject("Photon Attachments");
            instance.AddComponent<PhotonAttachments>();
            GameObjectUtility.SetParentAndAlign(instance, menuCommand?.context as GameObject);
            Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
            Selection.activeObject = instance;
        }
    }
}