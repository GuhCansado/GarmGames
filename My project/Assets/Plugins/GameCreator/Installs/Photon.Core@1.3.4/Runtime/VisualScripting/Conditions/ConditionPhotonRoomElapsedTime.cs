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
    [Title("Photon Room Elapsed Time")]
    [Description("Compare the room elapsed time. Use Start Timer action to use this.")]

    [Category("Photon/Room/Room Elapsed Time")]

    [Keywords("Network", "Photon", "Connection", "Room")]
    
    [Image(typeof(IconTimer), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionPhotonRoomElapsedTime : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private CompareDouble compareTo = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Room Elapsed Time {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return PhotonNetwork.InRoom && compareTo.Match(PhotonNetwork.CurrentRoom.GetElapsedTime(), args);
        }
    }
}