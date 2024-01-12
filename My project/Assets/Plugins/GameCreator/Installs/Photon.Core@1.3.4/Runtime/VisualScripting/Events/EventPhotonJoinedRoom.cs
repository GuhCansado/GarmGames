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

    [Title("On Photon Joined Room")]
    [Category("Photon/Matchmaking/On Joined Room")]
    [Description("Called when the LoadBalancingClient entered a room, no matter if this client created it or simply joined.")]

    [Image(typeof(IconHome), ColorTheme.Type.Green, typeof(OverlayTick))]

    [Keywords("Matchmaking", "Network", "Photon", "Room")]

    [Serializable]
    public class EventPhotonJoinedRoom : Event, IMatchmakingCallbacks
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

        void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList) {}
        void IMatchmakingCallbacks.OnCreatedRoom() {}
        void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnJoinedRoom() { _ = m_Trigger.Execute(Self); }
        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnLeftRoom() {}
    }
}