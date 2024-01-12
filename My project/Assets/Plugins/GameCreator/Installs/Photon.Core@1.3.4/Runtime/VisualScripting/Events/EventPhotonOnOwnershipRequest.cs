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

    [Title("On Ownership Request")]
    [Category("Photon/Core/On Ownership Request")]
    [Description("Called when another player requests ownership of a PhotonView. "+
        "Called on all clients, so check if (targetView.IsMine) or (targetView.Owner == PhotonNetwork.LocalPlayer) "+
        "to determine if a targetView.TransferOwnership(requestingPlayer) response should be given.")]

    [Image(typeof(IconEye), ColorTheme.Type.Yellow)]

    [Keywords("Player", "Network", "Photon", "Connection", "Photon View", "Request", "Ownership")]

    [Serializable]
    public class EventPhotonOnOwnershipRequest : Event, IPunOwnershipCallbacks
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
            var p = target.Get(Self).GetCachedPhotonView();
            if (p != targetView) return;
            _ = m_Trigger.Execute(requestingPlayer.TagObject as GameObject);
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
        }
    }
}
