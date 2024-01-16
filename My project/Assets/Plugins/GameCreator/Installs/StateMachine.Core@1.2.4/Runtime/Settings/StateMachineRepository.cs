using System;
using GameCreator.Runtime.Common;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NinjutsuGames.StateMachine.Runtime
{
    [Serializable]
    public class StateMachineRepository : TRepository<StateMachineRepository>
    {
        // REPOSITORY PROPERTIES: -----------------------------------------------------------------
        
        public override string RepositoryID => "statemachine.assets";

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private StateMachineAssets m_StateMachineAssets = new();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public StateMachineAssets StateMachineAssets => m_StateMachineAssets;
        
        // EDITOR ENTER PLAYMODE: -----------------------------------------------------------------

#if UNITY_EDITOR
        
        [InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode() => Instance = null;
        
#endif
    }

}