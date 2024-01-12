using System;
using GameCreator.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Last Joined Player")]
    [Category("Photon/Last Joined Player")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Green, typeof(OverlayArrowDown))]
    [Description("Reference to the last player who joined the room.")]

    [Serializable]
    public class GetGameObjectLastJoinedPlayer : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args)
        {
            if(PhotonNetworkManager.Instance && PhotonNetworkManager.Instance.LastJoinedPlayer != null)
            {
                var go = PhotonNetworkManager.Instance.LastJoinedPlayer.TagObject as GameObject;
                if (go) return go;
            }
            return null;
        }

        public static PropertyGetGameObject Create()
        {
            var instance = new GetGameObjectLastJoinedPlayer();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Last Joined Player";
    }
}