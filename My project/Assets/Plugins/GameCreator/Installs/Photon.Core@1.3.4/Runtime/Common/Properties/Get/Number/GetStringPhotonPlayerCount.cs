using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Player Count")]
    [Category("Photon/Player Count")]

    [Image(typeof(IconCharacter), ColorTheme.Type.Blue, typeof(OverlayDot))]
    [Description("Get the number of players in the current room.")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetStringPhotonPlayerCount : PropertyTypeGetString
    {
        public override string Get(Args args) => PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.PlayerCount.ToString() : "0";
        public override string String => PhotonNetwork.InRoom ? $"Players: {PhotonNetwork.CurrentRoom.PlayerCount}" : "(none)";
    }
}