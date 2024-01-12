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
    [Title("Photon Room Property Boolean")]
    [Description("Changes a room property.")]

    [Category("Photon/Room/Change Room Property Boolean")]
    
    [Parameter("Value", "How much is the property going to change.")]
    
    [Image(typeof(IconToggleOn), ColorTheme.Type.Blue)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonRoomPropertyBoolean : Instruction
    {
        [SerializeField] private PlayerPropertyName property = new();
        [SerializeField] private PropertyGetBool value = new(true);

        public override string Title => $"Change Room bool {property.ToString()} to {value}";

        protected override Task Run(Args args)
        {
            if (!PhotonNetwork.InRoom) return DefaultResult;
            var p = PhotonNetwork.CurrentRoom;
            var a = value.Get(args);
            p.SetBool(property.ToString(), a, false);

            return DefaultResult;
        }
    }
}