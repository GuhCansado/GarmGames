using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Room Is Visible")]
    [Description("Compare if the current room is visible.")]

    [Category("Photon/Room/Room Is Visible")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconVisibleOn), ColorTheme.Type.Purple)]

    
    [Serializable]
    public class ConditionPhotonRoomIsVisible : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private CompareBoolean compareTo = new(true);

        protected override string Summary => $"Is Room Visible {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            if (PhotonNetwork.InRoom) return false;
            return compareTo.Match(PhotonNetwork.CurrentRoom.IsVisible, args);
        }
    }
}