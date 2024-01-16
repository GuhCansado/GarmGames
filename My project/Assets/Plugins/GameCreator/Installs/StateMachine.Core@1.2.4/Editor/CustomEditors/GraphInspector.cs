using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    public class GraphInspector : UnityEditor.Editor
    {
        public static Action OnListChanged;
        private const string USS_PATH = EditorPaths.VARIABLES + "StyleSheets/RuntimeGlobalList";

        private const string NAME_LIST = "GC-RuntimeGlobal-List-Head";
        private const string CLASS_LIST_ELEMENT = "gc-runtime-global-list-element";

        private const string ERR_DUPLICATE_ID = "Another Variable has the same ID";

        protected const string PROP_SAVE_UNIQUE_ID = "m_SaveUniqueID";

        // MEMBERS: -------------------------------------------------------------------------------

        protected VisualElement m_Root;
        protected VisualElement m_Head;
        protected VisualElement m_Body;

        protected ErrorMessage m_MessageID;

        protected StateMachineAsset graph;

        public sealed override VisualElement CreateInspectorGUI()
        {
            // root = base.CreateInspectorGUI();

            m_Root = new VisualElement();
            m_Head = new VisualElement();
            m_Body = new VisualElement();

            m_Root.Add(m_Head);
            m_Root.Add(m_Body);

            m_Root.style.marginTop = new StyleLength(5);

            m_MessageID = new ErrorMessage(ERR_DUPLICATE_ID)
            {
                style = {marginTop = new StyleLength(10)}
            };

            switch (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                case true:
                    PaintRuntime();
                    break;
                case false:
                    PaintEditor();
                    break;
            }

            return m_Root;
        }

        // Don't use ImGUI
        public sealed override void OnInspectorGUI()
        {
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected void RefreshErrorID()
        {
            serializedObject.Update();
            m_MessageID.style.display = DisplayStyle.None;

            var id = serializedObject.FindProperty(PROP_SAVE_UNIQUE_ID);

            var itemID = id
                .FindPropertyRelative(SaveUniqueIDDrawer.PROP_UNIQUE_ID)
                .FindPropertyRelative(UniqueIDDrawer.SERIALIZED_ID)
                .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                .stringValue;

            var guids = AssetDatabase.FindAssets($"t:{nameof(TGlobalVariables)}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var variables = AssetDatabase.LoadAssetAtPath<TGlobalVariables>(path);

                if (variables.UniqueID.String != itemID || variables == target) continue;

                m_MessageID.style.display = DisplayStyle.Flex;
                return;
            }
        }

        // PAINT EDITOR: --------------------------------------------------------------------------

        protected void PaintEditor()
        {

            var nameList = serializedObject.FindProperty("m_NameList");
            var saveUniqueID = serializedObject.FindProperty(PROP_SAVE_UNIQUE_ID);

            var fieldNameList = new PropertyField(nameList);
            var fieldSaveUniqueID = new PropertyField(saveUniqueID);

            m_Body.Add(fieldNameList);
            m_Body.Add(m_MessageID);
            m_Body.Add(fieldSaveUniqueID);
            
            RefreshErrorID();
            fieldSaveUniqueID.RegisterValueChangeCallback(_ => RefreshErrorID());
            fieldNameList.RegisterValueChangeCallback(_ => OnListChanged?.Invoke());
        }


        // PAINT RUNTIME: -------------------------------------------------------------------------

        protected void PaintRuntime()
        {
            var variables = target as StateMachineAsset;
            if (variables == null) return;

            variables.Unregister(RuntimeOnChange);
            variables.Register(RuntimeOnChange);

            RuntimeOnChange(string.Empty);
        }

        private void RuntimeOnChange(string variableName)
        {
            m_Body.Clear();
            m_Body.styleSheets.Clear();

            var sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (var styleSheet in sheets) m_Body.styleSheets.Add(styleSheet);

            var content = new VisualElement
            {
                name = NAME_LIST
            };

            var variables = target as StateMachineAsset;
            if (variables == null) return;

            var names = variables.Names;
            foreach (var id in names)
            {
                var image = new Image
                {
                    image = variables.Icon(id)
                };

                var title = new Label(variables.Title(id));
                title.style.color = ColorTheme.Get(ColorTheme.Type.TextNormal);

                var element = new VisualElement();
                element.AddToClassList(CLASS_LIST_ELEMENT);

                element.Add(image);
                element.Add(title);

                content.Add(element);
            }

            m_Body.Add(content);
        }
    }
}