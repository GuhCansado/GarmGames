using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Connect to Best Cloud Server")]
    [Description("Connects to Photon using the specified method.")]

    [Category("Photon/Connection/Connect to Best Cloud Server")]
    
    [Parameter("GameVersion", "Version number of your game. Setting this updates the AppVersion, which separates your playerbase in matchmaking.")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Green, typeof(OverlayArrowUp))]

    [Keywords("Connect", "Network", "Photon", "Cloud", "Best Cloud")]
    [Serializable]
    public class InstructionPhotonConnectBestCloud : Instruction
    {
        [SerializeField] private PropertyGetString gameVersion = GetStringString.Create;

        public override string Title => $"Photon Connect to Best Cloud Server with version: {gameVersion}";

        protected override Task Run(Args args)
        {
            if(!string.IsNullOrEmpty(gameVersion.Get(args))) PhotonNetwork.GameVersion = gameVersion.Get(args);
            return Task.FromResult(PhotonNetwork.ConnectToBestCloudServer());
        }
    }
}