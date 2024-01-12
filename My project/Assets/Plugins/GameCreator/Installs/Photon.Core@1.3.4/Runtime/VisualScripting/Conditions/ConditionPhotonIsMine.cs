using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Is Mine")]
    [Description("True if the PhotonView is \"mine\" and can be controlled by this client.")]

    [Category("Photon/Core/Is Mine")]

    [Keywords("Network", "Photon", "Is Mine", "Mine")]
    
    [Image(typeof(IconPlayer), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionPhotonIsMine : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField] private PropertyGetGameObject target = new();
        [SerializeField] private CompareBoolean compareTo = new(true);
        protected override string Summary => $"Is {target} Mine {compareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            var go = target.Get(args);
            var photonView = go.Get<PhotonView>();
            if(!photonView)
            {
// #if UNITY_EDITOR
//                 Debug.LogError($"The object {go} doesn't have PhotonView component.", go);
// #endif
                return false;
            }
            return compareTo.Match(photonView.IsMine, args);
        }
    }
}