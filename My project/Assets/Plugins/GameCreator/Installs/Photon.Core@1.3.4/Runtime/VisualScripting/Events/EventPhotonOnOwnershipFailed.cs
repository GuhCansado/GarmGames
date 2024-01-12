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

    [Title("On Ownership Transfer Failed")]
    [Category("Photon/Core/On Ownership Transfer Failed")]
    [Description("Called when ownership of a PhotonView is transfered to another player.")]

    [Image(typeof(IconEye), ColorTheme.Type.Red)]

    [Keywords("Player", "Network", "Photon", "Connection", "Photon View", "Transfered", "Ownership")]

    [Serializable]
    public class EventPhotonOnOwnershipFailed : Event, IPunOwnershipCallbacks
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
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
            Debug.Log($"OnOwnershipTransferFailed targetView: {targetView} targetRequested: {target.Get(Self).GetCachedPhotonView()} senderOfFailedRequest: {senderOfFailedRequest} isMine: {targetView.IsMine} owner: {targetView.Owner} local: {PhotonNetwork.LocalPlayer}");
            // if(targetView.IsMine) return;
            var p = target.Get(Self).GetCachedPhotonView();
            if (p != targetView) return;
            _ = m_Trigger.Execute(senderOfFailedRequest.TagObject as GameObject);
        }
    }
}
