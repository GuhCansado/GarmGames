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
    [Title("Photon Change Player Score")]
    [Description("Changes a player score.")]

    [Category("Photon/Player/Change Player Score")]
    
    [Parameter("Player", "The target gameObject that should have a PhotonView to get the Photon Player.")]
    [Parameter("Amount", "How much is the score going to change.")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.Purple, typeof(OverlayPlus))]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonPlayerScore : Instruction
    {
        [SerializeField] private PropertyGetGameObject player = GetGameObjectLocalPlayer.Create();
        [SerializeField] private Operation operation = Operation.Add;
        [SerializeField] private PropertyGetInteger amount = new(0);

        public override string Title => $"{operation} {player} Score to {amount}";

        protected override Task Run(Args args)
        {
            var p = player.Get(args).GetPlayerFromView();
            var a = (int)amount.Get(args);
            switch (operation)
            {
                case Operation.Add:
                    p.AddScore(a);
                    break;
                case Operation.Set:
                    p.SetScore(a);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Undefined operation to change the score.");
            }
            return DefaultResult;
        }
    }
}