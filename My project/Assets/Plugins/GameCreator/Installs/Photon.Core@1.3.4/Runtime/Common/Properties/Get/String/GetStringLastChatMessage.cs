using System;
using GameCreator.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Last Chat Message")]
    [Category("Photon/Last Chat Message")]

    [Image(typeof(IconUIText), ColorTheme.Type.Green)]
    [Description("Returns the last chat message from the specified player.")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetStringLastChatMessage : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject fromPlayer = GetGameObjectLastChatPlayer.Create();

        public override string Get(Args args)
        {
            var p = fromPlayer.Get(args);
            if (!p) return string.Empty;
            var pl = p.GetPlayerFromView();
            if (pl == null)
            {
                Debug.Log($"Couldn't find photon view on player {fromPlayer}");
                return string.Empty;
            }
            RoomChat.Instance.LastMessages.TryGetValue(pl, out var message);
            return message ?? string.Empty;
        }
        public override string String => $"Last Chat Message from {fromPlayer}";
    }
}