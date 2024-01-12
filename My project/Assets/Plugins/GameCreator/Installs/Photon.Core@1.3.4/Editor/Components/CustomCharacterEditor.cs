using GameCreator.Editor.Characters;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Components
{
    // [CustomEditor(typeof(Character), true, isFallback = false)]
    public class CustomCharacterEditor : CharacterEditor
    {
        private const string NAME_GROUP_GENERAL   = "GC-Character-GroupGeneral";
        private const string NAME_GROUP_GENERAL_L = "GC-Character-GroupGeneral-L";
        private const string NAME_GROUP_GENERAL_R = "GC-Character-GroupGeneral-R";
        
        private const string PATH_USS = EditorPaths.CHARACTERS + "StyleSheets/Character";
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            var propertyIsPlayer = this.serializedObject.FindProperty("m_IsPlayer");
            var propertyTime = this.serializedObject.FindProperty("m_Time");
            var propertyBusy = this.serializedObject.FindProperty("m_Busy");

            var groupGeneral  = new VisualElement { name = NAME_GROUP_GENERAL   };
            var groupGeneralL = new VisualElement { name = NAME_GROUP_GENERAL_L };
            var groupGeneralR = new VisualElement { name = NAME_GROUP_GENERAL_R };
            
            root.Add(groupGeneral);
            groupGeneral.Add(groupGeneralL);
            groupGeneral.Add(groupGeneralR);
            
            groupGeneralL.Add(new PropertyField(propertyBusy));
            groupGeneralR.Add(new PropertyField(propertyIsPlayer));
            groupGeneralR.Add(new PropertyField(propertyIsPlayer));
            groupGeneralR.Add(new PropertyField(propertyTime));
            
            var propertyKernel = this.serializedObject.FindProperty("m_Kernel");
            var propertyIK = this.serializedObject.FindProperty("m_InverseKinematics");
            var propertyFootsteps = this.serializedObject.FindProperty("m_Footsteps");
            var propertyRagdoll = this.serializedObject.FindProperty("m_Ragdoll");
            var propertyUniqueID = this.serializedObject.FindProperty("m_UniqueID");
            
            var fieldKernel = new PropertyField(propertyKernel);
            var fieldIK = new PropertyField(propertyIK);
            var fieldFootsteps = new PropertyField(propertyFootsteps);
            var fieldRagdoll = new PropertyField(propertyRagdoll);
            var fieldUniqueID = new PropertyField(propertyUniqueID);
            
            // root.Add(fieldKernel);
            // root.Add(fieldIK);
            // root.Add(fieldFootsteps);
            // root.Add(fieldRagdoll);
            // root.Add(fieldUniqueID);
            Debug.Log($"Custom Character");
            
            var styleSheets = StyleSheetUtils.Load(PATH_USS);
            foreach (var styleSheet in styleSheets) root.styleSheets.Add(styleSheet);

            return root;
        }
    }
}