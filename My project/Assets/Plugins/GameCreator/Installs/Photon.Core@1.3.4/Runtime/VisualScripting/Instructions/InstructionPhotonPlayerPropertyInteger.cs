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
    [Title("Photon Player Property Integer")]
    [Description("Changes the target player property.")]

    [Category("Photon/Player/Change Property Integer")]
    
    [Parameter("Player", "The target gameObject that should have a PhotonView to get the Photon Player.")]
    [Parameter("Value", "How much is the property going to change.")]
    [Parameter("Only if Mine", "If true only will set the property if the target is mine.")]

    [Image(typeof(IconNumber), ColorTheme.Type.White)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonPlayerPropertyInteger : Instruction
    {
        [SerializeField] private PlayerPropertyName property = new();
        [SerializeField] private PropertyGetGameObject player = GetGameObjectLocalPlayer.Create();
        [SerializeField] private Operation operation = Operation.Add;
        [SerializeField] private PropertyGetInteger value = new(0);
        [SerializeField] private bool onlyIfMine = true;

        public override string Title => $"{operation} {player} Property {property.ToString()} to {value}";

        protected override async Task Run(Args args)
        {
            var p = player.Get(args).GetPlayerFromView();
            if (onlyIfMine && !p.IsLocal) return;

            var a = (int)value.Get(args);
            switch (operation)
            {
                case Operation.Add:
                    p.AddInt(property.ToString(), a);
                    break;
                case Operation.Set:
                    p.SetInt(property.ToString(), a);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Undefined operation to change player property: {property}.");
            }
            await NextFrame();
        }
    }
}