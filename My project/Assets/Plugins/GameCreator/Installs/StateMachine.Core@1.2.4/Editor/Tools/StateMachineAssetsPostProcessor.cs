using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;

namespace NinjutsuGames.StateMachine.Editor
{
    public class StateMachineAssetsPostProcessor : AssetPostprocessor
    {
        public static event Action EventRefresh;
        
        // PROCESSORS: ----------------------------------------------------------------------------

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            SettingsWindow.InitRunners.Add(new InitRunner(
                SettingsWindow.INIT_PRIORITY_LOW,
                CanRefreshVariables,
                RefreshVariables
            ));
        }
        
        private static void OnPostprocessAllAssets(
            string[] importedAssets, 
            string[] deletedAssets, 
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (importedAssets.Length == 0 && deletedAssets.Length == 0) return;
            RefreshVariables();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static bool CanRefreshVariables()
        {
            return true;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public static void RefreshVariables()
        {
            var varSettingsGuids = AssetDatabase.FindAssets($"t:{nameof(StateMachineSettings)}");
            if (varSettingsGuids.Length == 0) return;

            var varSettingsPath = AssetDatabase.GUIDToAssetPath(varSettingsGuids[0]);
            
            var varSettings = AssetDatabase.LoadAssetAtPath<StateMachineSettings>(varSettingsPath);
            if (varSettings == null) return;

            var nameVariablesGuids = AssetDatabase.FindAssets($"t:{nameof(StateMachineAsset)}");
            var nameVariables = new StateMachineAsset[nameVariablesGuids.Length];
            
            for (var i = 0; i < nameVariablesGuids.Length; i++)
            {
                var nameVariablesGuid = nameVariablesGuids[i];
                var nameVariablesPath = AssetDatabase.GUIDToAssetPath(nameVariablesGuid);
                nameVariables[i] = AssetDatabase.LoadAssetAtPath<StateMachineAsset>(nameVariablesPath);
            }
            
            var varSettingsSerializedObject = new SerializedObject(varSettings);
            var globalVariablesProperty = varSettingsSerializedObject
                .FindProperty(TAssetRepositoryEditor.NAMEOF_MEMBER)
                .FindPropertyRelative("m_StateMachineAssets");

            var nameVariablesProperty = globalVariablesProperty.FindPropertyRelative("m_StateMachineAssets");
                
            nameVariablesProperty.arraySize = nameVariables.Length;
            for (var i = 0; i < nameVariables.Length; ++i)
            {
                nameVariablesProperty.GetArrayElementAtIndex(i).objectReferenceValue = nameVariables[i];
            }

            varSettingsSerializedObject.ApplyModifiedPropertiesWithoutUndo();
            EventRefresh?.Invoke();
        }
    }
}