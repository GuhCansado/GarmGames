using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Player Property")]
    [Category("Photon/Player Property")]

    [Image(typeof(IconPlayer), ColorTheme.Type.Blue)]
    [Description("Returns the specified player property value.")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetStringPhotonPlayerProperty : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject player = GetGameObjectLocalPlayer.Create();
        [SerializeField] private PlayerPropertyName propertyName;

        public override string Get(Args args)
        {
            var p = player.Get(args).GetPlayerFromView();
            if (p.HasProperty(propertyName.ToString()))
            {
                return p.GetProperty(propertyName.ToString()).ToString();
            }
            return string.Empty;
        }
        public override string String => propertyName.Value != null ? propertyName.ToString() : "(none)";
    }
}