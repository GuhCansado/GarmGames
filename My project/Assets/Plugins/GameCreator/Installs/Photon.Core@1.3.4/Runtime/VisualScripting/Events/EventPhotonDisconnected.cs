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

    [Title("On Photon Disconnected")]
    [Category("Photon/Connection/On Disconnected")]
    [Description("Called after disconnecting from the Photon server. It could be a failure or an explicit disconnect call.")]

    [Image(typeof(IconComputer), ColorTheme.Type.Red)]

    [Keywords("Disconnect", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonDisconnected : Event, IConnectionCallbacks
    {
        [SerializeField] private bool triggerOnCause;
        [SerializeField, ConditionEnable("triggerOnCause")] private DisconnectCause triggerCause = DisconnectCause.ServerTimeout;
        [SerializeField] private PropertySetString setDisconnectCause = new();

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
        public void OnConnectedToMaster() {}

        public void OnDisconnected(DisconnectCause cause)
        {
            setDisconnectCause.Set(cause.ToString(), Self);
            if (triggerOnCause)
            {
                if(cause == triggerCause) _ = m_Trigger.Execute(Self);
            }
            else _ = m_Trigger.Execute(Self);
        }
        public void OnRegionListReceived(RegionHandler regionHandler) {}
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) {}
        public void OnCustomAuthenticationFailed(string debugMessage) {}
    }
}
