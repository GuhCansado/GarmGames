using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Transfer Ownership")]
    [Description("Transfer the ownership of a PhotonView to another player.")]

    [Category("Photon/Core/Photon Transfer Ownership")]
    
    [Parameter("Target", "The target game object that is going to be transferred to.")]
    [Parameter("New Owner", "The player that is going to get the ownership of the target.")]
    
    [Image(typeof(IconShuffle), ColorTheme.Type.Green)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonTransferOwnership : Instruction
    {
        [SerializeField] private PropertyGetGameObject target = GetGameObjectNone.Create();
        [SerializeField] private PropertyGetGameObject newOwner = GetGameObjectPlayer.Create();
        public override string Title => $"Photon Transfer {target}'s Ownership to {newOwner}";

        protected override async Task Run(Args args)
        {
            var p = target.Get(args).GetCachedPhotonView();
            if (p != null)
            {
                var player = newOwner.Get(args).GetPlayerFromView();
                if(p.Owner != player) p.TransferOwnership(player);
            }
            await NextFrame();
        }
    }
}