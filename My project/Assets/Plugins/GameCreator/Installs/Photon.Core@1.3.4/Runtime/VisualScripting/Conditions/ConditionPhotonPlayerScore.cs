using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Player Score")]
    [Description("Compare the target player score.")]

    [Category("Photon/Player/Player Score")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.Purple)]

    [Serializable]
    public class ConditionPhotonPlayerScore : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject target = new();
        [SerializeField] private CompareInteger compareTo = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Player Score is {compareTo}";
        
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

            int playerScore = photonView.Owner.GetScore();
            return compareTo.Match(playerScore, args);
        }
    }
}