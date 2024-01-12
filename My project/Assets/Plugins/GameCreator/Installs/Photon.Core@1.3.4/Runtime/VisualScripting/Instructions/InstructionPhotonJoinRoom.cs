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
    [Title("Photon Join Room")]
    [Description("Joins a room by name. Will callback: OnJoinedRoom or OnJoinRoomFailed.")]

    [Category("Photon/Room/Join Room")]
    
    [Parameter("Room Name", "Unique name of the room to create. Pass null or \"\" to make the server generate a name.")]
    
    [Image(typeof(IconHome), ColorTheme.Type.Green)]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonJoinRoom : Instruction
    {
        [SerializeField] private PropertyGetString roomName = GetStringString.Create;

        public override string Title => $"Join Room: {roomName}";

        protected override Task Run(Args args)
        {
            return Task.FromResult(PhotonNetwork.JoinRoom(string.IsNullOrEmpty(roomName.Get(args)) ? null : roomName.Get(args)));
        }
    }
}