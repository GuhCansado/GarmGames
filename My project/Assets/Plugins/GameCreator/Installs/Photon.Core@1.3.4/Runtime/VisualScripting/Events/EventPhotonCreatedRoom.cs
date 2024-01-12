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

    [Title("On Photon Created Room")]
    [Category("Photon/Matchmaking/On Created Room")]
    [Description("Called when this client created a room and entered it. OnJoinedRoom() will be called as well.")]

    [Image(typeof(IconHome), ColorTheme.Type.Green)]

    [Keywords("Create", "Network", "Photon", "Room")]

    [Serializable]
    public class EventPhotonCreatedRoom : Event, IMatchmakingCallbacks
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
        void IMatchmakingCallbacks.OnCreatedRoom() {_ = m_Trigger.Execute(Self); }
        void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnJoinedRoom() {}
        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnLeftRoom() {}
    }
}