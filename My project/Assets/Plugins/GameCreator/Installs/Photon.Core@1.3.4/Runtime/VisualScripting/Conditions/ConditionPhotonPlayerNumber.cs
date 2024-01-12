using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Player Number")]
    [Description("Compare the target player number.")]

    [Category("Photon/Player/Player Number")]

    [Keywords("Network", "Photon", "Connection")]
    
    [Image(typeof(IconNumber), ColorTheme.Type.Teal)]
    
    [Serializable]
    public class ConditionPhotonPlayerNumber : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject target = new();
        [SerializeField] private CompareInteger compareTo = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Player Number is {compareTo}";
        
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
            int playerNumber = photonView.Owner.GetPlayerNumber();
            return compareTo.Match(playerNumber, args);
        }
    }
}