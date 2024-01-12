using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Player Number")]
    [Category("Photon/Player Number")]

    [Image(typeof(IconNumber), ColorTheme.Type.Teal)]
    [Description("Returns the specified player number.")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetStringPhotonPlayerNumber : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject player = GetGameObjectLocalPlayer.Create();

        public override string Get(Args args)
        {
            var p = player.Get(args).GetPlayerFromView();
            return p.GetPlayerNumber().ToString();
        }
        public override string String => $"{player} Number";
    }
}