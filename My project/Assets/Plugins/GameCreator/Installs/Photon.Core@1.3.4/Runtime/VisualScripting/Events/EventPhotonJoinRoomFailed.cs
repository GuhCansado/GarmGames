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

    [Title("On Photon Join Room Failed")]
    [Category("Photon/Matchmaking/On Join Room Failed")]
    [Description("Called when a previous OpJoinRoom call failed on the server.")]

    [Image(typeof(IconHome), ColorTheme.Type.Red)]

    [Keywords("Matchmaking", "Network", "Photon", "Room")]

    [Serializable]
    public class EventPhotonJoinRoomFailed : Event, IMatchmakingCallbacks
    {
        [SerializeField] private PropertySetNumber setReturnCode = new();
        [SerializeField] private PropertySetString setMessage = new();
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
        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
        { 
            setReturnCode.Set(returnCode, Self);
            setMessage.Set(message, Self);
            _ = m_Trigger.Execute(Self);
        }
        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message) {}
        void IMatchmakingCallbacks.OnLeftRoom() {}
    }
}