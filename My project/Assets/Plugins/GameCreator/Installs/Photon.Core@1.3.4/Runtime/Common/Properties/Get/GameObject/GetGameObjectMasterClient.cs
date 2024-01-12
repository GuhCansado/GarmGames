using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Master Client")]
    [Category("Photon/Master Client")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Blue)]
    [Description("Reference to the master client gameObject.")]

    [Serializable]
    public class GetGameObjectMasterClient : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args)
        {
            var go = PhotonNetwork.MasterClient.TagObject as GameObject;
            if (go) return go;
            return args.Target ? args.Target : args.Self;
        }

        public override GameObject Get(GameObject gameObject)
        {
            var go = PhotonNetwork.MasterClient.TagObject as GameObject;
            if (go) return go;
            return gameObject;
        }

        public static PropertyGetGameObject Create()
        {
            var instance = new GetGameObjectMasterClient();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Master Client";
    }
}