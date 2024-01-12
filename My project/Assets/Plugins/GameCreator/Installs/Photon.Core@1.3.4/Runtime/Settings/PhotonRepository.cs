using System;
using GameCreator.Runtime.Common;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NinjutsuGames.Photon.Runtime
{
    [Serializable]
    public class PhotonRepository : TRepository<PhotonRepository>
    {
        // REPOSITORY PROPERTIES: -----------------------------------------------------------------
        
        public override string RepositoryID => "photon.general";

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PhotonCoreSettings settings = new();

        // PROPERTIES: ----------------------------------------------------------------------------

        public PhotonCoreSettings Settings => settings;
        
        // EDITOR ENTER PLAYMODE: -----------------------------------------------------------------

#if UNITY_EDITOR
        
        [InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode() => Instance = null;
        
#endif
    }
}