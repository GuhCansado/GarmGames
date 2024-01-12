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
    [Title("Photon Reconnect And Rejoin")]
    [Description("When the client lost connection during gameplay, this method attempts to reconnect and rejoin the room.")]

    [Category("Photon/Room/Reconnect And Rejoin")]
    
    [Image(typeof(IconHome), ColorTheme.Type.Yellow, typeof(OverlayArrowUp))]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonReconnectAndRejoin : Instruction
    {
        public override string Title => $"Reconnect And Rejoin";

        protected override Task Run(Args args)
        {
            return Task.FromResult(PhotonNetwork.ReconnectAndRejoin());
        }
    }
}