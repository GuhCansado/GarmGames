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
    [Title("Photon Room Property Float")]
    [Description("Changes a room property.")]

    [Category("Photon/Room/Change Room Property Float")]
    
    [Parameter("Value", "How much is the property going to change.")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.Blue)]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonRoomPropertyFloat : Instruction
    {
        [SerializeField] private PlayerPropertyName property = new();
        [SerializeField] private Operation operation = Operation.Add;
        [SerializeField] private PropertyGetDecimal value = new(0);

        public override string Title => $"{operation} Room float {property.ToString()} to {value}";

        protected override Task Run(Args args)
        {
            if (!PhotonNetwork.InRoom) return DefaultResult;
            var p = PhotonNetwork.CurrentRoom;
            var a = (float)value.Get(args);
            switch (operation)
            {
                case Operation.Add:
                    p.AddFloat(property.ToString(), a, false);
                    break;
                case Operation.Set:
                    p.SetFloat(property.ToString(), a, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Undefined operation to change room property: {property}.");
            }
            return DefaultResult;
        }
    }
}