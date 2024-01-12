using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Is Offline Mode")]
    [Description("Offline mode can be set to re-use your multiplayer code in single player game modes.")]

    [Category("Photon/Core/Is Offline Mode")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconSatellite), ColorTheme.Type.Red)]
    
    [Serializable]
    public class ConditionPhotonOfflineMode : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private CompareBoolean compareTo = new(true);

        protected override string Summary => $"Offline Mode {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return compareTo.Match(PhotonNetwork.OfflineMode, args);
        }
    }
}