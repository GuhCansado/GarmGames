using GameCreator.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Common;
using UnityEditor;

namespace NinjutsuGames.StateMachine.Runtime
{
    public class StateMachineSettings : AssetRepository<StateMachineRepository>
    {
        public override IIcon Icon => new IconStateMachine(ColorTheme.Type.TextLight);
        public override string Name => "State Machines";

#if UNITY_EDITOR

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnChangePlayMode;
            EditorApplication.playModeStateChanged += OnChangePlayMode;
            
            RefreshStateMachineAssets();
        }

        private void OnChangePlayMode(PlayModeStateChange playModeStateChange)
        {
            RefreshStateMachineAssets();
        }

        private void RefreshStateMachineAssets()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(StateMachineAsset)}");
            var stateMachineAssets = new StateMachineAsset[guids.Length];
            
            for (var i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                var path = AssetDatabase.GUIDToAssetPath(guid);
                stateMachineAssets[i] = AssetDatabase.LoadAssetAtPath<StateMachineAsset>(path);
            }
            
            Get().StateMachineAssets.Set(stateMachineAssets);
        }

#endif
    }

}