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
    [Title("Photon Room Property String")]
    [Description("Changes a room property.")]

    [Category("Photon/Room/Change Room Property String")]
    
    [Parameter("Value", "How much is the property going to change.")]
    
    [Image(typeof(IconString), ColorTheme.Type.Blue)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonRoomPropertyString : Instruction
    {
        [SerializeField] private PlayerPropertyName property = new();
        [SerializeField] private PropertyGetString value = new();

        public override string Title => $"Change Room string {property.ToString()} to {value}";

        protected override Task Run(Args args)
        {
            if (!PhotonNetwork.InRoom) return DefaultResult;
            var p = PhotonNetwork.CurrentRoom;
            var a = value.Get(args);
            p.SetString(property.ToString(), a, false);

            return DefaultResult;
        }
    }
}