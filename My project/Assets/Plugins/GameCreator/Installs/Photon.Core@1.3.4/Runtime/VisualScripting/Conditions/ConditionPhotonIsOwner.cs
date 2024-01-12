using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Comparers;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Is Owner")]
    [Description("True if the Target is owner of the Photon Viewand can be controlled by this client.")]

    [Category("Photon/Core/Is Owner")]

    [Keywords("Network", "Photon", "Photon View", "Is Owner")]
    
    [Image(typeof(IconEye), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionPhotonIsOwner : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        [SerializeField] private PropertyGetGameObject targetOwner = new();
        [SerializeField] private PropertyGetGameObject targetView = new();
        [SerializeField] private CompareBoolean compareTo = new(true);
        protected override string Summary => $"Is {targetOwner} Owner of {targetView}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            var go = targetView.Get(args);
            var photonView = go.Get<PhotonView>();
            Debug.Log($"IsOwner: {photonView.Owner == targetOwner.Get(args).GetPlayerFromView()} owner: {photonView.Owner} target: {targetOwner.Get(args).GetPlayerFromView()}");
            if(!photonView)
            {
// #if UNITY_EDITOR
//                 Debug.LogError($"The object {go} doesn't have PhotonView component.", go);
// #endif
                return false;
            }
            return compareTo.Match(photonView.Owner == targetOwner.Get(args).GetPlayerFromView(), args);
        }
    }
}