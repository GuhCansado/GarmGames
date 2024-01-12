using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Player Count")]
    [Description("Compare the count of players in this Room.")]

    [Category("Photon/Room/Player Count")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionPhotonPlayerCount : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private CompareInteger compareTo = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Player Count is {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            return compareTo.Match(playerCount, args);
        }
    }
}