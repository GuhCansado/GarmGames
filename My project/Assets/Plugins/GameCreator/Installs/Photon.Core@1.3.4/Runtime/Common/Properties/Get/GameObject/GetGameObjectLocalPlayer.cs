using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Local Player")]
    [Category("Photon/Local Player")]
    
    [Image(typeof(IconPlayer), ColorTheme.Type.Green)]
    [Description("Reference to the local Player gameObject.")]

    [Serializable]
    public class GetGameObjectLocalPlayer : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args)
        {
            var go = PhotonNetwork.LocalPlayer.TagObject as GameObject;
            if (go) return go;
            return args.Target ? args.Target : args.Self;
        }

        public override GameObject Get(GameObject gameObject)
        {
            var go = PhotonNetwork.LocalPlayer.TagObject as GameObject;
            if (go) return go;
            return gameObject;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectLocalPlayer instance = new GetGameObjectLocalPlayer();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Local Player";
    }
}