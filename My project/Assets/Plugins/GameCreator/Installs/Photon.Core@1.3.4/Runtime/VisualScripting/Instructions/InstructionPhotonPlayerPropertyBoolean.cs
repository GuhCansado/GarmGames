using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Player Property Boolean")]
    [Description("Changes the target player property.")]

    [Category("Photon/Player/Change Property Boolean")]
    
    [Parameter("Player", "The target gameObject that should have a PhotonView to get the Photon Player.")]
    [Parameter("Value", "How much is the property going to change.")]
    [Parameter("Only if Mine", "If true only will set the property if the target is mine.")]
    
    [Image(typeof(IconToggleOn), ColorTheme.Type.Green)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonPlayerPropertyBoolean : Instruction
    {
        [SerializeField] private PlayerPropertyName property = new();
        [SerializeField] private PropertyGetGameObject player = GetGameObjectLocalPlayer.Create();
        [SerializeField] private PropertyGetBool value = new(true);
        [SerializeField] private bool onlyIfMine = true;

        public override string Title => $"Change {player} bool {property.ToString()} to {value}";

        protected override async Task Run(Args args)
        {
            var p = player.Get(args).GetPlayerFromView();
            if (onlyIfMine && !p.IsLocal) return;

            var a = value.Get(args);
            p.SetBool(property.ToString(), a);
            await NextFrame();
        }
    }
}