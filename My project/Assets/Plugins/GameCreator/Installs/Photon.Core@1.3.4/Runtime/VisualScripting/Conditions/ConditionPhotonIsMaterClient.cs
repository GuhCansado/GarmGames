using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Is Master Client")]
    [Description("Are we the master client?")]

    [Category("Photon/Core/Is Master Client")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Blue)]

    [Serializable]
    public class ConditionPhotonIsMasterClient : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private CompareBoolean compareTo = new(true);

        protected override string Summary => $"Is MasterClient {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return compareTo.Match(PhotonNetwork.IsMasterClient, args);
        }
    }
}