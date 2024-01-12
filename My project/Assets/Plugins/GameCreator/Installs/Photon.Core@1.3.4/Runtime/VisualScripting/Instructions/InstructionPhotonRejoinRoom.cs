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
    [Title("Photon Rejoin Room")]
    [Description("Rejoins a room by roomName (using the userID internally to return).  Will callback: OnJoinedRoom or OnJoinRoomFailed.")]

    [Category("Photon/Room/Rejoin Room")]
    
    [Parameter("Room Name", "Unique name of the room to create. Pass null or \"\" to make the server generate a name.")]
    
    [Image(typeof(IconHome), ColorTheme.Type.Yellow, typeof(OverlayTick))]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonRejoinRoom : Instruction
    {
        [SerializeField] private PropertyGetString roomName = GetStringString.Create;

        public override string Title => $"Rejoin Room: {roomName}";

        protected override Task Run(Args args)
        {
            return Task.FromResult(PhotonNetwork.RejoinRoom(string.IsNullOrEmpty(roomName.Get(args)) ? null : roomName.Get(args)));
        }
    }
}