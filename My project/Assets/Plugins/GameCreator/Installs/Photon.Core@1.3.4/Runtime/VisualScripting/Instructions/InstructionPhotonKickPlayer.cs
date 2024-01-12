using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Kick Player")]
    [Description("Request a client to disconnect/kick, which happens if EnableCloseConnection is set to true. Only the master client can do this.")]

    [Category("Photon/Player/Kick Player")]
    
    [Parameter("Player", "The target player to kick/close connection.")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Red, typeof(OverlayMinus))]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonKickPlayer : Instruction
    {
        [SerializeField] private PropertyGetGameObject player = GetGameObjectNone.Create();
        public override string Title => $"Kick Player {player}";

        protected override Task Run(Args args)
        {
            var result = false;
            var p = player.Get(args).GetPlayerFromView();
            if (p != null)
            {
                result = PhotonNetwork.CloseConnection(p);
            }
            return Task.FromResult(result);
        }
    }
}