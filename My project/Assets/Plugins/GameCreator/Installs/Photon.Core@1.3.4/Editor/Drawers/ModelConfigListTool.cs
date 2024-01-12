using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Editor.VisualScripting;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor
{
    public class ModelConfigListTool : TPolymorphicListTool
    {
        private const string NAME_BUTTON_ADD = "GC-Instruction-List-Foot-Add";
        
        private static readonly IIcon ICON_ADD = new IconCharacter(ColorTheme.Type.TextLight);
        
        private const string MODEL_PATH = RuntimePaths.CHARACTERS + "Assets/3D/Mannequin.fbx";
        private const string SKELETON_PATH = RuntimePaths.CHARACTERS + "Assets/3D/Skeleton.asset";
        private const string FOOTSTEPS_PATH = RuntimePaths.CHARACTERS + "Assets/3D/Footsteps.asset";
        
        public static GameObject DefaultPrefab => AssetDatabase.LoadAssetAtPath<GameObject>(MODEL_PATH);
        public static Skeleton DefaultSkeleton => AssetDatabase.LoadAssetAtPath<Skeleton>(SKELETON_PATH);
        public static MaterialSoundsAsset DefaultFootsteps => AssetDatabase.LoadAssetAtPath<MaterialSoundsAsset>(FOOTSTEPS_PATH);

        // MEMBERS: -------------------------------------------------------------------------------

        protected Button m_ButtonAdd;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string ElementNameHead => "GC-Instruction-List-Head";
        protected override string ElementNameBody => "GC-Instruction-List-Body";
        protected override string ElementNameFoot => "GC-Instruction-List-Foot";

        protected override List<string> CustomStyleSheetPaths => new()
        {
            EditorPaths.VISUAL_SCRIPTING + "Instructions/StyleSheets/Instructions-List"
        };

        public override bool AllowReordering => true;
        public override bool AllowDuplicating => true;
        public override bool AllowDeleting  => true;
        public override bool AllowContextMenu => true;
        public override bool AllowCopyPaste => true;
        public override bool AllowInsertion => true;
        public override bool AllowBreakpoint => true;
        public override bool AllowDisable => true;
        public override bool AllowDocumentation => true;
        
        private BaseActions Actions => SerializedObject?.targetObject as BaseActions;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ModelConfigListTool(SerializedProperty property)
            : base(property, "m_Models")
        {
            SerializedObject.Update();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override VisualElement MakeItemTool(int index)
        {
            return new ModelConfigItemTool(this, index);
        }

        protected override void SetupHead()
        { }

        protected override void SetupFoot()
        {
            base.SetupFoot();
            
            m_ButtonAdd = new Button { name = NAME_BUTTON_ADD };

            m_ButtonAdd.Add(new Image { image = ICON_ADD.Texture });
            m_ButtonAdd.Add(new Label { text = "Add Model..." });

            m_ButtonAdd.clicked += () =>
            {
                SerializedObject.Update();
            
                int insertIndex = PropertyList.arraySize;
                PropertyList.InsertArrayElementAtIndex(insertIndex);
                PropertyList
                    .GetArrayElementAtIndex(insertIndex)
                    .SetValue(GetNewModelConfig(insertIndex));

                SerializationUtils.ApplyUnregisteredSerialization(SerializedObject);

                int size = PropertyList.arraySize;
                ExecuteEventChangeSize(size);
            
                using ChangeEvent<int> changeEvent = ChangeEvent<int>.GetPooled(size, size);
                changeEvent.target = this;
                SendEvent(changeEvent);
            
                Refresh();
            };

            m_Foot.Add(m_ButtonAdd);
        }

        private ModelConfig GetNewModelConfig(int index)
        {
            var modelConfig = new ModelConfig
            {
                // name = $"model-{index}",
                prefab = GetGameObjectInstance.Create(DefaultPrefab),
                skeleton = DefaultSkeleton,
                materialSounds = DefaultFootsteps
            };
            return modelConfig;
        }
    }
}