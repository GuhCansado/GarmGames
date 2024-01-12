using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Request Ownership")]
    [Description("Request the ownership of a PhotonView.")]

    [Category("Photon/Core/Photon Request Ownership")]
    
    [Parameter("Target", "The target game object that is going to be transferred to.")]
    
    [Image(typeof(IconShuffle), ColorTheme.Type.Green)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonRequestOwnership : Instruction
    {
        [SerializeField] private PropertyGetGameObject target = GetGameObjectNone.Create();
        public override string Title => $"Photon Request {target}'s Ownership";

        protected override Task Run(Args args)
        {
            var p = target.Get(args).GetCachedPhotonView();
            if (p != null)
            {
                p.RequestOwnership();
            }
            return DefaultResult;
        }
    }
}