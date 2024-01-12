using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("RPC Sender")]
    [Category("Photon/RPC Sender")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    [Description("Reference to the last player who sent an RPC.")]

    [Serializable]
    public class GetGameObjectRPCSender : PropertyTypeGetGameObject
    {
        public PropertyGetGameObject fallbackTo = GetGameObjectLocalPlayer.Create();

        public override GameObject Get(Args args)
        {
            if (args.Target != null && args.Target != args.Self) return args.Target;
            return fallbackTo.Get(args);
        }

        public override GameObject Get(GameObject gameObject)
        {
            return gameObject != null ? gameObject : fallbackTo.Get(gameObject);
        }

        public static PropertyGetGameObject Create()
        {
            var instance = new GetGameObjectRPCSender();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Last RPC Sender";
    }
}