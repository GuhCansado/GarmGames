using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Player Nickname")]
    [Description("Compare the target player nickname.")]

    [Category("Photon/Player/Player Property String")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconString), ColorTheme.Type.Purple)]
    
    [Serializable]
    public class ConditionPhotonPlayerName : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject target = new();
        [SerializeField] private CompareString compareTo = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Player ({target}) Nickname {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            var player = target.Get(args).GetPlayerFromView();
            return player != null && compareTo.Match(player.NickName, args);
        }
    }
}