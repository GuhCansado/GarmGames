using System;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Join Random or Create Room")]
    [Description("Attempts to join a room that matches the specified filter and creates a room if none found.")]
    [Category("Photon/Room/Join Random or Create Room")]
    // [Parameter("Expected Room Properties", "Unique name of the room to create. Pass null or \"\" to make the server generate a name.")]
    // [Parameter("Expected Max Players", "Unique name of the room to create. Pass null or \"\" to make the server generate a name.")]
    [Parameter("Matching Type", "Options for matchmaking rules for OpJoinRandom.")]
    [Parameter("Room Name", "Unique name of the room to create. Pass null or \"\" to make the server generate a name.")]
    [Parameter("Max Players", "Max number of players that can be in the room at any time. 0 means \"no limit\".")]
    [Parameter("Is Open", "Defines if this room can be joined at all.")]
    [Parameter("Is Visible", "Defines if this room is listed in the lobby. If not, it also is not joined randomly.")]
    [Parameter("Player TTL", "Time To Live (TTL) for an 'actor' in a room. If a client disconnects, this actor is inactive first and removed after this timeout. In milliseconds.")]
    [Parameter("Empty Room TTL", "Time To Live (TTL) for a room when the last player leaves. Keeps room in memory for case a player re-joins soon. In milliseconds.")]
    [Parameter("Room Properties", "The room's custom properties to set. Only string, boolean, color, vector3 and numbers are allowed.\nThis also defines the custom room properties that get listed in the lobby.")]
    [Image(typeof(IconHome), ColorTheme.Type.Green, typeof(OverlayTick))]
    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonJoinRandomOrCreateRoom : Instruction
    {
        [Header("Random Properties")] [SerializeField][Space]
        private GlobalNameVariables expectedProperties;

        [SerializeField] private PropertyGetInteger expectedMaxPlayers = GetDecimalInteger.Create(10);
        [SerializeField] private MatchmakingMode matchingType = MatchmakingMode.FillRoom;

        [Header("Create Properties")] [SerializeField][Space]
        private PropertyGetString roomName = GetStringString.Create;

        [SerializeField] private PropertyGetInteger maxPlayers = GetDecimalInteger.Create(10);
        [SerializeField] private PropertyGetBool isOpen = GetBoolValue.Create(true);
        [SerializeField] private PropertyGetBool isVisible = GetBoolValue.Create(true);
        [SerializeField] private PropertyGetInteger playerTtl = GetDecimalInteger.Create(0);
        [SerializeField] private PropertyGetInteger emptyRoomTtl = GetDecimalInteger.Create(0);
        [Space]
        [SerializeField] private GlobalNameVariables roomProperties;

        public override string Title => $"Join Random or Create Room: {roomName}";

        protected override Task Run(Args args)
        {
            RoomOptions roomOptions = new RoomOptions { };
            roomOptions.PublishUserId = true;
            roomOptions.IsOpen = isOpen.Get(args);
            roomOptions.IsVisible = isVisible.Get(args);
            roomOptions.MaxPlayers = (byte) (float) maxPlayers.Get(args);
            roomOptions.PlayerTtl = (int) playerTtl.Get(args);
            roomOptions.EmptyRoomTtl = (int) emptyRoomTtl.Get(args);
            if (roomProperties)
            {
                roomOptions.CustomRoomProperties = roomProperties.ToHashtable();
                roomOptions.CustomRoomPropertiesForLobby = roomProperties.ToAllowedKeys();
            }

            return Task.FromResult(PhotonNetwork.JoinRandomOrCreateRoom(expectedProperties ? expectedProperties.ToHashtable() : new Hashtable(), (byte) (float) expectedMaxPlayers.Get(args), matchingType,
                TypedLobby.Default, null, roomName.Get(args), roomOptions));
        }
    }
}