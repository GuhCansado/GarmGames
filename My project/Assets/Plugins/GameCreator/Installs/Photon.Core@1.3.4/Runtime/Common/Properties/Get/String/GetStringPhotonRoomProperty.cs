using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Room Property")]
    [Category("Photon/Room Property")]

    [Image(typeof(IconHome), ColorTheme.Type.Green)]
    [Description("Returns the specified room property value.")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetStringPhotonRoomProperty : PropertyTypeGetString
    {
        [SerializeField] private RoomPropertyName propertyName;

        public override string Get(Args args) =>
            PhotonNetwork.CurrentRoom.GetProperty(propertyName.ToString()).ToString();
        public override string String => propertyName.Value != null ? propertyName.ToString() : "(none)";
    }
}