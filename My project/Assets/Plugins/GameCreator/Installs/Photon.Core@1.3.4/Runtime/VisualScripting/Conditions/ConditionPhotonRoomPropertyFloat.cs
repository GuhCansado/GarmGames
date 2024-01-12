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
    [Title("Photon Room Property Float")]
    [Description("Compare the room property. Returns false if not connected to a room.")]

    [Category("Photon/Room/Room Property Float")]

    [Keywords("Network", "Photon", "Connection", "Room")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionPhotonRoomPropertyFloat : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private RoomPropertyName property = new();
        [SerializeField] private CompareDouble compareTo = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Room Property ({property.ToString()}) {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            if (!PhotonNetwork.InRoom) return false;
            return compareTo.Match(PhotonNetwork.CurrentRoom.GetFloat(property.ToString()), args);
        }
    }
}