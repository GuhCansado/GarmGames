using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Room Is Offline")]
    [Description("Compare if the current room is offline.")]

    [Category("Photon/Room/Room Is Offline")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconHome), ColorTheme.Type.Red)]

    
    [Serializable]
    public class ConditionPhotonRoomIsOffline : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private CompareBoolean compareTo = new(true);

        protected override string Summary => $"Is Room Offline {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            if (PhotonNetwork.InRoom) return false;
            return compareTo.Match(PhotonNetwork.CurrentRoom.IsOffline, args);
        }
    }
}