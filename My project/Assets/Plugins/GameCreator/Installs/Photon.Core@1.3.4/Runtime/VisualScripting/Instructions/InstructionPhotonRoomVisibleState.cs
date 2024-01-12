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
    [Title("Photon Change Room Visible State")]
    [Description("Defines if the room is listed in its lobby.")]

    [Category("Photon/Room/Change Room Visible State")]
    
    [Parameter("State", "Rooms can be created invisible, or changed to invisible. Invisible rooms are not listed in the lobby.")]
    
    [Image(typeof(IconVisibleOn), ColorTheme.Type.Purple)]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonRoomVisibleState : Instruction
    {
        public enum RoomVisibleState
        {
            Visible,
            Hidden
        }
        [SerializeField] private RoomVisibleState state = RoomVisibleState.Visible;

        public override string Title => $"Change Room Visible State to {state}";

        protected override Task Run(Args args)
        {
            if(PhotonNetwork.InRoom)
            {
                PhotonNetwork.CurrentRoom.IsVisible = state == RoomVisibleState.Visible;
            }
            return DefaultResult;
        }
    }
}