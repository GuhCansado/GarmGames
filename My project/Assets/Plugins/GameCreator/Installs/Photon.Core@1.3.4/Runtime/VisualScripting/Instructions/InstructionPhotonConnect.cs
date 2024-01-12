using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Connect")]
    [Description("Connect to Photon as configured in the PhotonServerSettings file.")]

    [Category("Photon/Connection/Connect")]
    
    [Parameter("GameVersion", "Version number of your game. Setting this updates the AppVersion, which separates your playerbase in matchmaking.")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Green, typeof(OverlayArrowUp))]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonConnect : Instruction
    {
        [SerializeField] private PropertyGetString gameVersion = GetStringString.Create;

        public override string Title => $"Photon Connect Using Settings with version: {gameVersion}";

        protected override Task Run(Args args)
        {
            if(!string.IsNullOrEmpty(gameVersion.Get(args))) PhotonNetwork.GameVersion = gameVersion.Get(args);
            PhotonNetwork.ConnectUsingSettings();
            return DefaultResult;
        }
    }
}