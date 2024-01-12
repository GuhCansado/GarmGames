using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Join Lobby")]
    [Description("On MasterServer this joins the default lobby which list rooms currently in use.")]

    [Category("Photon/Lobby/Join")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Green, typeof(OverlayArrowUp))]

    [Keywords("Create", "Network", "Photon", "Lobby", "Join")]
    [Serializable]
    public class InstructionPhotonJoinLobby : Instruction
    {
        public override string Title => $"Join Lobby";

        protected override Task Run(Args args)
        {
            return Task.FromResult(PhotonNetwork.JoinLobby());
        }
    }
}