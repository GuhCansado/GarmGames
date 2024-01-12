using System;
using GameCreator.Runtime.Common;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Player Nickname")]
    [Category("Photon/Player Nickname")]

    [Image(typeof(IconString), ColorTheme.Type.Purple)]
    [Description("Returns the specified player nickname.")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetStringPhotonPlayerName : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject player = GetGameObjectLocalPlayer.Create();

        public override string Get(Args args)
        {
            var p = player.Get(args);
            if(!p)
            {
                // Debug.Log($"Couldn't find player {player}");
                return PhotonNetwork.NickName;
            }
            var view = p.GetPlayerFromView();
            if(view == null) Debug.Log($"Couldn't find photon view on player {player}");
            return view == null ? string.Empty : view.NickName;
        }
        public override string String => $"{player} Nickname";
    }
}