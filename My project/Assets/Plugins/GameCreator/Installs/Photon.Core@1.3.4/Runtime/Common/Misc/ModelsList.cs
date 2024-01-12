using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime
{
    [Serializable]
    public class ModelsList
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeReference] private ModelConfig[] m_Models = Array.Empty<ModelConfig>();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public ModelConfig[] Models => m_Models;
    }
}