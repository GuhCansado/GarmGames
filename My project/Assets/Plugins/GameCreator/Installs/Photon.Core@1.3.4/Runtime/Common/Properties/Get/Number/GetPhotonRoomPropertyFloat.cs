using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Photon Room Float")]
    [Category("Photon/Room/Room Property Float")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.Green)]
    [Description("Photon network time, synced with the server.")]

    [Keywords("Float", "Decimal", "Double", "Increment", "Photon", "Network")]
    [Serializable]
    public class GetPhotonRoomPropertyFloat : PropertyTypeGetDecimal
    {
        [SerializeField] private RoomPropertyName property = new();

        public override double Get(Args args) => PhotonNetwork.CurrentRoom.GetFloat(property.ToString());
        public override double Get(GameObject gameObject) => PhotonNetwork.CurrentRoom.GetFloat(property.ToString());
        
        public GetPhotonRoomPropertyFloat() : base()
        { }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(new GetPhotonRoomPropertyFloat());

        public override string String => $"Room Float {property.ToString()}";
    }
}