using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Room Is Open")]
    [Description("Compare if the current room is open.")]

    [Category("Photon/Room/Room Is Open")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconCheckmark), ColorTheme.Type.Purple)]

    
    [Serializable]
    public class ConditionPhotonRoomIsOpen : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private CompareBoolean compareTo = new(true);

        protected override string Summary => $"Is Room Open {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            if (PhotonNetwork.InRoom) return false;
            return compareTo.Match(PhotonNetwork.CurrentRoom.IsOpen, args);
        }
    }
}