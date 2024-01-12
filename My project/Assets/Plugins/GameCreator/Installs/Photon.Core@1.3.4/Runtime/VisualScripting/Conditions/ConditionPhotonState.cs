using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Connection State")]
    [Description("False until you connected to Photon initially. True immediately after Connect-call, in offline mode, while connected to any server and even while switching servers.")]

    [Category("Photon/Connection/State")]

    [Keywords("Network", "Photon", "Connection", "State")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Green, typeof(OverlayTick))]
    
    [Serializable]
    public class ConditionPhotonState : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private CompareClientState compareTo = new(ClientState.Disconnected);

        protected override string Summary => $"Connection State {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return compareTo.Match(PhotonNetwork.NetworkClientState);
        }
    }
}