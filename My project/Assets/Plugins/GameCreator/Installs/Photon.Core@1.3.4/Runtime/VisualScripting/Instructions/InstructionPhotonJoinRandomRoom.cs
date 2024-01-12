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
    [Title("Photon Join Random Room")]
    [Description("Joins a random room that matches the filter. Will callback: OnJoinedRoom or OnJoinRandomFailed.")]
    [Category("Photon/Room/Join Random Room")]
    [Parameter("Expected Room Properties", "The expected room's custom properties to set. Only string, boolean, color, vector3 and numbers are allowed.\nThis also defines the custom room properties that get listed in the lobby.")]
    [Parameter("Expected Max Players", "Expected max number of players that can be in the room at any time. 0 means \"no limit\".")]
    [Parameter("Matching Type", "Options for matchmaking rules for OpJoinRandom.")]
    
    [Image(typeof(IconHome), ColorTheme.Type.Green, typeof(OverlayPhysics))]
    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonJoinRandomRoom : Instruction
    {
        [SerializeField] private GlobalNameVariables expectedProperties;
        [SerializeField] private PropertyGetInteger expectedMaxPlayers = GetDecimalInteger.Create(10);
        [SerializeField] private MatchmakingMode matchingType = MatchmakingMode.FillRoom;

        public override string Title => $"Join Random Room";

        protected override Task Run(Args args)
        {
            return Task.FromResult(PhotonNetwork.JoinRandomRoom(expectedProperties.ToHashtable(), (byte) (float) expectedMaxPlayers.Get(args), matchingType, TypedLobby.Default, null));
        }
    }
}