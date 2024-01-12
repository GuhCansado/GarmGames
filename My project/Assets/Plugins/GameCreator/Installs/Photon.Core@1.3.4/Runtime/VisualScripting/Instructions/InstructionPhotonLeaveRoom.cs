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
    [Title("Photon Leave Room")]
    [Description("Leave the current room and return to the Master Server where you can join or create rooms.")]

    [Category("Photon/Room/Leave Room")]
    
    [Image(typeof(IconHome), ColorTheme.Type.Red, typeof(OverlayArrowDown))]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonLeaveRoom : Instruction
    {
        public override string Title => $"Leave Room";

        protected override Task Run(Args args)
        {
            if(PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
            return DefaultResult;
        }
    }
}