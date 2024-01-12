using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Room Property Bool")]
    [Description("Compare the room property. Returns false if not connected to a room.")]

    [Category("Photon/Room/Room Property Bool")]

    [Keywords("Network", "Photon", "Connection", "Room")]
    
    [Image(typeof(IconToggleOn), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionPhotonRoomPropertyBool : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private RoomPropertyName property = new();
        [SerializeField] private CompareBoolean compareTo = new(true);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Room Property ({property.ToString()}) {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            if (!PhotonNetwork.InRoom) return false;
            return compareTo.Match(PhotonNetwork.CurrentRoom.GetBool(property.ToString()), args);
        }
    }
}