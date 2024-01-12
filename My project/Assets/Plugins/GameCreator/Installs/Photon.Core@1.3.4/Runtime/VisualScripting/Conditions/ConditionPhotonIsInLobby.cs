using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Is In Lobby")]
    [Description("True while this client is in a lobby.")]

    [Category("Photon/Connection/Is In Lobby")]

    [Keywords("Network", "Photon", "Lobby")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Purple)]

    [Serializable]
    public class ConditionPhotonInLobby : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private CompareBoolean compareTo = new(true);

        protected override string Summary => $"Is In Lobby {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return compareTo.Match(PhotonNetwork.InLobby, args);
        }
    }
}