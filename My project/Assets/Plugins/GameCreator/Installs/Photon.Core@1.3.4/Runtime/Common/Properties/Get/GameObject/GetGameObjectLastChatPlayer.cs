using System;
using GameCreator.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Last Chat Player")]
    [Category("Photon/Last Chat Player")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Green, typeof(OverlayTick))]
    [Description("Reference to the last player who sent a chat message.")]

    [Serializable, HideLabelsInEditor]
    public class GetGameObjectLastChatPlayer : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args)
        {
            if(RoomChat.Instance && RoomChat.Instance.LastPlayer != null)
            {
                var go = RoomChat.Instance.LastPlayer.TagObject as GameObject;
                if (go) return go;
            }
            return args.Target ? args.Target : args.Self;
        }

        public static PropertyGetGameObject Create()
        {
            var instance = new GetGameObjectLastChatPlayer();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Last Joined Player";
    }
}