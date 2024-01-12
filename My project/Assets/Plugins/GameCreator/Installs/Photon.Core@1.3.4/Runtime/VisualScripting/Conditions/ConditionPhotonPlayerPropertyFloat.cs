using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Player Property Float")]
    [Description("Compare the target player property.")]

    [Category("Photon/Player/Player Property Float")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionPhotonPlayerPropertyFloat : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject target = new();
        [SerializeField] private PlayerPropertyName property = new();
        [SerializeField] private CompareDouble compareTo = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Player Property ({(string.IsNullOrEmpty(property.ToString()) ? "none": property.ToString())}) {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            var go = target.Get(args);
            var photonView = go.Get<PhotonView>();
            if(!photonView)
            {
#if UNITY_EDITOR
                Debug.LogError($"The object {go} doesn't have PhotonView component.", go);
#endif
                return false;
            }
            var player = photonView.Owner;
            return compareTo.Match(player.GetFloat(property.ToString()), args);
        }
    }
}