using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.Photon.Runtime
{

    [Title("On Ownership Transfered")]
    [Category("Photon/Core/On Ownership Transfered")]
    [Description("Called when ownership of a PhotonView is transfered to another player.")]

    [Image(typeof(IconEye), ColorTheme.Type.Green)]

    [Keywords("Player", "Network", "Photon", "Connection", "Photon View", "Transfered", "Ownership")]

    [Serializable]
    public class EventPhotonOnOwnershipTransfered : Event, IPunOwnershipCallbacks
    {
        [SerializeField] private PropertyGetGameObject target = GetGameObjectNone.Create();
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            if(PhotonNetwork.NetworkingClient != null) PhotonNetwork.AddCallbackTarget(this);
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            if (PhotonNetwork.NetworkingClient != null) PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            var p = target.Get(Self).GetCachedPhotonView();
            if (p != targetView) return;
            _ = m_Trigger.Execute(targetView.Owner.TagObject as GameObject);
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
        }
    }
}
