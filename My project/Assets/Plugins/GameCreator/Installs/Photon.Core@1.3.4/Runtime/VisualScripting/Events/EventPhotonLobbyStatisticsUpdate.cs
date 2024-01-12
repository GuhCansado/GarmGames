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

    [Title("On Photon Lobby Statistics Update")]
    [Category("Photon/Lobby/On Lobby Statistics Update")]
    [Description("Called when the Master Server sent an update for the Lobby Statistics.")]

    [Image(typeof(IconRefresh), ColorTheme.Type.Purple)]

    [Keywords("Lobby", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonLobbyStatisticsUpdate : Event, ILobbyCallbacks
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
        public void OnJoinedLobby() {}
        public void OnLeftLobby() {}
        public void OnRoomListUpdate(List<RoomInfo> roomList) {}
        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) { _ = m_Trigger.Execute(Self); }
    }
}
