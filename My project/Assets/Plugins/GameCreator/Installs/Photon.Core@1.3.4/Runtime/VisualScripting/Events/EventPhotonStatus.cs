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

    [Title("On Photon Status")]
    [Category("Photon/Connection/On Photon Status")]
    [Description("Called when Photon connection status changes.")]

    [Image(typeof(IconCheckmark), ColorTheme.Type.Green)]

    [Keywords("Connection", "Network", "Photon", "Status")]

    [Serializable]
    public class EventPhotonStatus : Event
    {
        [SerializeField] private bool triggerOnStatus;
        [SerializeField, ConditionEnable("triggerOnStatus")] private ClientState triggerStatus = ClientState.Authenticated;
        [SerializeField] private PropertySetString setStatus = new();

        private ClientState _lastState;

        protected override void OnAwake(Trigger trigger)
        {
            base.OnAwake(trigger);
            SetStatus(ClientState.Disconnected);
        }

        protected override void OnUpdate(Trigger trigger)
        {
            if(_lastState != PhotonNetwork.NetworkClientState)
            {
                _lastState = PhotonNetwork.NetworkClientState;
                SetStatus(_lastState);
            }
        }

        private void SetStatus(ClientState state)
        {
            setStatus.Set(state.ToString(), Self);
            if (triggerOnStatus)
            {
                if(state == triggerStatus) _ = m_Trigger.Execute(Self);
            }
            else _ = m_Trigger.Execute(Self);
        }
    }
}
