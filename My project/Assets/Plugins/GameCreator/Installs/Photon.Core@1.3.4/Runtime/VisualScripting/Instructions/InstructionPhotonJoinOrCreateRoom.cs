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
    [Title("Photon Join or Create Room")]
    [Description("Joins a specific room by name and creates it on demand. Will callback: OnJoinedRoom or OnJoinRoomFailed.")]

    [Category("Photon/Room/Join or Create Room")]
    
    [Parameter("Room Name", "Unique name of the room to create. Pass null or \"\" to make the server generate a name.")]
    [Parameter("Max Players", "Max number of players that can be in the room at any time. 0 means \"no limit\".")]
    [Parameter("Is Open", "Defines if this room can be joined at all.")]
    [Parameter("Is Visible", "Defines if this room is listed in the lobby. If not, it also is not joined randomly.")]
    [Parameter("Player TTL", "Time To Live (TTL) for an 'actor' in a room. If a client disconnects, this actor is inactive first and removed after this timeout. In milliseconds.")]
    [Parameter("Empty Room TTL", "Time To Live (TTL) for a room when the last player leaves. Keeps room in memory for case a player re-joins soon. In milliseconds.")]
    [Parameter("Room Properties", "The room's custom properties to set. Only string, boolean, color, vector3 and numbers are allowed.\nThis also defines the custom room properties that get listed in the lobby.")]
    
    [Image(typeof(IconHome), ColorTheme.Type.Green, typeof(OverlayPlus))]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonJoinOrCreateRoom : Instruction
    {
        [SerializeField] private PropertyGetString roomName = GetStringString.Create;
        [SerializeField] private PropertyGetInteger maxPlayers = GetDecimalInteger.Create(10);
        [SerializeField] private PropertyGetBool isOpen = GetBoolValue.Create(true);
        [SerializeField] private PropertyGetBool isVisible = GetBoolValue.Create(true);
        [SerializeField] private PropertyGetInteger playerTtl = GetDecimalInteger.Create(0);
        [SerializeField] private PropertyGetInteger emptyRoomTtl = GetDecimalInteger.Create(0);
        [SerializeField] private GlobalNameVariables roomProperties;

        public override string Title => $"Join or Create Room: {roomName}";

        protected override Task Run(Args args)
        {
            RoomOptions roomOptions = new RoomOptions { };
            roomOptions.PublishUserId = true;
            roomOptions.IsOpen = isOpen.Get(args);
            roomOptions.IsVisible = isVisible.Get(args);
            roomOptions.MaxPlayers = (byte)(float)maxPlayers.Get(args);
            roomOptions.PlayerTtl = (int)playerTtl.Get(args);
            roomOptions.EmptyRoomTtl = (int)emptyRoomTtl.Get(args);
            if(roomProperties)
            {
                roomOptions.CustomRoomProperties = roomProperties.ToHashtable();
                roomOptions.CustomRoomPropertiesForLobby = roomProperties.ToAllowedKeys();
            }

            return Task.FromResult(PhotonNetwork.JoinOrCreateRoom(string.IsNullOrEmpty(roomName.Get(args)) ? null : roomName.Get(args), roomOptions, TypedLobby.Default));
        }
    }
}