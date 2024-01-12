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

    [Title("On Photon Joined Lobby")]
    [Category("Photon/Lobby/On Joined Lobby")]
    [Description("Called on entering a lobby on the Master Server. The actual room-list updates will call OnRoomListUpdate.")]

    [Image(typeof(IconComputer), ColorTheme.Type.Purple, typeof(OverlayTick))]

    [Keywords("Lobby", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonJoinedLobby : Event, ILobbyCallbacks
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
        public void OnJoinedLobby()
        {
            _ = m_Trigger.Execute(Self);
        }

        public void OnLeftLobby() {}
        public void OnRoomListUpdate(List<RoomInfo> roomList) {}
        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) {}
    }
}
