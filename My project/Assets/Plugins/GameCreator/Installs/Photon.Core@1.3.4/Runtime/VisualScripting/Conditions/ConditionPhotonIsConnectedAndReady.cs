using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Is Connected And Ready")]
    [Description("A refined version of connected which is true only if your connection to the server is ready to accept operations like join, leave, etc.")]

    [Category("Photon/Connection/Is Connected And Ready")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionPhotonIsConnectedAndReady : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private CompareBoolean compareTo = new(true);

        protected override string Summary => $"Is Connected and Ready {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return compareTo.Match(PhotonNetwork.IsConnectedAndReady, args);
        }
    }
}