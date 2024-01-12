using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Room Name")]
    [Category("Photon/Room Name")]

    [Image(typeof(IconHome), ColorTheme.Type.Green)]
    [Description("Returns the name of the room.")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetStringPhotonRoomName : PropertyTypeGetString
    {
        public override string Get(Args args) => PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name : string.Empty;
        public override string String => PhotonNetwork.InRoom ? $"Room: {PhotonNetwork.CurrentRoom.Name}" : "(none)";
    }
}