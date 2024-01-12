using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon In Room")]
    [Description("Is true while being in a room (NetworkClientState == ClientState.Joined).")]

    [Category("Photon/Room/In Room")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconHome), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionPhotonInRoom : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField] private CompareBoolean compareTo = new(true);

        protected override string Summary => $"In Room {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return compareTo.Match(PhotonNetwork.InRoom, args);
        }
    }
}