using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Photon Network Time")]
    [Category("Time/Photon Network Time")]
    
    [Image(typeof(IconTimer), ColorTheme.Type.Green, typeof(OverlayDot))]
    [Description("Photon network time, synced with the server.")]

    [Keywords("Float", "Decimal", "Double", "Increment", "Photon", "Network")]
    [Serializable]
    public class GetPhotonNetworkTime : PropertyTypeGetDecimal
    {
        public override double Get(Args args) => PhotonNetwork.Time;
        public override double Get(GameObject gameObject) => PhotonNetwork.Time;
        
        public GetPhotonNetworkTime() : base()
        { }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(new GetPhotonNetworkTime());

        public override string String => "Photon Network Time";
    }
}