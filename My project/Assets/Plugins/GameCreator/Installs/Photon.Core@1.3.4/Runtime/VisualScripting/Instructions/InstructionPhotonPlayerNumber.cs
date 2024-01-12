using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Set Player Number")]
    [Description("Set to synchronize the player's nickname with everyone in the room(s) you enter. This sets PhotonNetwork.player.NickName.")]

    [Category("Photon/Player/Set Player Number")]
    
    [Parameter("Player", "The target gameObject that should have a PhotonView to get the Photon Player.")]
    [Parameter("Number", "The number of the player.")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.Teal)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonPlayerNumber : Instruction
    {
        [SerializeField] private PropertyGetGameObject player = GetGameObjectLocalPlayer.Create();
        [SerializeField] private PropertyGetInteger number = new(0);

        public override string Title => $"Set {player} Number to {number}";

        protected override Task Run(Args args)
        {
            var p = player.Get(args).GetPlayerFromView();
            p?.SetPlayerNumber((int)number.Get(args));
            return DefaultResult;
        }
    }
}