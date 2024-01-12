using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Photon Set Player Name")]
    [Description("Set to synchronize the player's nickname with everyone in the room(s) you enter. This sets PhotonNetwork.player.NickName.")]

    [Category("Photon/Player/Set Player NickName")]
    
    [Parameter("Player Name", "The name of the player.")]
    
    [Image(typeof(IconString), ColorTheme.Type.Purple)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonPlayerName : Instruction
    {
        [SerializeField] private PropertyGetString playerName = GetStringString.Create;

        public override string Title => $"Set Player NickName to {playerName}";

        protected override Task Run(Args args)
        {
            if(!string.IsNullOrEmpty(playerName.Get(args))) PhotonNetwork.NickName = playerName.Get(args);
            return DefaultResult;
        }
    }
}