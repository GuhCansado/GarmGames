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

    [Title("On Photon Left Room")]
    [Category("Photon/Matchmaking/On Left Room")]
    [Description("Called when the local user/client left a room, so the game's logic can clean up it's internal state.")]

    [Image(typeof(IconHome), ColorTheme.Type.Yellow, typeof(OverlayArrowLeft))]

    [Keywords("Matchmaking", "Network", "Photon", "Room")]

    [Serializable]
    public class EventPhotonLeftRoom : Event, IMatchmakingCallbacks
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
        void IMatchmakingCallbacks.OnJoinedRoom() {}
        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnLeftRoom() { _ = m_Trigger.Execute(Self); }
    }
}