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
    [Title("Photon Player Property Float")]
    [Description("Changes the target player property.")]

    [Category("Photon/Player/Change Property Float")]
    
    [Parameter("Player", "The target gameObject that should have a PhotonView to get the Photon Player.")]
    [Parameter("Value", "How much is the property going to change.")]
    [Parameter("Only if Mine", "If true only will set the property if the target is mine.")]

    [Image(typeof(IconNumber), ColorTheme.Type.Green)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonPlayerPropertyFloat : Instruction
    {
        [SerializeField] private PlayerPropertyName property = new();
        [SerializeField] private PropertyGetGameObject player = GetGameObjectLocalPlayer.Create();
        [SerializeField] private Operation operation = Operation.Add;
        [SerializeField] private PropertyGetDecimal value = new(0);
        [SerializeField] private bool onlyIfMine = true;

        public override string Title => $"{operation} {player} Property {property.ToString()} to {value}";

        protected override async Task Run(Args args)
        {
            var p = player.Get(args).GetPlayerFromView();
            if (onlyIfMine && !p.IsLocal) return;

            var a = (float)value.Get(args);
            switch (operation)
            {
                case Operation.Add:
                    p.AddFloat(property.ToString(), a);
                    break;
                case Operation.Set:
                    p.SetFloat(property.ToString(), a);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Undefined operation to change player property: {property}.");
            }

            await NextFrame();
        }
    }
}