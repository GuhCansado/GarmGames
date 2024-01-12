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

    [Title("On Photon Connected")]
    [Category("Photon/Connection/On Photon Connected")]
    [Description("Called to signal that the \"low level connection\" got established but before the client can call operation on the server.")]

    [Image(typeof(IconComputer), ColorTheme.Type.Green, typeof(OverlayTick))]

    [Keywords("Create", "Network", "Photon", "Room")]

    [Serializable]
    public class EventPhotonConnected : Event, IConnectionCallbacks
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

        public void OnConnected()
        {
            _ = m_Trigger.Execute(Self);
        }

        public void OnConnectedToMaster() {}
        public void OnDisconnected(DisconnectCause cause) {}
        public void OnRegionListReceived(RegionHandler regionHandler) {}
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) {}
        public void OnCustomAuthenticationFailed(string debugMessage) {}
    }
}
