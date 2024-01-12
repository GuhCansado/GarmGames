using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Is Connected")]
    [Description("False until you connected to Photon initially. True immediately after Connect-call, in offline mode, while connected to any server and even while switching servers.")]

    [Category("Photon/Connection/Is Connected")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Green, typeof(OverlayDot))]
    
    [Serializable]
    public class ConditionPhotonIsConnected : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private CompareBoolean compareTo = new(true);

        protected override string Summary => $"Is Connected {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return compareTo.Match(PhotonNetwork.IsConnected, args);
        }
    }
}