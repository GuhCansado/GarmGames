using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.Photon.Runtime
{

    [Title("On Photon Connected to Master")]
    [Category("Photon/Connection/On Photon Connected to Master")]
    [Description("Called when the client is connected to the Master Server and ready for matchmaking and other tasks.")]

    [Image(typeof(IconComputer), ColorTheme.Type.Blue)]

    [Keywords("Create", "Network", "Photon", "Room")]

    [Serializable]
    public class EventPhotonConnectedToMaster : Event, IConnectionCallbacks
    {
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
        public void OnConnected() {}
        public void OnConnectedToMaster() { _ = m_Trigger.Execute(Self); }
        public void OnDisconnected(DisconnectCause cause) {}
        public void OnRegionListReceived(RegionHandler regionHandler) {}
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) {}
        public void OnCustomAuthenticationFailed(string debugMessage) {}
    }
}
