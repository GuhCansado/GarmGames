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
    [Title("Photon Change Room Open State")]
    [Description("Defines if the room can be joined.")]

    [Category("Photon/Room/Change Room Open State")]
    
    [Parameter("State", "This does not affect listing in a lobby but joining the room will fail if not open. \nIf not open, the room is excluded from random matchmaking.")]
    
    [Image(typeof(IconCheckmark), ColorTheme.Type.Purple)]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonRoomOpenState : Instruction
    {
        public enum RoomOpenState
        {
            Open,
            Close
        }
        [SerializeField] private RoomOpenState state = RoomOpenState.Close;

        public override string Title => $"Change Room Open State to {state}";

        protected override Task Run(Args args)
        {
            if(PhotonNetwork.InRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = state == RoomOpenState.Open;
            }
            return DefaultResult;
        }
    }
}