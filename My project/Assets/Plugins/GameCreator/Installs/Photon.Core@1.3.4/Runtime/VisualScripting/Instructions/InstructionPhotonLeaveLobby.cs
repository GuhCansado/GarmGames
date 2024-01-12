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
    [Title("Photon Leave Lobby")]
    [Description("Leave a lobby to stop getting updates about available rooms.")]

    [Category("Photon/Lobby/Leave")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Red, typeof(OverlayArrowDown))]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonLeaveLobby : Instruction
    {
        public override string Title => $"Leave Lobby";

        protected override Task Run(Args args)
        {
            return Task.FromResult(PhotonNetwork.LeaveLobby());
        }
    }
}