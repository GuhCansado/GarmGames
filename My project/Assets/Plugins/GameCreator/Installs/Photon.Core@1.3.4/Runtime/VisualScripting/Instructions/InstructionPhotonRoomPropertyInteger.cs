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
    [Title("Photon Room Property Integer")]
    [Description("Changes a room property.")]

    [Category("Photon/Room/Change Room Property Integer")]
    
    [Parameter("Value", "How much is the property going to change.")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.White)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonRoomPropertyInteger : Instruction
    {
        [SerializeField] private PlayerPropertyName property = new();
        [SerializeField] private Operation operation = Operation.Add;
        [SerializeField] private PropertyGetInteger value = new(0);

        public override string Title => $"{operation} Room integer {property.ToString()} to {value}";

        protected override Task Run(Args args)
        {
            if (!PhotonNetwork.InRoom) return DefaultResult;
            var p = PhotonNetwork.CurrentRoom;
            var a = (int)value.Get(args);
            switch (operation)
            {
                case Operation.Add:
                    p.AddInt(property.ToString(), a, false);
                    break;
                case Operation.Set:
                    p.SetInt(property.ToString(), a, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Undefined operation to change room property: {property}.");
            }
            return DefaultResult;
        }
    }
}