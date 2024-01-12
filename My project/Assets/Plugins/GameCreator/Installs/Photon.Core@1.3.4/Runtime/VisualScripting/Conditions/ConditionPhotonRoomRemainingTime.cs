using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Room Remaining Time")]
    [Description("Compare the room remaining time. Use Start Countdown Timer action to use this.")]

    [Category("Photon/Room/Room Remaining Time")]

    [Keywords("Network", "Photon", "Connection", "Room")]
    
    [Image(typeof(IconTimer), ColorTheme.Type.Green, typeof(OverlayMinus))]
    
    [Serializable]
    public class ConditionPhotonRoomRemainingTime : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private CompareDouble compareTo = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Room Remaining Time {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return PhotonNetwork.InRoom && compareTo.Match(PhotonNetwork.CurrentRoom.GetRemainingTime(), args);
        }
    }
}