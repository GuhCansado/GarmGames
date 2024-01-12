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

    [Title("On Photon Friend List Update")]
    [Category("Photon/Matchmaking/On Friend List Update")]
    [Description("Called when this client created a room and entered it. OnJoinedRoom() will be called as well.")]

    [Image(typeof(IconCharacter), ColorTheme.Type.Purple, typeof(OverlayTick))]

    [Keywords("Error", "Network", "Photon", "Room")]

    [Serializable]
    public class EventPhotonFriendListUpdate : Event, IMatchmakingCallbacks
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

        void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList) { _ = m_Trigger.Execute(Self); }
        void IMatchmakingCallbacks.OnCreatedRoom() {}
        void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnJoinedRoom() {}
        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnLeftRoom() {}
    }
}